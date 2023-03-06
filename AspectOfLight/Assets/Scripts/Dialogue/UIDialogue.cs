using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDialogue : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI speakerText;

    public TextMeshProUGUI dialogueText;

    [Header("Dialogue Information")]
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

        dialogueText.text = " ";

        gameObject.SetActive(true);

        speakerText.text = speaker;
        currentNode = newNode;

        currentCoroutine = DisplayNode(currentNode);
        StartCoroutine(currentCoroutine);
    }

    private IEnumerator DisplayNode(string text)
    {
        foreach (char character in text)
        {
            dialogueText.text += character;
            yield return new WaitForSeconds(textSpeed);
        }

        nodeIsFinish = true;
    }

    public void FinishDisplayNode()
    {
        StopCoroutine(currentCoroutine);

        dialogueText.text = currentNode;
        nodeIsFinish = true;
    }

    public void ExitUIDialogue()
    {
        gameObject.SetActive(false);
    }
}
