using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VideoOptionsMenu : MonoBehaviour
{
    [Header("Resolution Information")]
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions = new List<Resolution>();
    private int currentResolutionIndex;
    private float currentRefreshRate;

    [SerializeField]
    private TMP_Dropdown dropdown;

    private int currentSelectedResolution;

    private void Awake()
    {
        GetAvailableResolutions();
        InitDropdownResolutions();
    }

    private void GetAvailableResolutions()
    {
        resolutions = Screen.resolutions;
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == currentRefreshRate)
                filteredResolutions.Add(resolutions[i]);
        }
    }

    private void InitDropdownResolutions()
    {
        List<string> resolutionsToAdd = new List<string>();

        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string _option =
                filteredResolutions[i].width
                + "x"
                + filteredResolutions[i].height
                + " "
                + currentRefreshRate
                + " Hz";

            resolutionsToAdd.Add(_option);

            bool _isCurrentResolution =
                filteredResolutions[i].width == Screen.width
                && filteredResolutions[i].height == Screen.height;

            if (_isCurrentResolution)
            {
                currentResolutionIndex = i;
                GameManager.Instance.currentResolution = filteredResolutions[i];
            }
        }

        dropdown.options.Clear();

        dropdown.AddOptions(resolutionsToAdd);
        dropdown.value = currentResolutionIndex;

        dropdown.RefreshShownValue();
    }

    public void OnSaveChangedOptions()
    {
        if (
            filteredResolutions[currentSelectedResolution].width
                != GameManager.Instance.currentResolution.width
            && filteredResolutions[currentSelectedResolution].height
                != GameManager.Instance.currentResolution.height
        )
        {
            ChangeResolution(currentSelectedResolution);
        }
    }

    public void OnSelectedResolutionChanged(int value)
    {
        currentSelectedResolution = value;
        Debug.Log("Choise: " + currentSelectedResolution);
    }

    private void ChangeResolution(int newResolutionID)
    {
        int _newHorizontal = filteredResolutions[newResolutionID].width;
        int _newVertical = filteredResolutions[newResolutionID].height;

        Screen.SetResolution(_newHorizontal, _newVertical, true);
    }
}

[System.Serializable]
public class Resolutions
{
    public string label;
    public int horizontal;
    public int vertical;
}
