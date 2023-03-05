using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArielDialogueTrigger : DialogueTrigger
{
    [Header("Ariel Dialogue Informations")]
    [SerializeField]
    private float minimumDistanceToTalk = 6;
    private float[] newDialogueDistance;

    protected virtual void OnTriggerStay2D()
    {
        if (!dialogueManager.inDialogueMode)
            dialogueManager.StartDialogueMode(dialogue);
        else
            dialogueManager.TryGoToNextNode();
    }

    void Update()
    {
        if (dialogueManager.inDialogueMode) { }
    }
}
