using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Informations")]
    [SerializeField]
    protected float minimumTimeToInteractAgain = 0.5f;
    protected float timeSinceInteract;
    protected bool hasFinishedInteraction;

    [SerializeField]
    protected D_Dialogue dialogue;

    [SerializeField]
    protected Player player;

    private void OnEnable()
    {
        EventManager.StartListening("onExitDialogue", FinishInteraction);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onExitDialogue", FinishInteraction);
    }

    private void Start()
    {
        timeSinceInteract = minimumTimeToInteractAgain;
    }

    private void Update()
    {
        if (hasFinishedInteraction)
        {
            timeSinceInteract += Time.deltaTime;
        }
    }

    private void FinishInteraction(Dictionary<string, object> message)
    {
        timeSinceInteract = 0;
        hasFinishedInteraction = true;
    }

    public virtual void StartDialogue()
    {
        if (timeSinceInteract < minimumTimeToInteractAgain)
            return;

        hasFinishedInteraction = false;

        EventManager.TriggerEvent(
            "onDialogue",
            true,
            new Dictionary<string, object> { { "dialogue", dialogue } }
        );
    }

    public virtual void OnTriggerStay2D(Collider2D other) { }
}
