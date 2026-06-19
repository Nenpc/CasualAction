using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Level Config")]
public class LevelConfig : ScriptableObject
{
    public LevelConfigEntry[] Levels;
}

[System.Serializable]
public class LevelConfigEntry
{
    public int LevelId;
    public int EnemyCount = 5;
    public float LevelDuration = 60f;
}
