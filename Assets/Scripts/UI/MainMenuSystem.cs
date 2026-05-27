using UnityEngine;
using UnityEngine.UIElements;
using Unity.Entities;
using System.Linq;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private VisualElement _root;
    private Button _startButton;

    private void OnEnable()
    {
        if (uiDocument == null) return;

        _root = uiDocument.rootVisualElement;
        _startButton = _root.Q<Button>("ContinueButton");

        if (_startButton != null)
        {
            _startButton.clicked += OnStartButtonClicked;
        }
    }

    private void OnDisable()
    {
        if (_startButton != null)
        {
            _startButton.clicked -= OnStartButtonClicked;
        }
    }

    private void OnStartButtonClicked()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        if (world == null) return;

        var entityManager = world.EntityManager;
        var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<GameStateComponent>());

        if (query.IsEmpty)
            return;

        var gameStateEntity = query.GetSingletonEntity();
        var data = entityManager.GetComponentData<GameStateComponent>(gameStateEntity);

        data.IsGameStarted = true;
        data.IsPaused = false;

        entityManager.SetComponentData(gameStateEntity, data);
        _root.style.display = DisplayStyle.None;
    }
}