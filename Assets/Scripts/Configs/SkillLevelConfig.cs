using UnityEngine;

[System.Serializable]
public class SkillLevelConfig
{
    [Tooltip("Prefab, который будет появляться, когда скил активен.")]
    public GameObject Prefab;

    [Tooltip("Сколько жизней отнимается у врага при столкновении префаба скила с врагом.")]
    public float Damage = 1f;

    [Tooltip("Время перезарядки скила на этом уровне.")]
    public float Cooldown = 1f;
}
