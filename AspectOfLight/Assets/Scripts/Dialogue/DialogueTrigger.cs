using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
  [Header("Informations")]
  [SerializeField]
  protected bool readyToInteract = true;
  [SerializeField]
  protected bool onlyInteractOnce;

  [SerializeField]
  protected float minimumTimeAfterDialogue;
  [SerializeField]

  protected float timeAfterDialogue;

  [SerializeField]
  protected DialogueManager dialogueManager;

  [SerializeField]
  protected D_Dialogue dialogue;

  [SerializeField]
  protected Player player;

  public virtual void OnTriggerStay2D(Collider2D other) { }
}
