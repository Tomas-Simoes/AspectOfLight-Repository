using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagistrateSigilUITrigger : MonoBehaviour
{
    [Header("Magistrate Sigil Information")]
    [SerializeField]
    private GameObject magistrateSigilMenu;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            magistrateSigilMenu.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            magistrateSigilMenu.SetActive(false);
        }
    }
}
