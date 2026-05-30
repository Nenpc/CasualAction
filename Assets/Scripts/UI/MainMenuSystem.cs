using UnityEngine;
using UnityEngine.UIElements;
using Unity.Entities;

public class MainMenuManager : MonoBehaviour
{
    static string ContinueButtonName = "ContinueButton";
    static string CloseButtonName = "CloseButton";

    [SerializeField] private UIDocument uiDocument;

    private VisualElement _root;
    private Button _startButton;
    private Button _closeButton;

    private void OnEnable()
    {
        if (uiDocument == null) return;

        _root = uiDocument.rootVisualElement;
        _startButton = _root.Q<Button>(ContinueButtonName);
        _closeButton = _root.Q<Button>(CloseButtonName);

        if (_startButton != null)
        {
            _startButton.clicked += OnStartButtonClicked;
        }

        if (_closeButton != null)
        {
            _closeButton.clicked += OnCloseButtonClicked;
        }
    }

    private void OnDisable()
    {
        if (_startButton != null)
        {
            _startButton.clicked -= OnStartButtonClicked;
        }

        if (_closeButton != null)
        {
            _closeButton.clicked -= OnCloseButtonClicked;
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

    private void OnCloseButtonClicked()
    {
        Application.Quit();
    }
}