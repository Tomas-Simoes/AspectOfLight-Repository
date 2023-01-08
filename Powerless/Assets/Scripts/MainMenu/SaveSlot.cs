using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public void SetData(GameData data)
    {
        if (data == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        else
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            areaInMap.text = data.areaInMap;
            deathCount.text = data.deathCount + " DEATHS";
        }
    }

    public string GetProfileId()
    {
        return this.profileId;
    }
}
