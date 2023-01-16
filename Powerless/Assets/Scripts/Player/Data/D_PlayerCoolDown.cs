using UnityEngine;

[CreateAssetMenu(fileName = "newCoolDownData", menuName = "Data/State Data/Player/CoolDown")]
public class D_PlayerCoolDown : ScriptableObject
{
    // Dash //
    public float dashCD = 4;
  public float timeSinceDash = 4;

    // Necromancer //
  public float necromancerCD = 10;
  public float timeSinceNecromancer = 10;
}
