using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VideoOptionsMenu : MonoBehaviour
{
    [Header("Resolution Information")]
    [SerializeField]
    private List<Resolution> resolutions;

    [SerializeField]
    private TMP_Dropdown dropdown;

    private int currentSelectedResolution;

    private void Awake()
    {
        InitDropdownResolutions();
    }

    private void InitDropdownResolutions()
    {
        List<string> resolutionsToAdd = new List<string>();

        foreach (Resolution resolution in resolutions)
            resolutionsToAdd.Add(resolution.label);

        dropdown.options.Clear();

        dropdown.AddOptions(resolutionsToAdd);
        dropdown.value = 0;

        dropdown.RefreshShownValue();
    }

    public void OnSaveChangedOptions()
    {
        if (resolutions[currentSelectedResolution].label != GameManager.Instance.currentResolution)
        {
            ChangeResolution(currentSelectedResolution);
        }
    }

    public void OnSelectedResolutionChanged(int value)
    {
        currentSelectedResolution = value;
    }

    private void ChangeResolution(int newResolutionID)
    {
        int _newHorizontal = resolutions[currentSelectedResolution].horizontal;
        int _newVertical = resolutions[currentSelectedResolution].vertical;

        Debug.Log(_newHorizontal + " " + _newVertical);
        Screen.SetResolution(_newHorizontal, _newVertical, true);
    }
}

[System.Serializable]
public class Resolution
{
    public string label;
    public int horizontal;
    public int vertical;
}
