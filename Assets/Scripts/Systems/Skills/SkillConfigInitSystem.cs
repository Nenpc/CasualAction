using Unity.Entities;
using UnityEngine;

/// <summary>
/// Система инициализации, которая загружает SkillLibraryConfig и переносит его данные в ECS буферы
/// </summary>
public partial struct SkillConfigInitSystem : ISystem
{
    private bool _initialized;

    public void OnCreate(ref SystemState state)
    {
        _initialized = false;
        state.RequireForUpdate<ConfigComponent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        if (_initialized)
            return;

        var configEntity = SystemAPI.GetSingletonEntity<ConfigComponent>();
        var config = SystemAPI.GetSingleton<ConfigComponent>();

        // Загружаем SkillLibraryConfig из Resources
        var skillLibrary = Resources.Load<SkillLibraryConfig>("Configs/SkillLibrary");
        if (skillLibrary == null)
        {
            Debug.LogWarning("SkillLibrary не найден в Resources/Configs/");
            _initialized = true;
            return;
        }

        // Получаем буфер для хранения данных скилов
        var entityManager = state.EntityManager;
        var skillBuffer = entityManager.GetBuffer<SkillDataBuffer>(configEntity);
        skillBuffer.Clear();

        // Переносим данные из SkillLibraryConfig в ECS буфер
        if (skillLibrary.Skills != null)
        {
            foreach (var skillConfig in skillLibrary.Skills)
            {
                if (skillConfig == null)
                    continue;

                for (int levelIndex = 0; levelIndex < skillConfig.LevelCount; levelIndex++)
                {
                    var levelConfig = skillConfig.GetLevel(levelIndex);
                    if (levelConfig == null)
                        continue;

                    var prefabEntity = Entity.Null;
                    if (levelConfig.Prefab != null)
                    {
                        // Для конвертации Prefab GameObject в Entity нужно использовать GameObjectEntity или другой способ
                        // Пока оставляем Entity.Null - это можно решить позже
                        // prefabEntity = GetEntity(levelConfig.Prefab, ...);
                    }

                    skillBuffer.Add(new SkillDataBuffer
                    {
                        SkillNameHash = skillConfig.SkillName.GetHashCode(),
                        SkillId = skillConfig.SkillID,
                        Damage = levelConfig.Damage,
                        Cooldown = levelConfig.Cooldown,
                        Prefab = prefabEntity,
                        Level = levelIndex
                    });
                }
            }
        }

        _initialized = true;
    }
}
