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

    [System.Serializable]
    public struct DialogueNode
    {
        public string dialogueText;
        public List<DialogueChoise> dialogueChoises;
    }

    [System.Serializable]
    public struct DialogueChoise
    {
        public string choiseText;
        public D_Dialogue followOnDialogue;
        public bool choiseWasAlreadyChoosen;
    }
}
