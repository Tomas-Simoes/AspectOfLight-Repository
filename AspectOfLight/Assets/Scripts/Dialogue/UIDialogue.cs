using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIDialogue : MonoBehaviour
{
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
    public bool isReadyForNewNode = true;

    private bool nodeIsFinish;

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
        List<D_Dialogue.DialogueChoise> dialogueChoises
    )
    {
        nodeIsFinish = false;
        isReadyForNewNode = false;
        timeSinceNodeEnded = 0;

        dialogueHasChoises = dialogueChoises.Count != 0;

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

        currentCoroutine = DisplayNode(currentNode, dialogueChoises);
        StartCoroutine(currentCoroutine);
    }

    private IEnumerator DisplayNode(string text, List<D_Dialogue.DialogueChoise> choises)
    {
        foreach (char character in text)
        {
            GetDialogueText(dialogueHasChoises).text += character;

            yield return new WaitForSeconds(textSpeed);
        }

        int _choiseIndex = 0;

        foreach (D_Dialogue.DialogueChoise choise in choises)
        {
            GameObject _newChoiseObj = Instantiate(choisePrefab);

            _newChoiseObj.transform.SetParent(choisesContainer.transform);
            _newChoiseObj.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

            TextMeshProUGUI currentChoiseText = _newChoiseObj.transform
                .Find("Text")
                .gameObject.GetComponent<TextMeshProUGUI>();

            foreach (char character in choise.choiseText)
            {
                currentChoiseText.text += character;
                yield return new WaitForSeconds(textSpeed);
            }

            _choiseIndex++;
        }

        nodeIsFinish = true;
    }

    public void FinishDisplayNode()
    {
        StopCoroutine(currentCoroutine);

        GetDialogueText(dialogueHasChoises).text = currentNode;
        nodeIsFinish = true;
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
