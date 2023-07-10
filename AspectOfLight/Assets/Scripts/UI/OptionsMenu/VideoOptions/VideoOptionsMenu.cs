using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class VideoOptionsMenu : MonoBehaviour
{
    [Header("Resolution Information")]
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions = new List<Resolution>();
    private int currentResolutionIndex;
    private float currentRefreshRate;

    [Header("Current Selected Options")]
    private int currentSelectedResolution;
    private int currentSelectedTargetFrameRate;

    [Header("UI Elements")]
    [SerializeField]
    private TMP_Dropdown resolutionsDropdown;

    [SerializeField]
    private TMP_InputField targetFrameRateInputField;

    private void Awake()
    {
        GetAvailableResolutions();
        InitDropdownResolutions();

        targetFrameRateInputField.text = GameManager.Instance.currentTargetFrameRate.ToString();
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

        resolutionsDropdown.options.Clear();

        resolutionsDropdown.AddOptions(resolutionsToAdd);
        resolutionsDropdown.value = currentResolutionIndex;

        resolutionsDropdown.RefreshShownValue();
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

        if (currentSelectedTargetFrameRate != GameManager.Instance.currentTargetFrameRate)
        {
            ChangeTargetFrameRate(currentSelectedTargetFrameRate);
        }
    }

    public void OnSelectedResolutionChanged(int value)
    {
        currentSelectedResolution = value;
    }

    public void OnEndEditFrameRateChanged(string value)
    {
        currentSelectedTargetFrameRate =
            value != "" || value == "0"
                ? Int16.Parse(value)
                : GameManager.Instance.currentTargetFrameRate;

        if (value == "" || value == "0")
            targetFrameRateInputField.text = currentSelectedTargetFrameRate.ToString();
    }

    private void ChangeResolution(int newResolutionID)
    {
        int _newHorizontal = filteredResolutions[newResolutionID].width;
        int _newVertical = filteredResolutions[newResolutionID].height;

        Screen.SetResolution(_newHorizontal, _newVertical, true);
        GameManager.Instance.currentResolution = filteredResolutions[newResolutionID];
    }

    private void ChangeTargetFrameRate(int newTargetFrameRate)
    {
        Application.targetFrameRate = newTargetFrameRate;
        GameManager.Instance.currentTargetFrameRate = newTargetFrameRate;
    }
}
