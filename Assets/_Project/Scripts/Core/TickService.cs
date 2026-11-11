using System;
using UnityEngine;

namespace SnakeScroller
{
    // The game clock. Fires one OnTick event every tick seconds,
    // so all game logic moves in steps instead of every frame.
    public class TickService : MonoBehaviour
    {
        [SerializeField] private GridConfig _gridConfig;
        [SerializeField] private bool _startOnPlay = true;

        // Fired once every tick. Listeners move one step per event.
        public event Action OnTick;

        private float _tickSeconds;
        private float _timer;
        private bool _running;

        // Copies the tick time from the config once, so runtime speed
        // changes never edit the asset itself.
        private void Awake()
        {
            if (_gridConfig == null)
            {
                Debug.LogError("TickService: assign a GridConfig.", this);
                enabled = false;
                return;
            }
            _tickSeconds = _gridConfig.TickSeconds;
        }

        // Starts the clock on Play while there is no GameManager yet to call StartTicking().
        private void Start()
        {
            if (_startOnPlay)
            {
                StartTicking();
            }
        }

        // Counts frame time and fires OnTick each time a full tick passed.
        private void Update()
        {
            if (!_running) return;

            _timer += Time.deltaTime;
            while (_timer >= _tickSeconds)
            {
                _timer -= _tickSeconds;
                OnTick?.Invoke();
            }
        }

        // Starts the clock. GameManager calls this when the game starts.
        public void StartTicking()
        {
            _timer = 0f;
            _running = true;
        }

        // Stops the clock. GameManager calls this on death.
        public void StopTicking()
        {
            _running = false;
        }

        // Changes speed at runtime (speed-up food later). Clamped so the
        // game never freezes or runs impossibly fast.
        public void SetTickSeconds(float seconds)
        {
            _tickSeconds = Mathf.Clamp(seconds, 0.01f, 1f);
        }
    }
}
