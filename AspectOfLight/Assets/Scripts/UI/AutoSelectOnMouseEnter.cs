using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AutoSelectOnMouseEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button thisButton;

    private void Awake()
    {
        thisButton = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        thisButton.Select();
    }

    public void OnPointerExit(PointerEventData eventData) { }
}
