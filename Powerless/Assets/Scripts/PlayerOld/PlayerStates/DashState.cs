using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Dash")]
public class DashState : State<PlayerController>
{
    public Rigidbody2D _rigidbody;

    [Header("Dash Settings")]
    [SerializeField]
    public float dashSpeed;

    [SerializeField]
    public float dashTime;

    public bool dashing;
    public float timeDashing;
    public float timeSinceDash;

    public override void Init(PlayerController parent)
    {
        if (_rigidbody == null)
            _rigidbody = parent.GetComponent<Rigidbody2D>();
        base.Init(parent);

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y) * dashSpeed;

        timeDashing = 0;
        dashing = true;
    }

    public override void GetInput() { }

    public override void Update()
    {
        timeDashing += Time.deltaTime;
        if (timeDashing >= dashTime)
        {
            timeDashing = 0;
            dashing = false;
        }
    }

    public override void FixedUpdate() { }

    public override void ChangeState()
    {
        if (!dashing)
        {
            _runner.SetState(typeof(WalkState));
        }
    }

    public override void Exit() { }
}
