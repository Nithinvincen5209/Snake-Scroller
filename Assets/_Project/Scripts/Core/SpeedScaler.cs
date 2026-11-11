using UnityEngine;

namespace SnakeScroller
{
    // Makes the snake faster as the score grows.
    // Each N points shrinks the tick time by a percentage.
    public class SpeedScaler : MonoBehaviour
    {
        [SerializeField] private ScoreService _scoreService;
        [SerializeField] private TickService _tickService;
        [SerializeField] private GridConfig _gridConfig;

        [SerializeField, Min(1)] private int _pointsPerSpeedup = 50;
        [SerializeField, Range(0.5f, 0.99f)] private float _speedupFactor = 0.9f;
        [SerializeField, Min(0.01f)] private float _minTickSeconds = 0.05f;

        private float _baseTickSeconds;

        private void Awake()
        {
            if (_gridConfig != null) _baseTickSeconds = _gridConfig.TickSeconds;
        }

        private void OnEnable()
        {
            if (_scoreService != null) _scoreService.OnScoreChanged += UpdateSpeed;
        }

        private void OnDisable()
        {
            if (_scoreService != null) _scoreService.OnScoreChanged -= UpdateSpeed;
        }

        // Recalculates tick time from the current score.
        private void UpdateSpeed(int score)
        {
            if (_tickService == null) return;

            int levels = score / _pointsPerSpeedup;
            float newTime = _baseTickSeconds * Mathf.Pow(_speedupFactor, levels);
            newTime = Mathf.Max(_minTickSeconds, newTime);
            _tickService.SetTickSeconds(newTime);
        }
    }
}
