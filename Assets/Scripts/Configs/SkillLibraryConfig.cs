using UnityEngine;

[CreateAssetMenu(fileName = "SkillLibraryConfig", menuName = "Configs/Skill Library")]
public class SkillLibraryConfig : ScriptableObject
{
    public SkillConfig[] Skills;

    public SkillConfig GetSkill(string skillName)
    {
        if (Skills == null || string.IsNullOrEmpty(skillName))
            return null;

        for (int i = 0; i < Skills.Length; i++)
        {
            if (Skills[i] != null && Skills[i].SkillName == skillName)
                return Skills[i];
        }

        return null;
    }
}
