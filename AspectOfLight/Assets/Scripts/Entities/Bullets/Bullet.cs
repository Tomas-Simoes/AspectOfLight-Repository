using UnityEngine;

public class Bullet : MonoBehaviour
{
  public Vector3 targetPosition;

  [Header("Bullet Stats")]
  [SerializeField]
  protected float speed = 3f;

  [SerializeField]
  public float damage = 2;

  [SerializeField]
  protected float lifeTime = 4f;

  [SerializeField]
  protected string targatableTag;

  protected Rigidbody2D rb;



  public InputHandler inputHandler { get; private set; }

  public DefaultBullet defaultBullet { get; private set; }

  public virtual void Start()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  public virtual void Update()
  { }
}
