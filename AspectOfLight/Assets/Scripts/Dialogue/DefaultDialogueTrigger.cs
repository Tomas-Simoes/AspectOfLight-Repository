using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultDialogueTrigger : DialogueTrigger
{
    // TODO Decouple the player from here
    public override void OnTriggerStay2D(Collider2D other)
    {
        base.OnTriggerStay2D(other);

        if (other.CompareTag("Player") && player.inputHandler.pressedInteract)
        {
            if (!DialogueManager.Instance.inDialogueMode)
            {
                StartDialogue(0);
            }
            else
                EventManager.TriggerEvent("onGoToNextNode", false, null);
        }
    }
}
