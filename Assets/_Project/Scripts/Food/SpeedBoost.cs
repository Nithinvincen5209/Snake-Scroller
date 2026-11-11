using UnityEngine;

namespace SnakeScroller
{
    // Makes the snake faster for a short time after it eats Speed Up food.
    public class SpeedBoost : MonoBehaviour
    {
        [SerializeField] private GridConfig _gridConfig;
        [SerializeField] private TickService _tickService;
        [SerializeField] private FoodSpawner _foodSpawner;

        private float _timeLeft;

        // True while the snake is faster. Handy for testing and later for UI.
        public bool IsBoosted { get; private set; }

        // Listens for eaten food so we can react to Speed Up.
        private void OnEnable()
        {
            if (_foodSpawner != null) _foodSpawner.OnFoodEaten += HandleFoodEaten;
        }

        // Stops listening when disabled, so no events reach a dead object.
        private void OnDisable()
        {
            if (_foodSpawner != null) _foodSpawner.OnFoodEaten -= HandleFoodEaten;
        }

        // Counts down the boost and ends it when time runs out.
        private void Update()
        {
            if (!IsBoosted) return;

            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0f) EndBoost();
        }

        // Only Speed Up food starts a boost; other foods are ignored.
        private void HandleFoodEaten(FoodData food)
        {
            if (food.EffectType != FoodEffectType.SpeedUp) return;
            StartBoost(food);
        }

        // Makes ticks shorter. Eating again only restarts the timer, it does not stack.
        private void StartBoost(FoodData food)
        {
            _tickService.SetTickSeconds(_gridConfig.TickSeconds / food.EffectMultiplier);
            _timeLeft = food.EffectDuration;
            IsBoosted = true;
        }

        // Puts the normal tick time back.
        private void EndBoost()
        {
            _tickService.SetTickSeconds(_gridConfig.TickSeconds);
            IsBoosted = false;
        }
    }
}
