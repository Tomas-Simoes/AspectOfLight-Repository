using UnityEngine;

[CreateAssetMenu(
    fileName = "D_ThornborneUGAttack",
    menuName = "Data/State Data/Specific Enemy/Thornborne/Healing State"
)]
public class D_ThornborneHealingState : ScriptableObject
{
    public float heal;
    public float timeToHeal;
}
