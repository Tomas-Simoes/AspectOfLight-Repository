using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    public Entity bulletParent;

    [Header("Bullet Stats")]
    [SerializeField]
    float speed = 3f;

    [SerializeField]
    int damage = 2;

    [SerializeField]
    float lifeTime = 4f;

    [SerializeField]
    string targatableTag;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity =
            (bulletParent.target.transform.position - transform.position).normalized * speed;

        Destroy(this.gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targatableTag))
        {
            other.GetComponent<Entity>().TakeDamage(damage);
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}
