using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;

public class ConfigAuthoring : MonoBehaviour
{
    public CharacterConfig CharacterConfig;
    public EnemyConfig Config;
    public SkillLibraryConfig SkillLibrary;
    public LevelConfig LevelConfig;
    public int SelectedLevelIndex;
    public int SelectedCharacterIndex;

    class Baker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.None);

            CharacterConfigEntry characterEntry = null;
            if (authoring.CharacterConfig != null && authoring.CharacterConfig.Characters != null && authoring.CharacterConfig.Characters.Length > 0)
            {
                int index = Mathf.Clamp(authoring.SelectedCharacterIndex, 0, authoring.CharacterConfig.Characters.Length - 1);
                characterEntry = authoring.CharacterConfig.Characters[index];
            }

            if (characterEntry == null)
            {
                Debug.LogWarning("CharacterConfig не задан или не содержит ни одной записи на ConfigAuthoring.", authoring);
            }

            // Добавляем разделённые конфигурационные компоненты
            AddComponent(entity, new CharacterComponent
            {
                ID = characterEntry != null ? characterEntry.ID : 0,
                CharacterPrefab = characterEntry != null && characterEntry.Prefab != null
                    ? GetEntity(characterEntry.Prefab, TransformUsageFlags.Dynamic)
                    : Entity.Null,
                MaxHealth = characterEntry != null ? characterEntry.Health : 0f,
                MoveSpeed = characterEntry != null ? characterEntry.MoveSpeed : 0f,
                DefaultSkillId = characterEntry != null ? characterEntry.DefaultSkillId : 0,
            });

            LevelConfigEntry levelEntry = null;
            if (authoring.LevelConfig != null && authoring.LevelConfig.Levels != null && authoring.LevelConfig.Levels.Length > 0)
            {
                int li = Mathf.Clamp(authoring.SelectedLevelIndex, 0, authoring.LevelConfig.Levels.Length - 1);
                levelEntry = authoring.LevelConfig.Levels[li];
            }

            AddComponent(entity, new LevelComponent
            {
                EnemyCount = levelEntry != null ? levelEntry.EnemyCount : 5,
                LevelDuration = levelEntry != null ? levelEntry.LevelDuration : 60f,
                LevelId = levelEntry != null ? levelEntry.LevelId : 0
            });

            AddComponent(entity, new SkillLibraryComponent
            {
                SkillLibrary = Entity.Null
            });

            // Создаём пустой буфер для данных скилов, который будет заполнен SkillConfigInitSystem
            AddBuffer<SkillDataBuffer>(entity);

            var buffer = AddBuffer<EnemyPrefabBuffer>(entity);
            foreach (var enemy in authoring.Config.Enemies)
            {
                buffer.Add(new EnemyPrefabBuffer 
                { 
                    Value = GetEntity(enemy.Prefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}