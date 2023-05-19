using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserComponent : MonoBehaviour
{
    [Header("Objects")]
    public Player player;
    public Entity entity;
    public GameObject laser;

    [Header("Laser Information")]
    [SerializeField] protected string targatableTag; //Why? I don't remember why i did this.
    [SerializeField] private float tickRate;
    [SerializeField] private float laserTimer;
    
    [Header("Laser Values")]
    [SerializeField] private float offset;
    public float damage;
    public float lifetime;

    void Start()
    {
        laser.SetActive(true);
        Destroy(this.gameObject, lifetime);
        entity.rb.velocity = Vector2.zero;
    }
    
    void Update()
    {
        aiming();
    }

    void aiming()
    {
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(player.transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0,0, angle);

        Vector3 playerToMouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
        playerToMouseDir.z = 0;

        transform.position = player.transform.position + (offset * playerToMouseDir.normalized);

        Vector3 AimLocalScale = Vector3.one;

        if(angle > 90 || angle < -90)
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
            if(other.CompareTag(targatableTag))
            {
                other.gameObject.GetComponent<Entity>()
                .TakeLaserDamage(damage, laserTimer, tickRate);
                Destroy(this.gameObject, lifetime);
                Debug.Log("hit");
            }
        }
}
