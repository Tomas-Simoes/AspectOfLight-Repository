using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager>
{
    [Header("Death UI")]
    private GameObject deathScreen;
    private TextMeshProUGUI deathText;
    private string[] deathMessages;

    [Header("MagistrateSigil UI")]
    private GameObject magistrateSigilMenu;

    [Header("Pause UI")]
    private GameObject pauseMenu;

    [Header("Dialogue UI")]
    private GameObject dialogueUI;
    public bool loadedDialogueUIHasChoises;

    private void OnEnable()
    {
        EventManager.StartListening("onPlayerDeath", Instance.OpenDeathScreen);
        EventManager.StartListening("onPause", Instance.ChangeUIPauseState);

        EventManager.StartListening("onDialogue", Instance.OpenUIDialogue);
        EventManager.StartListening("onExitDialogue", Instance.CloseUIDialogue);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onPlayerDeath", Instance.OpenDeathScreen);
        EventManager.StopListening("onPause", Instance.ChangeUIPauseState);

        EventManager.StopListening("onDialogue", Instance.OpenUIDialogue);
        EventManager.StopListening("onExitDialogue", Instance.CloseUIDialogue);
    }

    public void OpenDeathScreen(Dictionary<string, object> message)
    {
        if (deathScreen == null)
        {
            D_DeathScreen deathScreenData = Resources.Load<D_DeathScreen>(
                "UIElements/DeathScreen/DeathScreenData"
            );

            deathScreen = Instantiate(deathScreenData.deathScreen);
            deathMessages = deathScreenData.deathMessages;

            deathScreen.transform.SetParent(
                GameObject.FindGameObjectWithTag("Canvas").transform,
                false
            );

            deathScreen.transform
                .Find("Buttons/ContinueButton")
                .GetComponent<Button>()
                .onClick.AddListener(DeathMenu.Instance.OnContinueClicked);

            deathText = deathScreen.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }

        Time.timeScale = 0f;

        deathScreen.SetActive(true);
        deathText.text = deathMessages[UnityEngine.Random.Range(0, deathMessages.Length)];
    }

    public void ChangeUIPauseState(Dictionary<string, object> message)
    {
        if (pauseMenu == null)
        {
            pauseMenu = LoadUIObject("UIElements/PauseMenu/PauseMenu");

            GetChildObject(pauseMenu, "Buttons/ContinueButton")
                .GetComponent<Button>()
                .onClick.AddListener(PauseMenu.Instance.OnContinueClicked);
        }

        pauseMenu.SetActive((bool)message["pauseGame"]);
    }

    public void ChangeUIMagistrateSigilState(bool activeState)
    {
        if (magistrateSigilMenu == null)
        {
            magistrateSigilMenu = LoadUIObject(
                "UIElements/MagistrateSigilMenu/MagistrateSigilMenu"
            );

            magistrateSigilMenu.transform
                .Find("Buttons/SaveButton")
                .GetComponent<Button>()
                .onClick.AddListener(MagistrateSigilMenu.Instance.OnSaveGamePressed);
        }

        magistrateSigilMenu.SetActive(activeState);
    }

    public void OpenUIDialogue(Dictionary<string, object> message)
    {
        D_Dialogue _thisDialogue = (D_Dialogue)message["dialogue"];

        LoadDialogueUIBasedOnChoises(_thisDialogue.dialogueNodes[0].dialogueChoises.Count != 0);

        DialogueManager.Instance.StartDialogueMode((D_Dialogue)message["dialogue"]);
    }

    public void LoadDialogueUIBasedOnChoises(bool hasChoises)
    {
        GameObject newDialogueUI = dialogueUI;

        if (hasChoises && (dialogueUI == null || !loadedDialogueUIHasChoises))
        {
            newDialogueUI = LoadUIObject("UIElements/DialogueMenu/DialogueWithChoises");
            LoadChildDialogueUI(newDialogueUI);

            DialogueManager.Instance.choisesContainer = GetChildObject(
                newDialogueUI,
                "DialogueChoises/ScrollChoises/Content"
            );

            DialogueManager.Instance.choisePrefab = Resources.Load<GameObject>(
                "UIElements/DialogueMenu/Choise"
            );

            loadedDialogueUIHasChoises = true;
        }
        else if (!hasChoises && (dialogueUI == null || loadedDialogueUIHasChoises))
        {
            newDialogueUI = LoadUIObject("UIElements/DialogueMenu/DialogueWithoutChoises");
            LoadChildDialogueUI(newDialogueUI);

            loadedDialogueUIHasChoises = false;
        }

        if (newDialogueUI != dialogueUI)
        {
            Debug.Log("Destroy dialogue UI");
            Destroy(dialogueUI);

            newDialogueUI.SetActive(true);
        }

        dialogueUI = newDialogueUI;
    }

    public void LoadChildDialogueUI(GameObject newDialogueUI)
    {
        DialogueManager.Instance.dialogue = newDialogueUI.GetComponent<Image>();

        DialogueManager.Instance.dialogueText = GetChildObject(
                newDialogueUI,
                "DialogueText/Scroll View/Viewport/Content/DialogueText"
            )
            .GetComponent<TextMeshProUGUI>();

        DialogueManager.Instance.speakerText = GetChildObject(newDialogueUI, "SpeakerName")
            .GetComponent<TextMeshProUGUI>();
    }

    public void CloseUIDialogue(Dictionary<string, object> message)
    {
        Destroy(dialogueUI.gameObject);
    }

    private GameObject LoadUIObject(string path)
    {
        GameObject _newObject = Instantiate(Resources.Load<GameObject>(path));

        _newObject.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);

        return _newObject;
    }

    private GameObject GetChildObject(GameObject parent, string name)
    {
        return parent.transform.Find(name)?.gameObject;
    }
}
