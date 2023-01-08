using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Walk")]
public class WalkState : State<PlayerController>
{
    InputHandler inputHandler;
    private Rigidbody2D rigidbody;
    public Vector2 axisMovement;
    private bool dashing = false;

    //Move speed
    [SerializeField]
    public float walkSpeed;

    public override void Init(PlayerController parent)
    {
        dashing = false;

        if (rigidbody == null)
            rigidbody = parent.GetComponent<Rigidbody2D>();

        if (inputHandler == null)
            inputHandler = parent.GetComponent<InputHandler>();

        base.Init(parent);
    }

    public override void GetInput()
    {
        axisMovement.x = inputHandler.xAxis;
        axisMovement.y = inputHandler.yAxis;

        dashing = inputHandler.pressedDash;
    }

    public override void Update()
    {
        rigidbody.velocity = axisMovement.normalized * walkSpeed;
    }

    public override void FixedUpdate() { }

    public override void ChangeState()
    {
        if (dashing)
        {
            _runner.SetState(typeof(DashState));
        }
    }

    public override void Exit() { }
}
