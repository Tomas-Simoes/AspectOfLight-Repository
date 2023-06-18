using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    [Header("Stamina Bar")]
    [SerializeField]
    private Slider staminaSlider;

    [Header("Health Bar")]
    [SerializeField]
    private Slider healthSlider;

    [Header("Attack Bar")]
    [SerializeField]
    private Image attackMask;

    [Header("Others")]
    [SerializeField]
    private Player player;
    private GameObject inPlayerUIObject;

    void Start()
    {
        inPlayerUIObject = attackMask.transform.parent.gameObject;

        DontDestroyOnLoad(this);
    }

    void Update()
    {
        UpdateStaminaBar();
        UpdateHealthBar();

        if (player.stateMachine.currentState == player.attackState)
            UpdateAttackBar();
        else
            inPlayerUIObject.SetActive(false);
    }

    void UpdateStaminaBar()
    {
        staminaSlider.value = (float)player.currentStamina / (float)player.staminaData.MAX_STAMINA;
    }

    void UpdateHealthBar()
    {
        healthSlider.value = (float)player.currentHealth / (float)player.entityData.MAX_HEALTH;
    }

    void UpdateAttackBar()
    {
        inPlayerUIObject.SetActive(true);

        attackMask.fillAmount =
            (float)player.attackState.attackTime
            / (float)player.attackState.stateData.maxAttackTime;
    }
}
