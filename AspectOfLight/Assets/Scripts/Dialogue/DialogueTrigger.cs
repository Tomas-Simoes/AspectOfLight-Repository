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
        EventManager.StartListening("onExitDialogue", FinishInteraction);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onExitDialogue", FinishInteraction);
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
        timeSinceInteract = 0;
        hasFinishedInteraction = true;
    }

    public virtual void StartDialogue(int startNode)
    {
        Debug.Log("bb");
        Debug.Log(timeSinceInteract);
        Debug.Log(minimumTimeToInteractAgain);
        if (timeSinceInteract < minimumTimeToInteractAgain)
            return;

        hasFinishedInteraction = false;

        Debug.Log("triggered");
        EventManager.TriggerEvent(
            "onDialogue",
            true,
            new Dictionary<string, object>
            {
                { "dialogue", dialogue },
                { "inputMap", "Player" },
                { "startNode", startNode },
                { "inputMapState", false }
            }
        );
    }

    public virtual void OnTriggerStay2D(Collider2D other) { }
}
