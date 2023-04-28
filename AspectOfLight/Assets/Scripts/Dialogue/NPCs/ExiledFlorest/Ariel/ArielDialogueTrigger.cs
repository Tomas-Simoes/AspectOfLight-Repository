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
            if (
                !DialogueManager.Instance.inDialogueMode
                && !DialogueManager.Instance.hasDialogueInStandBy
            )
            {
                StartDialogue();
                return;
            }

            if (player.inputHandler.pressedInteract)
            {
                DialogueManager.Instance.TryToStandByCurrentDialogue();
            }
        }
    }

    void Update()
    {
        if (DialogueManager.Instance.hasDialogueInStandBy)
        {
            if (
                Mathf.Round(
                    Mathf.Abs(player.transform.position.x - gameObject.transform.position.x)
                ) <= newNodeMinimumDistance
            )
            {
                DialogueManager.Instance.ResumeStandByDialogue();

                if (newNodeMinimumDistance == 2)
                    newNodeStep++;

                newNodeMinimumDistance -= newNodeStep;
            }
        }
    }
}
