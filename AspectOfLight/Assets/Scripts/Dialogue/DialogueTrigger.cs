using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Informations")]
    [SerializeField]
    protected float minimumTimeToInteractAgain = 0.5f;

    [SerializeField]
    protected float timeSinceInteract;
    protected bool hasFinishedInteraction;

    [SerializeField]
    protected D_Dialogue dialogue;

    [SerializeField]
    protected Player player;

    private void OnEnable()
    {
        EventManager.StartListening("onDialogue", FinishInteraction);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onDialogue", FinishInteraction);
    }

    public virtual void Start()
    {
        timeSinceInteract = minimumTimeToInteractAgain;
    }

    public virtual void Update()
    {
        if (hasFinishedInteraction)
        {
            timeSinceInteract += Time.deltaTime;
        }
    }

    private void FinishInteraction(Dictionary<string, object> message)
    {
        if (!(bool)message["activeState"])
        {
            timeSinceInteract = 0;
            hasFinishedInteraction = true;
        }
    }

    public virtual void StartDialogue(int startNode)
    {
        if (timeSinceInteract < minimumTimeToInteractAgain)
            return;

        hasFinishedInteraction = false;

        EventManager.TriggerEvent(
            "onDialogue",
            true,
            new Dictionary<string, object> { { "dialogue", dialogue }, { "startNode", startNode }, {"activeState", true}}
        );
    }

    public virtual void OnTriggerStay2D(Collider2D other) { }
}
