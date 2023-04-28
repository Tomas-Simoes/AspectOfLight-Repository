using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagistrateSigilUITrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.ChangeUIMagistrateSigilState(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.ChangeUIMagistrateSigilState(false);
        }
    }
}
