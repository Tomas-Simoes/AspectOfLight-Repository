using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIDialogue : MonoBehaviour
{
    [Header("Others")]
    [SerializeField]
    private DialogueManager dialogueManager;

    [Header("UI Elements")]
    public TextMeshProUGUI speakerText;

    public Image dialogueWithChoises;
    public Image dialogueWithoutChoises;
    public TextMeshProUGUI withChoisesDialogueText;
    public TextMeshProUGUI withoutChoisesDialogueText;

    public GameObject choisesContainer;
    public GameObject choisePrefab;

    [Header("Dialogue Information")]
    private bool dialogueHasChoises;
    private string currentNode;
    private List<D_Dialogue.DialogueChoise> currentChoises;
    public bool isReadyForNewNode = true;

    public bool nodeIsFinish;

    [SerializeField]
    private float minimumTimeWhenNodeEnds = 0.5f;

    [SerializeField]
    private float timeSinceNodeEnded;

    [SerializeField]
    private float textSpeed = 0.2f;

    private IEnumerator currentCoroutine;

    void Update()
    {
        if (nodeIsFinish)
            timeSinceNodeEnded += Time.deltaTime;

        if (!isReadyForNewNode && timeSinceNodeEnded >= minimumTimeWhenNodeEnds)
        {
            isReadyForNewNode = true;
        }
    }

    public void UpdateUI(
        string speaker,
        string newNode,
        List<D_Dialogue.DialogueChoise> newDialogueChoises
    )
    {
        nodeIsFinish = false;
        isReadyForNewNode = false;
        timeSinceNodeEnded = 0;

        dialogueHasChoises = newDialogueChoises.Count != 0;

        GetDialogueText(dialogueHasChoises).text = " ";

        gameObject.SetActive(true);

        if (dialogueHasChoises)
        {
            dialogueWithChoises.gameObject.SetActive(true);
            dialogueWithoutChoises.gameObject.SetActive(false);
        }
        else
        {
            dialogueWithoutChoises.gameObject.SetActive(true);
            dialogueWithChoises.gameObject.SetActive(false);
        }

        speakerText.text = speaker;
        currentNode = newNode;
        currentChoises = newDialogueChoises;

        currentCoroutine = DisplayNode(currentNode);
        StartCoroutine(currentCoroutine);
    }

    private IEnumerator DisplayNode(string text)
    {
        foreach (char character in text)
        {
            GetDialogueText(dialogueHasChoises).text += character;

            yield return new WaitForSeconds(textSpeed);
        }

        if (dialogueHasChoises)
        {
            foreach (D_Dialogue.DialogueChoise choise in currentChoises)
            {
                GameObject _newChoiseObj = InstantiateChoise();

                TextMeshProUGUI _currentChoiseText = _newChoiseObj.transform
                    .Find("Text")
                    .gameObject.GetComponent<TextMeshProUGUI>();

                foreach (char character in choise.choiseText)
                {
                    _currentChoiseText.text += character;
                    yield return new WaitForSeconds(textSpeed);
                }

                _newChoiseObj
                    .GetComponent<Button>()
                    .onClick.AddListener(
                        delegate
                        {
                            dialogueManager.SelectChoise(choise.choiseIndex);
                        }
                    );
            }

            choisesContainer.transform.GetChild(0).GetComponent<Button>().Select();
        }

        nodeIsFinish = true;
    }

    public void FinishDisplayNode()
    {
        StopCoroutine(currentCoroutine);

        GetDialogueText(dialogueHasChoises).text = currentNode;

        if (dialogueHasChoises)
        {
            foreach (D_Dialogue.DialogueChoise choise in currentChoises)
            {
                GameObject _newChoiseObj = InstantiateChoise();

                TextMeshProUGUI _currentChoiseText = _newChoiseObj.transform
                    .Find("Text")
                    .gameObject.GetComponent<TextMeshProUGUI>();

                _currentChoiseText.text = choise.choiseText;

                _newChoiseObj
                    .GetComponent<Button>()
                    .onClick.AddListener(
                        delegate
                        {
                            dialogueManager.SelectChoise(choise.choiseIndex);
                        }
                    );
            }

            choisesContainer.transform.GetChild(0).GetComponent<Button>().Select();
        }

        nodeIsFinish = true;
    }

    public GameObject InstantiateChoise()
    {
        GameObject _newChoise = Instantiate(choisePrefab);

        _newChoise.transform.SetParent(choisesContainer.transform);
        _newChoise.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

        return _newChoise;
    }

    public void ExitUIDialogue()
    {
        gameObject.SetActive(false);
    }

    private TextMeshProUGUI GetDialogueText(bool hasChoises)
    {
        if (dialogueHasChoises)
            return withChoisesDialogueText;
        else
            return withoutChoisesDialogueText;
    }
}
