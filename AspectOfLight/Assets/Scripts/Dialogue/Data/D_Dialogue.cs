using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/New Dialogue")]
public class D_Dialogue : ScriptableObject
{
    [Header("Speaker Information")]
    public string speakerName;

    [Header("Dialogue")]
    public List<DialogueNode> dialogueNodes = new List<DialogueNode>();

    [Header("Next Dialogue (Optional)")]
    public D_Dialogue followOnDialogue;
    public int startFollowOnDialogueIndex;

    [System.Serializable]
    public class DialogueNode
    {
        public string dialogueText;
        public List<DialogueChoise> dialogueChoises;
    }

    [System.Serializable]
    public class DialogueChoise
    {
        public string choiseText;
        public int choiseIndex;
        public D_Dialogue followOnDialogue;
        public bool choiseWasAlreadyChoosen;
    }

    public void ChangeChoosenChoiseState(bool state, int nodeIndex, int choiseIndex)
    {
        dialogueNodes[nodeIndex].dialogueChoises[choiseIndex].choiseWasAlreadyChoosen = true;
    }
}
