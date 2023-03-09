using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeathRandomMessage : MonoBehaviour
{
  [SerializeField]
  private string[] deathTexts;
  public TextMeshProUGUI mytext;
  void Start()
  {
    mytext.text = deathTexts[Random.Range(0, deathTexts.Length)];
  }
}
