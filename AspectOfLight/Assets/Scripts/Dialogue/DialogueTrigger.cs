using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Informations")]
    protected bool readyToInteract = true;

    [SerializeField]
    protected DialogueManager dialogueManager;

    [SerializeField]
    protected D_Dialogue dialogue;

    [SerializeField]
    protected Player player;

    protected virtual void StartDialogue() { }

    protected virtual void OnTriggerStay2D(Collider2D other) { }
}
