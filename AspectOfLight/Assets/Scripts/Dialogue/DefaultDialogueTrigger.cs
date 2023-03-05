using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultDialogueTrigger : DialogueTrigger
{
    protected override void OnTriggerStay2D(Collider2D other)
    {
        base.OnTriggerStay2D(other);
        if (other.CompareTag("Player") && player.inputHandler.pressedInteract)
        {
            if (!dialogueManager.inDialogueMode)
            {
                if (!readyToInteract)
                    return;

                dialogueManager.StartDialogueMode(dialogue);
                readyToInteract = false;
            }
            else
            {
                dialogueManager.TryGoToNextNode();
            }
        }
    }
}
