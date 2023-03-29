using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("First Selected Button")]
    [SerializeField]
    private Button firstSelected;

    protected virtual void OnEnable()
    {
        SetFirstSelected(firstSelected);
        Debug.Log("Set first selected: " + firstSelected.gameObject.name);
    }

    public void SetFirstSelected(Button firstSelectedButton)
    {
        firstSelectedButton.Select();
    }
}
