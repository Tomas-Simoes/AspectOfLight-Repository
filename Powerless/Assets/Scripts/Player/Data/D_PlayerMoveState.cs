using UnityEngine;

[CreateAssetMenu(fileName = "newMoveData", menuName = "Data/State Data/Player/Move State")]
public class D_PlayerMoveState : ScriptableObject
{
  public Vector2 axisMovement;
  public float movementSpeed = 3;
}
