using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiantCrystal : MonoBehaviour
{
    [Header("Objects")]
    public Player player;

    [Header("Laser Information")]
    [SerializeField]
    private float tickRate;

    [SerializeField]
    private float offset;

    [SerializeField]
    private float damage;

    [SerializeField]
    private float lifetime;

    [Header("Enemies Information")]
    public List<Collider2D> enemiesInRange = new List<Collider2D>();

    void Start()
    {
        StartCoroutine(Damage());

        Destroy(this.gameObject, lifetime);

        player.rb.velocity = Vector2.zero;
    }

    void Update()
    {
        LaserDirection();
    }

    void LaserDirection()
    {
        var direction =
            Input.mousePosition - Camera.main.WorldToScreenPoint(player.transform.position);
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 playerToMouseDirection =
            Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;

        playerToMouseDirection.z = 0;

        transform.position =
            player.transform.position + (offset * playerToMouseDirection.normalized);

        Vector3 AimLocalScale = Vector3.one;

        if (angle > 90 || angle < -90)
        {
            AimLocalScale.y = -1f;
        }
        else
        {
            AimLocalScale.y = 1f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !enemiesInRange.Contains(other))
            enemiesInRange.Add(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && enemiesInRange.Contains(other))
            enemiesInRange.Remove(other);
    }

    private IEnumerator Damage()
    {
        while (true)
        {
            foreach (Collider2D enemy in enemiesInRange)
            {
                Debug.Log("enemy");
                enemy.GetComponent<Entity>().TakeDamage(damage);
            }

            yield return new WaitForSeconds(tickRate);
        }
    }

    private void OnDestroy()
    {
        EventManager.TriggerEvent(
            "onPlayerCastingState",
            false,
            new Dictionary<string, object> { { "activeState", false } }
        );
    }
}
