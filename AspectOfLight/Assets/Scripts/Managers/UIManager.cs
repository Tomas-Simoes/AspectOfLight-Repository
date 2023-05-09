using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager>
{
    [Header("UI information")]
    private string currentUI;

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

    [Header("Ability Tree UI")]
    private GameObject abilityTree;

    private void OnEnable()
    {
        EventManager.StartListening("onPlayerDeath", Instance.OpenDeathScreen);
        EventManager.StartListening("onPause", Instance.ChangeUIPauseState);
        EventManager.StartListening("onDialogue", Instance.ChangeUIDialogueState);
        EventManager.StartListening("onAbilityTree", Instance.ChangeUIAbilityTreeState);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onPlayerDeath", Instance.OpenDeathScreen);
        EventManager.StopListening("onPause", Instance.ChangeUIPauseState);
        EventManager.StopListening("onDialogue", Instance.ChangeUIDialogueState);
        EventManager.StopListening("onAbilityTree", Instance.ChangeUIAbilityTreeState);
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

    //TODO Change this to the event system
    public void ChangeUIMagistrateSigilState(bool activeState)
    {
        if (magistrateSigilMenu == null)
        {
            magistrateSigilMenu = LoadUIObject(
                "UIElements/MagistrateSigilMenu/MagistrateSigilMenu"
            );

            magistrateSigilMenu.transform
                .Find("Buttons/RestButton")
                .GetComponent<Button>()
                .onClick.AddListener(MagistrateSigilMenu.Instance.OnRestPressed);

            magistrateSigilMenu.transform
                .Find("Buttons/SacredPathsButton")
                .GetComponent<Button>()
                .onClick.AddListener(MagistrateSigilMenu.Instance.OnSkillTreePressed);

            magistrateSigilMenu.transform
                .Find("Buttons/SaveButton")
                .GetComponent<Button>()
                .onClick.AddListener(MagistrateSigilMenu.Instance.OnSaveGamePressed);

            magistrateSigilMenu.transform
                .Find("Buttons/LoadButton")
                .GetComponent<Button>()
                .onClick.AddListener(MagistrateSigilMenu.Instance.OnLoadGamePressed);
        }

        magistrateSigilMenu.SetActive(activeState);
    }

    public void ChangeUIAbilityTreeState(Dictionary<string, object> message)
    {
        bool _activeState = (bool)message["activeState"];

        ChangeUIMagistrateSigilState(!_activeState);

        if (abilityTree == null)
        {
            abilityTree = LoadUIObject("UIElements/AbilitiesMenu/AbilitiesMenu");
            AbilityManager.Instance.abilitiesData = Resources.Load<D_Abilities>(
                "UIElements/AbilitiesMenu/AbilitiesData"
            );
        }

        abilityTree.SetActive(_activeState);
    }

    public void ChangeUIDialogueState(Dictionary<string, object> message)
    {
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
