using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    [Header("Stamina Bar")]
    [SerializeField]
    private Image staminaMask;

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
    }

    void Update()
    {
        UpdateStaminaBar();

        if (player.stateMachine.currentState == player.attackState)
            UpdateAttackBar();
        else
            inPlayerUIObject.SetActive(false);
    }

    void UpdateStaminaBar()
    {
        staminaMask.fillAmount =
            (float)player.currentStamina / (float)player.entityData.MAX_STAMINA;
    }

    void UpdateAttackBar()
    {
        inPlayerUIObject.SetActive(true);

        attackMask.fillAmount =
            (float)player.attackState.attackTime
            / (float)player.attackState.stateData.maxAttackTime;
    }
}
