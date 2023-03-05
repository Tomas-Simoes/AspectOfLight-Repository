using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Manager Information")]
    [SerializeField]
    private UIDialogue UIDialogue;

    [SerializeField]
    private float minimumTimeInNode = 1;

    [Header("Dialogue Information")]
    private D_Dialogue currentDialogue;
    private D_Dialogue.DialogueNode currentNode;
    private int currentNodeIndex = 0;

    public bool inDialogueMode = false;

    [SerializeField]
    private float timeInNode;

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

        currentDialogue = dialogue;
        currentNode = currentDialogue.dialogueNodes[currentNodeIndex];

        UIDialogue.UpdateUI(dialogue.speakerName, currentNode.dialogueText);
    }

    public void TryGoToNextNode()
    {
        if (timeInNode <= minimumTimeInNode)
            return;

        if (!UIDialogue.isReadyForNewNode)
        {
            UIDialogue.FinishDisplayNode();
            return;
        }

        if (currentNodeIndex + 1 >= currentDialogue.dialogueNodes.Count)
        {
            ExitDialogueMode();
            return;
        }

        currentNodeIndex++;
        currentNode = currentDialogue.dialogueNodes[currentNodeIndex];

        UIDialogue.UpdateUI(currentDialogue.speakerName, currentNode.dialogueText);

        timeInNode = 0;
    }

    public void ExitDialogueMode()
    {
        inDialogueMode = false;
        currentNodeIndex = 0;
        UIDialogue.ExitUIDialogue();
    }
}
