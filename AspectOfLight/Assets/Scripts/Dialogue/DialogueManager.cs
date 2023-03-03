using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private D_Dialogue.DialogueNode currentNode;
    private int currentNodeIndex = 0;

    public void StartDialogueMode(D_Dialogue dialogue)
    {
        currentNode = dialogue.dialogueNodes[currentNodeIndex];
    }

    public void GoToNextNode() { }

    public void ExitDialogueMode()
    {
        currentNodeIndex = 0;
    }
}
