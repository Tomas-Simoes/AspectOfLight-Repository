using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultDialogueTrigger : DialogueTrigger
{
  void Update()
  {
    if (!dialogueManager.inDialogueMode)
      timeAfterDialogue += Time.deltaTime;
  }

  public override void OnTriggerStay2D(Collider2D other)
  {
    base.OnTriggerStay2D(other);


    if (other.CompareTag("Player") && player.inputHandler.pressedInteract)
    {
      if (!dialogueManager.inDialogueMode)
      {
        if (!readyToInteract)
          return;

        if (timeAfterDialogue <= minimumTimeAfterDialogue)
          return;

        dialogueManager.StartDialogueMode(dialogue);

        timeAfterDialogue = 0;
        readyToInteract = !onlyInteractOnce;
      }
      else
      {
        dialogueManager.TryGoToNextNode();
      }
    }
  }
}
