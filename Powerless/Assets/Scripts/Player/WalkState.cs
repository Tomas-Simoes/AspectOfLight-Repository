using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Walk")]
public class WalkState : State<PlayerController>
{

    private Rigidbody2D Rb;

    public Vector2 AxisMovementyx; // X & Y Movement

    //Move speed
    [SerializeField] public float Speed;
    [SerializeField] public float MovingSpeed;
    [SerializeField] public float SprintSpeed;

    public override void Init(PlayerController parent)
    {
        base.Init(parent);
    }

    public override void GetInput()
    {
        //Get's raw X value and makes it (+1 up / -1 down)
        AxisMovementyx.x = Input.GetAxisRaw("Horizontal");
        //Get's raw Y value and makes it (+1 up / -1 down)
        AxisMovementyx.y = Input.GetAxisRaw("Vertical");
    }

    public override void Update()
    {
        Rb.velocity = AxisMovementyx.normalized * Speed;
        Speed = MovingSpeed;
    }

    public override void FixedUpdate() {}

    public override void ChangeState(){}

    public override void Exit(){}





}
