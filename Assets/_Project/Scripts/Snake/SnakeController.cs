using System;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeScroller
{
    // The snake's data and movement logic. No visuals here, only cells.
    public class SnakeController : MonoBehaviour
    {
        [SerializeField] private GridConfig _gridConfig;
        [SerializeField] private TickService _tickService;
        [SerializeField, Min(1)] private int _startLength = 3;

        // Fired after the snake moves one cell. SnakeView redraws from this.
        public event Action OnMoved;
        // Fired when the snake dies (wall or self). GameManager ends the run.
        public event Action OnDied;

        // Body as cells, head first.
        private readonly List<Vector2Int> _body = new List<Vector2Int>();
        private Vector2Int _direction = Vector2Int.right;
        private Vector2Int _wantedDirection = Vector2Int.right;
        private bool _growPending;
        private bool _alive;

        public Vector2Int Head => _body.Count > 0 ? _body[0] : Vector2Int.zero;
        public List<Vector2Int> Body => _body;
        public Vector2Int Direction => _direction;
        public bool IsAlive => _alive;
        public GridConfig GridConfig => _gridConfig;

        private void Awake()
        {
            if (_gridConfig == null)
            {
                Debug.LogError("SnakeController: GridConfig is not assigned!", this);
                enabled = false;
                return;
            }
            if (_tickService == null)
            {
                Debug.LogError("SnakeController: TickService is not assigned!", this);
                enabled = false;
                return;
            }
            ResetSnake();
        }

        private void OnEnable()
        {
            if (_tickService != null) _tickService.OnTick += Tick;
        }

        private void OnDisable()
        {
            if (_tickService != null) _tickService.OnTick -= Tick;
        }

        // Puts the snake back to its starting state (also used on restart).
        public void ResetSnake()
        {
            _body.Clear();
            Vector2Int start = Vector2Int.zero;
            if (_gridConfig != null)
            {
                start = new Vector2Int(_gridConfig.Width / 2, _gridConfig.Height / 2);
            }
            for (int i = 0; i < _startLength; i++)
            {
                _body.Add(new Vector2Int(start.x - i, start.y));
            }
            _direction = Vector2Int.right;
            _wantedDirection = Vector2Int.right;
            _growPending = false;
            _alive = true;
        }

        // Sets the wanted direction from input. Blocks a 180-degree turn.
        public void SetDirection(Vector2Int wanted)
        {
            if (wanted == Vector2Int.zero) return;
            if (wanted == -_direction) return;
            _wantedDirection = wanted;
        }

        // Called once per tick by TickService.
        private void Tick()
        {
            if (!_alive) return;

            _direction = _wantedDirection;
            Vector2Int newHead = _body[0] + _direction;

            // Wall check.
            if (_gridConfig != null && !GridUtils.IsInside(newHead, _gridConfig))
            {
                Die();
                return;
            }

            // Self-hit check. The tail will move away this tick, so ignore it.
            int tailIndex = _body.Count - 1;
            for (int i = 0; i < tailIndex; i++)
            {
                if (_body[i] == newHead)
                {
                    Die();
                    return;
                }
            }

            _body.Insert(0, newHead);

            if (_growPending)
            {
                _growPending = false;
            }
            else
            {
                _body.RemoveAt(_body.Count - 1);
            }

            OnMoved?.Invoke();
        }

        // Grows the snake by one cell on the next tick.
        public void Grow()
        {
            _growPending = true;
        }

        // Marks the snake dead and notifies listeners.
        private void Die()
        {
            _alive = false;
            OnDied?.Invoke();
        }
    }
}
