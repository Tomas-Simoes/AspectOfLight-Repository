using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagistrateExiledFlorestDialogueTrigger : DialogueTrigger, IDataPersistence
{
    [Header("Tutorial Information")]
    [SerializeField]
    private bool onEnterArea;

    [SerializeField]
    private bool onExitArea;

    [SerializeField]
    private string tutorialName;

    [SerializeField]
    private float dialogueDelay;

    protected bool alreadyReadTutorial = false;
    private bool inDialogue = false;

    public override void Update()
    {
        base.Update();

        if (
            inDialogue
            && player.inputHandler.pressedInteract
            && DialogueManager.Instance.inDialogueMode
        )
        {
            EventManager.TriggerEvent("onGoToNextNode", false, null);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !alreadyReadTutorial && onEnterArea)
        {
            alreadyReadTutorial = true;

            StartCoroutine(StartDialogueAfterSeconds(dialogueDelay));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !alreadyReadTutorial && onExitArea)
        {
            alreadyReadTutorial = true;

            StartCoroutine(StartDialogueAfterSeconds(dialogueDelay));
        }
    }

    public IEnumerator StartDialogueAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        inDialogue = true;
        StartDialogue(0);
        this.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void LoadData(GameData data,  bool loadSensitiveData)
    {
        alreadyReadTutorial = data.tutorials[tutorialName];
    }

    public void SaveData(GameData data)
    {
        data.tutorials[tutorialName] = alreadyReadTutorial;
    }
}
