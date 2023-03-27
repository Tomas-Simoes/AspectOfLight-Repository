using UnityEngine;

[CreateAssetMenu(
    fileName = "D_ThornborneUGAttack",
    menuName = "Data/Specific Enemy/Thornborne/UGAttack"
)]
public class D_ThornborneUGAttackState : ScriptableObject
{
    public float ugAttackRange = 10;
    public float ugAttackTickRate = 2;
    public int explosionCount = 5;
    public int wavesCount = 4;
    public int plantsCount = 3;
}
