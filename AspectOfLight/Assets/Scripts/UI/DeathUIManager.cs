using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathUIManager : MonoBehaviour
{
    [Header("Death Information")]
    [SerializeField]
    private GameObject deathScreen;

    [SerializeField]
    private TextMeshProUGUI deathText;

    [SerializeField]
    private string[] deathMessages;

    public void UpdateDeathUI()
    {
        deathScreen.SetActive(true);
        deathText.text = deathMessages[Random.Range(0, deathMessages.Length)];
    }
}
