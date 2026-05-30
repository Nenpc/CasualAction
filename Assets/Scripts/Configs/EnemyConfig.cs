using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemy Config")]
public class EnemyConfig : ScriptableObject
{
    public EnemyConfigEntry[] Enemies;
}

[System.Serializable]
public class EnemyConfigEntry
{
    public string Name;
    public GameObject Prefab;
    public float Health = 10f;
    public float MoveSpeed = 2f;
    public int ExperienceReward = 10;
}
