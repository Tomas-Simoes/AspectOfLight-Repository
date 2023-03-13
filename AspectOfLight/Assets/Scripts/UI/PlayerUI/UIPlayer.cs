using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    [Header("Progress Bar")]
    [SerializeField]
    private Image progressMask;

    [Header("Others")]
    [SerializeField]
    private Player player;

    void Update()
    {
        UpdateStaminaBar();
    }

    void UpdateStaminaBar()
    {
        progressMask.fillAmount =
            (float)player.currentStamina / (float)player.entityData.MAX_STAMINA;
    }
}
