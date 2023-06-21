using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
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

    [Header("Stack Bar")]
    [SerializeField]
    private GameObject stackBar;

    private GameObject poisonStack;

    [Header("Stack Prefabs")]
    [SerializeField]
    private GameObject stackPrefabTypePoison;

    [Header("Others")]
    [SerializeField]
    private Player player;
    private GameObject inPlayerUIObject;

    void Start()
    {
        //inPlayerUIObject = attackMask.transform.parent.gameObject;
    }

    private void OnEnable()
    {
        player = GameManager.Instance.playerReference;

        EventManager.StartListening("onUpdateStackBar", UpdateStackBar);
    }

    private void OnDisable()
    {
        EventManager.StopListening("onUpdateStackBar", UpdateStackBar);
    }

    void Update()
    {
        UpdateStaminaBar();
        UpdateHealthBar();

        //if (player.stateMachine.currentState == player.attackState)
        //    UpdateAttackBar();
        //else
        //   inPlayerUIObject.SetActive(false);
    }

    void UpdateStaminaBar()
    {
        staminaSlider.value = (float)player.currentStamina / (float)player.staminaData.MAX_STAMINA;
    }

    void UpdateHealthBar()
    {
        healthSlider.value = (float)player.currentHealth / (float)player.entityData.MAX_HEALTH;
    }

    void UpdateStackBar(Dictionary<string, object> message)
    {
        GameObject _currentStack = null;

        switch ((string)message["type"])
        {
            case "poison":
                if (poisonStack == null)
                {
                    poisonStack = Instantiate(stackPrefabTypePoison);
                    poisonStack.transform.SetParent(stackBar.transform, false);
                }

                _currentStack = poisonStack;
                break;
            default:
                return;
        }

        _currentStack.GetComponentInChildren<TextMeshProUGUI>().text = message["stackNumber"] + "x";

        if ((int)message["stackNumber"] <= 0)
            GameObject.Destroy(_currentStack);
    }

    void UpdateAttackBar()
    {
        inPlayerUIObject.SetActive(true);

        attackMask.fillAmount =
            (float)player.attackState.attackTime
            / (float)player.attackState.stateData.maxAttackTime;
    }
}
