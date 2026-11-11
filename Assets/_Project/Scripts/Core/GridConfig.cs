using UnityEngine;

namespace SnakeScroller
{
    // Holds the grid settings in one asset, so we can tweak them without changing code.
    [CreateAssetMenu(fileName = "GridConfig", menuName = "SnakeScroller/Grid Config")]
    public class GridConfig : ScriptableObject
    {
        [SerializeField, Min(1)] private int _width = 20;
        [SerializeField, Min(1)] private int _height = 20;
        [SerializeField, Min(0.01f)] private float _cellSize = 1f;
        [SerializeField, Min(0.01f)] private float _tickSeconds = 0.15f;

        // Number of cells from left to right.
        public int Width => _width;

        // Number of cells from bottom to top.
        public int Height => _height;

        // Size of one cell in world units.
        public float CellSize => _cellSize;

        // Time between two snake moves.
        public float TickSeconds => _tickSeconds;
    }
}
