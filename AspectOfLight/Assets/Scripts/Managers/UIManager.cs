using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
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

    [Header("Ability Tree UI")]
    private GameObject abilityTree;

    [Header("Progress Saved UI")]
    private GameObject progressSavedText;

    private void OnEnable()
    {
        EventManager.StartListening("onPlayerDeath", OpenDeathScreen);
        EventManager.StartListening("onPause", ChangeUIPauseState);
        EventManager.StartListening("onMagistrateSigil", ChangeUIMagistrateSigilState);
        EventManager.StartListening("onDialogue", ChangeUIDialogueState);
        EventManager.StartListening("onAbilityTree", ChangeUIAbilityTreeState);
        EventManager.StartListening("onGameSaved", ShowProgressSavedText);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onPlayerDeath", OpenDeathScreen);
        EventManager.StopListening("onPause", ChangeUIPauseState);
        EventManager.StopListening("onMagistrateSigil", ChangeUIMagistrateSigilState);
        EventManager.StopListening("onDialogue", ChangeUIDialogueState);
        EventManager.StopListening("onAbilityTree", ChangeUIAbilityTreeState);
        EventManager.StopListening("onGameSaved", ShowProgressSavedText);
    }

    public void OpenDeathScreen(Dictionary<string, object> message)
    {
        if (deathScreen == null)
        {
            deathScreen = LoadUIObject("UIElements/DeathScreen/DeathScreen");
        }

        deathScreen.SetActive((bool)message["activeState"]);
    }

    public void ChangeUIPauseState(Dictionary<string, object> message)
    {
        if (pauseMenu == null)
        {
            pauseMenu = LoadUIObject("UIElements/PauseMenu/PauseMenu");
        }

        pauseMenu.SetActive((bool)message["activeState"]);
    }

    public void ChangeUIMagistrateSigilState(Dictionary<string, object> message)
    {
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
        ChangeUIMagistrateSigilState(new Dictionary<string, object> { { "activeState", false } });

        if (abilityTree == null)
        {
            abilityTree = LoadUIObject("UIElements/AbilitiesMenu/AbilitiesMenu");
            AbilityManager.Instance.abilitiesData = Resources.Load<D_Abilities>(
                "UIElements/AbilitiesMenu/AbilitiesData"
            );
        }

        abilityTree.SetActive((bool)message["activeState"]);
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

    private IEnumerator HideProgressSavedText()
    {
        yield return new WaitForSeconds(3f);

        if (GameManager.Instance.gameStarted)
            progressSavedText.SetActive(false);
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
