using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("UI Elements")]
    public TextMeshProUGUI speakerText;
    public Image dialogue;
    public TextMeshProUGUI dialogueText;

    public GameObject choisesContainer;
    public GameObject choisePrefab;

    private float minimumTimeInNode = 0.5f;

    [Header("User-Dialogue information")]
    public bool inDialogueMode = false;

    [Header("Dialogue Information")]
    private D_Dialogue currentDialogue;
    private D_Dialogue.DialogueNode currentNode;
    public int currentNodeIndex = 0;
    private bool nodeHasChoises;
    private List<D_Dialogue.DialogueChoise> currentChoises;
    private IEnumerator currentDialogueCoroutine;

    [Header("Node Information")]
    private float nodeSpeed = 0.05f;

    private float timeInNode;

    public bool nodeIsFinish;

    [Header("Stand-by Dialogue Information")]
    public bool hasDialogueInStandBy = false;
    private D_Dialogue dialogueInStandBy;
    private int nodeIndexInStandBy;

    private void OnEnable()
    {
        EventManager.StartListening("onGoToNextNode", TryGoToNextNode);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onGoToNextNode", TryGoToNextNode);
    }

    void Update()
    {
        if (inDialogueMode)
            timeInNode += Time.deltaTime;
    }

    public void StartDialogueMode(D_Dialogue dialogue)
    {
        if (inDialogueMode)
            return;

        inDialogueMode = true;
        hasDialogueInStandBy = false;
        //        inputHandler.ChangePlayerMapState(false); TODO Change how I make this

        currentDialogue = dialogue;
        currentNode = currentDialogue.dialogueNodes[currentNodeIndex];

        UpdateDialogueUI(
            dialogue.speakerName,
            currentNode.dialogueText,
            currentNode.dialogueChoises
        );
    }

    public void UpdateDialogueUI(
        string speaker,
        string newNode,
        List<D_Dialogue.DialogueChoise> newNodeChoises
    )
    {
        nodeIsFinish = false;

        nodeHasChoises = newNodeChoises.Count != 0;
        dialogueText.text = " ";

        speakerText.text = speaker;
        currentChoises = newNodeChoises;

        currentDialogueCoroutine = DisplayNode(newNode);
        StartCoroutine(currentDialogueCoroutine);
    }

    private IEnumerator DisplayNode(string text)
    {
        dialogueText.text = "";

        foreach (char character in text)
        {
            dialogueText.text += character;

            yield return new WaitForSeconds(nodeSpeed);
        }

        if (nodeHasChoises)
        {
            foreach (D_Dialogue.DialogueChoise choise in currentChoises)
            {
                GameObject _newChoiseObj = InstantiateChoise(choise);

                TextMeshProUGUI _currentChoiseText = _newChoiseObj.transform
                    .Find("Text")
                    .gameObject.GetComponent<TextMeshProUGUI>();

                foreach (char character in choise.choiseText)
                {
                    _currentChoiseText.text += character;
                    yield return new WaitForSeconds(nodeSpeed);
                }
            }

            choisesContainer.transform.GetChild(0).GetComponent<Button>().Select();
        }

        nodeIsFinish = true;
    }

    public GameObject InstantiateChoise(D_Dialogue.DialogueChoise newChoise)
    {
        GameObject _newChoise = Instantiate(choisePrefab);

        _newChoise.transform.SetParent(choisesContainer.transform, false);

        Button _newChoiseButton = _newChoise.GetComponent<Button>();

        _newChoiseButton.onClick.AddListener(
            delegate
            {
                SelectChoise(newChoise.choiseIndex);
            }
        );

        if (newChoise.onlyReadbleOnce && newChoise.choiseWasAlreadyChoosen)
            _newChoiseButton.GetComponent<Button>().interactable = false;

        return _newChoise;
    }

    public void SelectChoise(int choiseIndex)
    {
        D_Dialogue.DialogueChoise _currentChoise = currentNode.dialogueChoises[choiseIndex];
        _currentChoise.choiseWasAlreadyChoosen = true;

        if (_currentChoise.followOnDialogue == null)
        {
            ExitDialogueMode();
            return;
        }

        ChangeCurrentDialogue(_currentChoise.followOnDialogue);
    }

    public void TryGoToNextNode(Dictionary<string, object> message)
    {
        if (timeInNode <= minimumTimeInNode)
            return;

        if (!nodeIsFinish)
        {
            FinishDisplayNode();
            return;
        }

        if (nodeHasChoises)
            return;

        if (currentNodeIndex + 1 >= currentDialogue.dialogueNodes.Count)
        {
            if (currentDialogue.followOnDialogue == null)
                ExitDialogueMode();
            else
                ChangeCurrentDialogue(
                    currentDialogue.followOnDialogue,
                    currentDialogue.startFollowOnDialogueIndex
                );

            return;
        }

        currentNodeIndex++;
        currentNode = currentDialogue.dialogueNodes[currentNodeIndex];

        UIManager.Instance.LoadDialogueUIBasedOnChoises(currentNode.dialogueChoises.Count != 0);

        UpdateDialogueUI(
            currentDialogue.speakerName,
            currentNode.dialogueText,
            currentNode.dialogueChoises
        );

        timeInNode = 0;
    }

    public void ChangeCurrentDialogue(D_Dialogue newDialogue, int newDialogueStartIndex = 0)
    {
        currentDialogue = newDialogue;
        currentNodeIndex = newDialogueStartIndex;

        currentNode = currentDialogue.dialogueNodes[currentNodeIndex];

        UIManager.Instance.LoadDialogueUIBasedOnChoises(currentNode.dialogueChoises.Count != 0);

        UpdateDialogueUI(
            currentDialogue.speakerName,
            currentNode.dialogueText,
            currentNode.dialogueChoises
        );
    }

    public void FinishDisplayNode()
    {
        StopCoroutine(currentDialogueCoroutine);

        dialogueText.text = currentNode.dialogueText;

        if (nodeHasChoises)
        {
            DestroyPreviousChoises();

            foreach (D_Dialogue.DialogueChoise choise in currentChoises)
            {
                GameObject _newChoiseObj = InstantiateChoise(choise);

                TextMeshProUGUI _currentChoiseText = _newChoiseObj.transform
                    .Find("Text")
                    .gameObject.GetComponent<TextMeshProUGUI>();

                _currentChoiseText.text = choise.choiseText;
            }

            choisesContainer.transform.GetChild(0).GetComponent<Button>().Select();
        }

        nodeIsFinish = true;
        timeInNode = 0;
    }

    private void DestroyPreviousChoises()
    {
        foreach (Transform childChoises in choisesContainer.transform)
        {
            Destroy(childChoises.gameObject);
        }
    }

    public void ExitDialogueMode()
    {
        inDialogueMode = false;
        currentNodeIndex = 0;

        EventManager.TriggerEvent(
            "onDialogue",
            true,
            new Dictionary<string, object> { { "activeState", false } }
        );
    }

    public void TryToStandByCurrentDialogue()
    {
        if (hasDialogueInStandBy)
            return;

        if (timeInNode <= minimumTimeInNode)
            return;

        if (!nodeIsFinish)
        {
            FinishDisplayNode();
            return;
        }

        hasDialogueInStandBy = true;

        dialogueInStandBy = currentDialogue;
        nodeIndexInStandBy = currentNodeIndex;

        ExitDialogueMode();
    }

    public void ResumeStandByDialogue()
    {
        timeInNode = 0;
        currentNodeIndex = nodeIndexInStandBy + 1;

        EventManager.TriggerEvent(
            "onDialogue",
            true,
            new Dictionary<string, object>
            {
                { "activeState", true },
                { "startNode", currentNodeIndex },
                { "dialogue", dialogueInStandBy }
            }
        );
    }
}
