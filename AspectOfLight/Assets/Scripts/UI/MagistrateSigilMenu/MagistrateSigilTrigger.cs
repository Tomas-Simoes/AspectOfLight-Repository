using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagistrateSigilTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.TriggerEvent(
                "onMagistrateSigil",
                true,
                new Dictionary<string, object> { { "activeState", true } }
            );
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.TriggerEvent(
                "onMagistrateSigil",
                true,
                new Dictionary<string, object> { { "activeState", false } }
            );
        }
    }
}
