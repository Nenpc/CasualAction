using Unity.Entities;

/// <summary>
/// Вспомогательный класс для работы с данными скилов в ECS
/// </summary>
public static class SkillDataHelper
{
    /// <summary>
    /// Получить данные скила по имени и уровню
    /// </summary>
    public static bool TryGetSkillData(
        DynamicBuffer<SkillDataBuffer> skillBuffer,
        string skillName,
        int level,
        out SkillDataBuffer skillData)
    {
        skillData = default;
        int skillNameHash = skillName.GetHashCode();
        level = System.Math.Clamp(level, 0, 4);

        foreach (var data in skillBuffer)
        {
            if (data.SkillNameHash == skillNameHash && data.Level == level)
            {
                skillData = data;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Получить данные скила по id и уровню
    /// </summary>
    public static bool TryGetSkillData(
        DynamicBuffer<SkillDataBuffer> skillBuffer,
        int skillId,
        int level,
        out SkillDataBuffer skillData)
    {
        skillData = default;
        level = System.Math.Clamp(level, 0, 4);

        foreach (var data in skillBuffer)
        {
            if (data.SkillId == skillId && data.Level == level)
            {
                skillData = data;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Получить данные скила по имени для конкретного уровня
    /// </summary>
    public static SkillDataBuffer GetSkillData(
        DynamicBuffer<SkillDataBuffer> skillBuffer,
        string skillName,
        int level)
    {
        if (TryGetSkillData(skillBuffer, skillName, level, out var skillData))
        {
            return skillData;
        }

        return default;
    }

    /// <summary>
    /// Проверить наличие скила в буфере
    /// </summary>
    public static bool HasSkill(DynamicBuffer<SkillDataBuffer> skillBuffer, string skillName)
    {
        int skillNameHash = skillName.GetHashCode();
        foreach (var data in skillBuffer)
        {
            if (data.SkillNameHash == skillNameHash)
                return true;
        }

        return false;
    }
}
