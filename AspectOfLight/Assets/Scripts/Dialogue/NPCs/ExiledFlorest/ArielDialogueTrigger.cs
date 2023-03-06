using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArielDialogueTrigger : DialogueTrigger
{
    [Header("Ariel Dialogue Informations")]
    [SerializeField]
    private float newNodeStep = 1;

    [SerializeField]
    private float newNodeMinimumDistance = 5;

    public override void OnTriggerStay2D(Collider2D other)
    {
        base.OnTriggerStay2D(other);

        if (other.CompareTag("Player"))
        {
            if (!dialogueManager.inDialogueMode && !dialogueManager.hasDialogueInStandBy)
            {
                dialogueManager.StartDialogueMode(dialogue);
                return;
            }

            if (player.inputHandler.pressedInteract)
            {
                dialogueManager.TryToStandByCurrentDialogue();
            }
        }
    }

    void Update()
    {
        if (dialogueManager.hasDialogueInStandBy)
        {
            Debug.Log(
                Mathf.Round(
                    Mathf.Abs(player.transform.position.x - gameObject.transform.position.x)
                )
            );
            if (
                Mathf.Round(
                    Mathf.Abs(player.transform.position.x - gameObject.transform.position.x)
                ) <= newNodeMinimumDistance
            )
            {
                dialogueManager.ResumeStandByDialogue();

                if (newNodeMinimumDistance == 2)
                    newNodeStep++;

                newNodeMinimumDistance -= newNodeStep;
            }
        }
    }
}
