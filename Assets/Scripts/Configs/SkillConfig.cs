using UnityEngine;

[CreateAssetMenu(fileName = "SkillConfig", menuName = "Configs/Skill Config")]
public class SkillConfig : ScriptableObject
{
    [Tooltip("Имя скила для отображения в инспекторе и выбора.")]
    public string SkillName;

    [Tooltip("Уровни улучшения скила. Всего 5 уровней.")]
    public SkillLevelConfig[] Levels = new SkillLevelConfig[5];

    public int LevelCount => Levels != null ? Levels.Length : 0;

    public SkillLevelConfig GetLevel(int index)
    {
        if (Levels == null || index < 0 || index >= Levels.Length)
            return null;

        return Levels[index];
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Levels == null || Levels.Length != 5)
            System.Array.Resize(ref Levels, 5);
    }
#endif
}
