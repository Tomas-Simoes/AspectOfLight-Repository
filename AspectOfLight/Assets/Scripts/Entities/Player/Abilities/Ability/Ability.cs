using UnityEngine;

[System.Serializable]
public abstract class Ability
{
    public string abilityName = "No name";
    public string abilityPath;
    public float abilityCD = 0;
    public int staminaCost = 0;
    public int unlockCost = 0;
    public bool isAvailable = false;
    public Sprite iconAvailable;
    public Sprite iconUnavailable;

    [TextArea(3, 10)]
    public string description = "No description yet";

    protected Player player;

    public virtual void Cast()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
}
