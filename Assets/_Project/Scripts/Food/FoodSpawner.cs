using System;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeScroller
{
    // Places food on a free cell and tells others when the snake eats it.
    public class FoodSpawner : MonoBehaviour
    {
        [SerializeField] private GridConfig _gridConfig;
        [SerializeField] private SnakeController _snakeController;
        [SerializeField] private FoodData[] _foods;
        [SerializeField] private GameObject _foodPrefab;
        [SerializeField, Range(0.1f, 1f)] private float _fillPercent = 0.9f;

        // Fired when the snake eats food. Score and effects listen to this.
        public event Action<FoodData> OnFoodEaten;

        private GameObject _foodObject;
        private SpriteRenderer _foodRenderer;
        private float _lifeTimer;
        private readonly List<Vector2Int> _freeCells = new List<Vector2Int>();

        // The cell the food is on. Game logic uses this, not the GameObject.
        public Vector2Int FoodCell { get; private set; }

        // The food type currently on the grid.
        public FoodData CurrentFood { get; private set; }

        // Creates the one food object we reuse, so we never Instantiate per meal.
        private void Awake()
        {
            if (_foodPrefab == null) return;
            _foodObject = Instantiate(_foodPrefab, transform);
            _foodRenderer = _foodObject.GetComponent<SpriteRenderer>();
        }

        // Cleans up the food object when this spawner is destroyed.
        private void OnDestroy()
        {
            if (_foodObject != null) Destroy(_foodObject);
        }

        // Listens to the snake so we can check eating after every move.
        private void OnEnable()
        {
            if (_snakeController != null) _snakeController.OnMoved += CheckEaten;
        }

        // Stops listening when disabled, so no events reach a dead object.
        private void OnDisable()
        {
            if (_snakeController != null) _snakeController.OnMoved -= CheckEaten;
        }

        // Places the first food when the game starts.
        private void Start()
        {
            SpawnFood();
        }

        // Counts down the food's lifetime and moves it when time runs out.
        private void Update()
        {
            if (CurrentFood == null || CurrentFood.LifetimeSeconds <= 0f) return;

            _lifeTimer -= Time.deltaTime;
            if (_lifeTimer <= 0f) SpawnFood();
        }

        // Picks a food type and moves it to a new random free cell.
        public void SpawnFood()
        {
            bool found = PickRandomFreeCell(out Vector2Int cell);
            _foodObject.SetActive(found);
            if (!found) return;

            CurrentFood = PickRandomFood();
            _lifeTimer = CurrentFood.LifetimeSeconds;
            FoodCell = cell;
            _foodObject.transform.position = GridUtils.CellToWorld(cell, _gridConfig);
            ShowFood(CurrentFood);
        }

        // Picks a food type at random. A higher SpawnWeight means more often.
        private FoodData PickRandomFood()
        {
            if (_foods == null || _foods.Length == 0) return null;

            float total = 0f;
            foreach (FoodData food in _foods) total += food.SpawnWeight;
            if (total <= 0f) return _foods[0];

            float roll = UnityEngine.Random.Range(0f, total);
            foreach (FoodData food in _foods)
            {
                roll -= food.SpawnWeight;
                if (roll <= 0f) return food;
            }
            return _foods[_foods.Length - 1];
        }

        // Sets the food's sprite and colour, and scales it to fit one cell.
        private void ShowFood(FoodData food)
        {
            _foodRenderer.sprite = food.Sprite;
            _foodRenderer.color = food.Color;
            if (food.Sprite == null) return;

            Vector2 spriteSize = food.Sprite.bounds.size;
            float biggestSide = Mathf.Max(spriteSize.x, spriteSize.y);
            float scale = _gridConfig.CellSize * _fillPercent / biggestSide;
            _foodObject.transform.localScale = new Vector3(scale, scale, 1f);
        }

        // Collects all free cells and picks one at random. False if the grid is full.
        private bool PickRandomFreeCell(out Vector2Int cell)
        {
            _freeCells.Clear();
            for (int x = 0; x < _gridConfig.Width; x++)
            {
                for (int y = 0; y < _gridConfig.Height; y++)
                {
                    Vector2Int candidate = new Vector2Int(x, y);
                    if (IsFreeCell(candidate)) _freeCells.Add(candidate);
                }
            }

            cell = Vector2Int.zero;
            if (_freeCells.Count == 0) return false;

            cell = _freeCells[UnityEngine.Random.Range(0, _freeCells.Count)];
            return true;
        }

        // A cell is free when it is on the grid and the snake is not on it.
        private bool IsFreeCell(Vector2Int cell)
        {
            if (!GridUtils.IsInside(cell, _gridConfig)) return false;
            return !_snakeController.Body.Contains(cell);
        }

        // After each move: if the head is on the food, grow, tell others, and respawn.
        private void CheckEaten()
        {
            if (CurrentFood == null || _snakeController.Head != FoodCell) return;

            FoodData eaten = CurrentFood;
            _snakeController.Grow();
            OnFoodEaten?.Invoke(eaten);
            SpawnFood();
        }
    }
}
