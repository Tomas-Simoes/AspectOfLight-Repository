using UnityEngine;

[CreateAssetMenu(fileName = "newDeathScreen", menuName = "Data/Death Screen", order = 0)]
public class D_DeathScreen : ScriptableObject
{
    public GameObject deathScreen;
    public string[] deathMessages;
}
