using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profiles")]
    [SerializeField]
    private string profileId = "";

    [Header("Content")]
    [SerializeField]
    private GameObject noDataContent;

    [SerializeField]
    private GameObject hasDataContent;

    [SerializeField]
    private TextMeshProUGUI deathCount;

    [SerializeField]
    private TextMeshProUGUI timePlayed;

    [SerializeField]
    private TextMeshProUGUI areaInMap;

    [Header("Clear Data Button")]
    [SerializeField]
    private Button clearButton;

    private Button saveSlotButton;

    private void Awake()
    {
        saveSlotButton = this.GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        if (data == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            clearButton.gameObject.SetActive(false);
        }
        else
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            clearButton.gameObject.SetActive(true);

            areaInMap.text = data.areaInMap;
            deathCount.text = data.deathCount + " DEATHS";
        }
    }

    public string GetProfileId()
    {
        return this.profileId;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
        clearButton.interactable = interactable;
    }
}
