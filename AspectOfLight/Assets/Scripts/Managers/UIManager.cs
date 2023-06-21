using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager>
{
    [Header("Death UI")]
    private GameObject deathScreen;
    private bool inDeathScreen = false;
    private TextMeshProUGUI deathText;
    private string[] deathMessages;

    [Header("MagistrateSigil UI")]
    private GameObject magistrateSigilMenu;
    private bool inMagistrateSigil;

    [Header("Pause UI")]
    private GameObject pauseMenu;
    private bool inPauseMenu = false;

    [Header("Dialogue UI")]
    private GameObject dialogueUI;
    private bool inDialogue = false;
    public bool loadedDialogueUIHasChoises;

    [Header("Ability Tree UI")]
    private GameObject abilityTree;
    public bool inAbilityTree = false;

    [Header("Progress Saved UI")]
    private GameObject progressSavedText;

    [Header("God Mode UI")]
    private GameObject godModeText;

    [Header("Player UI")]
    private GameObject playerUI;

    [Header("Prompt UI")]
    private GameObject promptMenu;

    private void OnEnable()
    {
        EventManager.StartListening("onPlayerDeath", OpenDeathScreen);
        EventManager.StartListening("onExit", OnExitUI);
        EventManager.StartListening("onMagistrateSigil", ChangeUIMagistrateSigilState);
        EventManager.StartListening("onDialogue", ChangeUIDialogueState);
        EventManager.StartListening("onAbilityTree", ChangeUIAbilityTreeState);
        EventManager.StartListening("onGameSaved", ShowProgressSavedText);
        EventManager.StartListening("onGodMode", ChangeUIGodModeState);
        EventManager.StartListening("onPlayerUI", OpenPlayerUI);
        EventManager.StartListening("onPrompt", onPrompt);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onPlayerDeath", OpenDeathScreen);
        EventManager.StopListening("onExit", OnExitUI);
        EventManager.StopListening("onMagistrateSigil", ChangeUIMagistrateSigilState);
        EventManager.StopListening("onDialogue", ChangeUIDialogueState);
        EventManager.StopListening("onAbilityTree", ChangeUIAbilityTreeState);
        EventManager.StopListening("onGameSaved", ShowProgressSavedText);
        EventManager.StopListening("onGodMode", ChangeUIGodModeState);
        EventManager.StopListening("onPlayerUI", OpenPlayerUI);
        EventManager.StopListening("onPrompt", onPrompt);
    }

    public void OnExitUI(Dictionary<string, object> message)
    {
        Debug.Log("asaa");

        if (inAbilityTree)
        {
            Debug.Log("aaa");
            EventManager.TriggerEvent("onExitInAbilityTree", true, null);
            return;
        }

        Debug.Log("a");
        ChangeUIPauseState(
            new Dictionary<string, object> { { "activeState", !GameManager.Instance.isPaused } }
        );
    }

    public void onPrompt(Dictionary<string, object> message)
    {
        promptMenu =
            (message["callbackMessage"] as Dictionary<string, object>)["prompt"] as GameObject;

        TextMeshProUGUI _textPrompt = promptMenu.transform
            .Find("TextPrompt")
            .GetComponent<TextMeshProUGUI>();

        Button _yesButton = promptMenu.transform.Find("YesButton").GetComponent<Button>();
        Button _noButton = promptMenu.transform.Find("NoButton").GetComponent<Button>();

        _textPrompt.text = (string)message["promptText"];

        _yesButton.onClick.AddListener(() =>
        {
            OnPromptYes callback = message["yesCallback"] as OnPromptYes;
            callback.Invoke(message["callbackMessage"] as Dictionary<string, object>);
        });

        _noButton.onClick.AddListener(() =>
        {
            OnPromptNo callback = message["noCallback"] as OnPromptNo;
            callback.Invoke(message["callbackMessage"] as Dictionary<string, object>);
        });
    }

    public delegate void OnPromptYes(Dictionary<string, object> message);

    public delegate void OnPromptNo(Dictionary<string, object> message);

    public void OpenDeathScreen(Dictionary<string, object> message)
    {
        inDeathScreen = (bool)message["activeState"];

        if (inDeathScreen && inMagistrateSigil)
        {
            ChangeUIMagistrateSigilState(
                new Dictionary<string, object> { { "activeState", false } }
            );
        }

        if (!inDeathScreen && inMagistrateSigil)
        {
            ChangeUIMagistrateSigilState(
                new Dictionary<string, object> { { "activeState", true } }
            );
        }

        if (deathScreen == null)
        {
            deathScreen = LoadUIObject("UIElements/DeathScreen/DeathScreen");
        }

        deathScreen.SetActive((bool)message["activeState"]);
    }

    public void OpenPlayerUI(Dictionary<string, object> message)
    {
        if (playerUI == null)
        {
            playerUI = LoadUIObject("UIElements/PlayerUI/PlayerUI");
        }

        playerUI.SetActive(true);
    }

    public void ChangeUIPauseState(Dictionary<string, object> message)
    {
        inPauseMenu = (bool)message["activeState"];
        GameManager.Instance.isPaused = inPauseMenu;

        if (inAbilityTree)
        {
            return;
        }

        if (inPauseMenu && inMagistrateSigil)
        {
            ChangeUIMagistrateSigilState(
                new Dictionary<string, object> { { "activeState", false } }
            );
        }

        if (!inPauseMenu && inMagistrateSigil)
        {
            ChangeUIMagistrateSigilState(
                new Dictionary<string, object> { { "activeState", true } }
            );
        }

        if (pauseMenu == null)
        {
            pauseMenu = LoadUIObject("UIElements/PauseMenu/PauseMenu");
        }

        pauseMenu.SetActive((bool)message["activeState"]);
    }

    public void ChangeUIMagistrateSigilState(Dictionary<string, object> message)
    {
        if (!inPauseMenu && !inAbilityTree && !inDeathScreen)
        {
            inMagistrateSigil = (bool)message["activeState"];
        }

        if (magistrateSigilMenu == null)
        {
            magistrateSigilMenu = LoadUIObject(
                "UIElements/MagistrateSigilMenu/MagistrateSigilMenu"
            );
        }

        magistrateSigilMenu.SetActive((bool)message["activeState"]);
    }

    public void ChangeUIAbilityTreeState(Dictionary<string, object> message)
    {
        inAbilityTree = (bool)message["activeState"];

        if (!inAbilityTree && inMagistrateSigil)
        {
            ChangeUIMagistrateSigilState(
                new Dictionary<string, object> { { "activeState", true } }
            );
        }

        if (inAbilityTree && inMagistrateSigil)
        {
            ChangeUIMagistrateSigilState(
                new Dictionary<string, object> { { "activeState", false } }
            );
        }

        if (abilityTree == null)
        {
            abilityTree = LoadUIObject("UIElements/AbilitiesMenu/AbilitiesMenu");
        }

        abilityTree.SetActive((bool)message["activeState"]);
    }

    public void ChangeUIDialogueState(Dictionary<string, object> message)
    {
        inDialogue = (bool)message["activeState"];

        if ((bool)message["activeState"])
        {
            D_Dialogue _thisDialogue = (D_Dialogue)message["dialogue"];

            LoadDialogueUIBasedOnChoises(
                _thisDialogue.dialogueNodes[(int)message["startNode"]].dialogueChoises.Count != 0
            );

            DialogueManager.Instance.StartDialogueMode(_thisDialogue);
        }
        else
        {
            Destroy(dialogueUI.gameObject);
        }
    }

    public void ShowProgressSavedText(Dictionary<string, object> message)
    {
        if (!GameManager.Instance.gameStarted)
            return;

        if (progressSavedText == null)
        {
            progressSavedText = LoadUIObject("UIElements/Informations/OnSaveText");
        }

        progressSavedText.SetActive(true);
        StartCoroutine(HideProgressSavedText());
    }

    public void ChangeUIGodModeState(Dictionary<string, object> message)
    {
        if (godModeText == null)
        {
            godModeText = LoadUIObject("UIElements/Informations/OnGodModeText");
        }

        godModeText.SetActive((bool)message["activeState"]);
    }

    //TODO Change this to work without having to load the UI
    //? (it's needed to separate DialogueManager to DialogueMenu, so good luck future Tomás)
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

    private IEnumerator HideProgressSavedText()
    {
        yield return new WaitForSeconds(3f);

        if (GameManager.Instance.gameStarted)
            progressSavedText.SetActive(false);
    }

    public GameObject LoadUIObject(string path)
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
