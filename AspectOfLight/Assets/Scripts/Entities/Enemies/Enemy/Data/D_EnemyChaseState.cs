using UnityEngine;

[CreateAssetMenu(fileName = "newChaseData", menuName = "Data/State Data/General Enemy/Chase State")]
public class D_EnemyChaseState : ScriptableObject
{
    public float chaseSpeed = 4f;
    public float minDistanceBetweenPlayer = 0.5f;
}
