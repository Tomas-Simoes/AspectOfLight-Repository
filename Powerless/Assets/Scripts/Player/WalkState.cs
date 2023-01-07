using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Walk")]
public class WalkState : State<PlayerController>
{

    public Rigidbody2D _rigidbody;

    public Vector2 AxisMovementyx; // X & Y Movement

    //Move speed
    [SerializeField] public float Speed;
    [SerializeField] public float MovingSpeed;
    [SerializeField] public float SprintSpeed;

    public override void Init(PlayerController parent)
    {
        if (_rigidbody == null) _rigidbody = parent.GetComponent<Rigidbody2D>();
        base.Init(parent);
    }

    public override void GetInput()
    {

        _rigidbody.velocity = AxisMovementyx.normalized * Speed;

        //Get's raw X value and makes it (+1 up / -1 down)
        AxisMovementyx.x = Input.GetAxisRaw("Horizontal");
        //Get's raw Y value and makes it (+1 up / -1 down)
        AxisMovementyx.y = Input.GetAxisRaw("Vertical");

    
    }

    public override void Update()
    {
        Speed = MovingSpeed;
    }

    public override void FixedUpdate() {}

    public override void ChangeState(){}

    public override void Exit(){}

}
