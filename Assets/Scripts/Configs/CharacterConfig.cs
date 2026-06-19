using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "Configs/Character Config")]
public class CharacterConfig : ScriptableObject
{
    public CharacterConfigEntry[] Characters;
}

[System.Serializable]
public class CharacterConfigEntry
{
    public int ID;
    public string Name;
    public GameObject Prefab;
    public float Health = 100f;
    public float MoveSpeed = 3f;
    public int DefaultSkillId;
}
