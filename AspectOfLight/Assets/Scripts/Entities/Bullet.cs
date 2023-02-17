using UnityEngine;

public class Bullet : MonoBehaviour
{
  public Vector3 targetPosition;

  [Header("Bullet Stats")]
  [SerializeField]
  float speed = 3f;

  [SerializeField]
  public float damage = 2;

  [SerializeField]
  float lifeTime = 4f;

  [SerializeField]
  string targatableTag;

  private Rigidbody2D rb;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();


    //Debug.Log("Target position: " + targetPosition);
    //Debug.Log("This position: " + transform.position);
    //Debug.Log("Vector normalized: " + (targetPosition - transform.position).normalized);

    rb.velocity =
        (targetPosition - transform.position).normalized * speed;

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
