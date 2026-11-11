using UnityEngine;
using UnityEngine.InputSystem;

namespace SnakeScroller
{
    // Manages game state: Menu, Playing, GameOver.
    // Listens to snake death and handles restart.
    public class GameManager : MonoBehaviour
    {
        public enum GameState { Menu, Playing, GameOver }

        [SerializeField] private SnakeController _snakeController;
        [SerializeField] private TickService _tickService;
        [SerializeField] private FoodSpawner _foodSpawner;
        [SerializeField] private ScoreService _scoreService;
        [SerializeField] private GameObject _gameOverCanvas;

        public GameState CurrentState { get; private set; } = GameState.Playing;

        private void OnEnable()
        {
            if (_snakeController != null) _snakeController.OnDied += HandleDeath;
        }

        private void OnDisable()
        {
            if (_snakeController != null) _snakeController.OnDied -= HandleDeath;
        }

        private void Update()
        {
            if (CurrentState != GameState.GameOver) return;

            var kb = Keyboard.current;
            if (kb == null) return;

            if (kb.rKey.wasPressedThisFrame || kb.spaceKey.wasPressedThisFrame)
            {
                Restart();
            }
        }

        // Called when the snake dies.
        private void HandleDeath()
        {
            CurrentState = GameState.GameOver;
            _tickService.StopTicking();
            if (_gameOverCanvas != null) _gameOverCanvas.SetActive(true);
        }

        // Resets everything and starts a new run.
        public void Restart()
        {
            CurrentState = GameState.Playing;
            if (_gameOverCanvas != null) _gameOverCanvas.SetActive(false);
            _snakeController.ResetSnake();
            _foodSpawner.SpawnFood();
            _scoreService.ResetScore();
            _tickService.StartTicking();
        }
    }
}
