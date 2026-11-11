using System;
using UnityEngine;

namespace SnakeScroller
{
    // Keeps the player's score. It listens to eaten food and adds that food's points.
    public class ScoreService : MonoBehaviour
    {
        [SerializeField] private FoodSpawner _foodSpawner;

        // Fired with the new score each time it changes. The UI will listen later.
        public event Action<int> OnScoreChanged;

        // The current score. Only this script can change it.
        public int Score { get; private set; }

        // Listens for eaten food so we can add points.
        private void OnEnable()
        {
            _foodSpawner.OnFoodEaten += AddScore;
        }

        // Stops listening when disabled, so no events reach a dead object.
        private void OnDisable()
        {
            _foodSpawner.OnFoodEaten -= AddScore;
        }

        // Starts every game at zero.
        private void Start()
        {
            ResetScore();
        }

        // Sets the score back to 0 (also useful on restart later).
        public void ResetScore()
        {
            Score = 0;
            OnScoreChanged?.Invoke(Score);
        }

        // Adds the eaten food's points and tells listeners.
        private void AddScore(FoodData food)
        {
            Score += food.ScoreValue;
            OnScoreChanged?.Invoke(Score);
        }
    }
}
