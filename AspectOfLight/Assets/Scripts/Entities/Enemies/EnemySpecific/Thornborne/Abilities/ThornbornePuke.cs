using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornbornePuke : MonoBehaviour
{
    [Header("Puke Information")]
    [SerializeField]
    private float tickRate;

    [SerializeField]
    private float damage;

    [SerializeField]
    private float lifeTime;

    private float timeSinceDamage;
    private Player player;
    private bool playerOnPuke;

    private void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }

    void Update()
    {
        timeSinceDamage += Time.deltaTime;

        if (playerOnPuke && timeSinceDamage >= tickRate)
            DoDamage(player);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (player == null)
                player = other.gameObject.GetComponent<Player>();

            playerOnPuke = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        playerOnPuke = false;
    }

    private void DoDamage(Entity entity)
    {
        timeSinceDamage = 0;
        entity.TakeDamage(damage);
    }
}
