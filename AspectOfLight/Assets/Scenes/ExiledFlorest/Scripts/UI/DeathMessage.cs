using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DeathMessage : MonoBehaviour
{
  private TextMeshProUGUI mytext;
  [SerializeField]
  protected string[] deathMessages;

  void Start()
  {
    mytext.text = deathMessages[Random.Range(0, deathMessages.Length)];
  }

}
