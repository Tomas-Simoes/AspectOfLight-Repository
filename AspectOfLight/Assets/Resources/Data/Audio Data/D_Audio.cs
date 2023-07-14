using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New audio data", menuName = "Data/Audio/Audio Data", order = 0)]
public class D_Audio : ScriptableObject
{
    public List<Sound> sounds;
}
