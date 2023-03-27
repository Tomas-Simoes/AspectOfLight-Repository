using UnityEngine;

[CreateAssetMenu(fileName = "newStaminaData", menuName = "Data/Others/Player/Stamina")]
public class D_PlayerStamina : ScriptableObject
{
    public int MAX_STAMINA;
    public int recoverStaminaRate;
}
