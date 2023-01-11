using UnityEngine;

[CreateAssetMenu(fileName = "newCoolDownData", menuName = "Data/State Data/Player/CoolDown")]
public class D_PlayerCoolDown : ScriptableObject
{
  public float dashCD = 4;
  public float timeSinceDash = 4;
}
