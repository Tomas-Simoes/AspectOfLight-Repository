using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "D_ScenesData", menuName = "Data/Scenes/Scenes Data")]
public class D_ScenesData : ScriptableObject
{
    public List<GameScene> gameScenes = new List<GameScene>();

    [System.Serializable]
    public class GameScene
    {
        public string name;
        public List<SceneConnection> sceneConnections;
    }

    [System.Serializable]
    public class SceneConnection
    {
        [TextArea(1, 5)]
        public string OptionalDescription;
        public int connectionID;
        public Vector3 position;
    }
}
