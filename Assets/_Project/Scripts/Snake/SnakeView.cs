using System.Collections.Generic;
using UnityEngine;

namespace SnakeScroller
{
    // Draws the snake using pooled segment GameObjects.
    // It only reads SnakeController data; it never changes game state.
    public class SnakeView : MonoBehaviour
    {
        [SerializeField] private SnakeController _snakeController;
        [SerializeField] private GameObject _segmentPrefab;
        [SerializeField] private Color _headColor = new Color(0.2f, 0.9f, 0.2f);
        [SerializeField] private Color _bodyColor = new Color(0.1f, 0.5f, 0.1f);
        [SerializeField, Min(0)] private int _initialPoolSize = 8;

        private readonly Queue<GameObject> _pool = new Queue<GameObject>();
        private readonly List<GameObject> _active = new List<GameObject>();

        private void Awake()
        {
            // Pre-create a few segments so the first ticks don't allocate.
            for (int i = 0; i < _initialPoolSize; i++)
            {
                _pool.Enqueue(CreateSegment());
            }
        }

        private void OnEnable()
        {
            if (_snakeController != null) _snakeController.OnMoved += Refresh;
        }

        private void OnDisable()
        {
            if (_snakeController != null) _snakeController.OnMoved -= Refresh;
        }

        private void Start()
        {
            // Show the snake before the first tick fires.
            Refresh();
        }

        // Called once per tick after the snake moves.
        private void Refresh()
        {
            if (_snakeController == null || _segmentPrefab == null) return;
            if (_snakeController.GridConfig == null) return;

            var body = _snakeController.Body;
            int needed = body.Count;

            // Activate segments from the pool until we have enough.
            while (_active.Count < needed)
            {
                GameObject seg = _pool.Count > 0 ? _pool.Dequeue() : CreateSegment();
                seg.SetActive(true);
                _active.Add(seg);
            }

            // Return extra segments to the pool.
            while (_active.Count > needed)
            {
                GameObject seg = _active[_active.Count - 1];
                _active.RemoveAt(_active.Count - 1);
                seg.SetActive(false);
                _pool.Enqueue(seg);
            }

            // Position and colour each segment.
            for (int i = 0; i < needed; i++)
            {
                _active[i].transform.position = GridUtils.CellToWorld(body[i], _snakeController.GridConfig);
                var renderer = _active[i].GetComponent<SpriteRenderer>();
                if (renderer != null) renderer.color = (i == 0) ? _headColor : _bodyColor;
            }
        }

        // Creates one disabled segment and parents it to this object.
        private GameObject CreateSegment()
        {
            GameObject seg = Instantiate(_segmentPrefab, transform);
            seg.SetActive(false);
            return seg;
        }
    }
}
