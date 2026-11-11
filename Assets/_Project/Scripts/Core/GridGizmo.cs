using UnityEngine;

namespace SnakeScroller
{
    // Draws the grid lines in the Scene view, only to help us see the grid while working.
    public class GridGizmo : MonoBehaviour
    {
        [SerializeField] private GridConfig _config;
        [SerializeField] private Color _lineColor = new Color(1f, 1f, 1f, 0.3f);

        // Unity calls this in the Editor to draw debug shapes.
        private void OnDrawGizmos()
        {
            if (_config == null)
            {
                return;
            }

            Gizmos.color = _lineColor;
            DrawVerticalLines();
            DrawHorizontalLines();
        }

        // Draws one line for each column border.
        private void DrawVerticalLines()
        {
            Vector2 corner = GridUtils.GetBottomLeftCorner(_config);
            float top = corner.y + _config.Height * _config.CellSize;

            for (int x = 0; x <= _config.Width; x++)
            {
                float lineX = corner.x + x * _config.CellSize;
                Gizmos.DrawLine(new Vector3(lineX, corner.y, 0f), new Vector3(lineX, top, 0f));
            }
        }

        // Draws one line for each row border.
        private void DrawHorizontalLines()
        {
            Vector2 corner = GridUtils.GetBottomLeftCorner(_config);
            float right = corner.x + _config.Width * _config.CellSize;

            for (int y = 0; y <= _config.Height; y++)
            {
                float lineY = corner.y + y * _config.CellSize;
                Gizmos.DrawLine(new Vector3(corner.x, lineY, 0f), new Vector3(right, lineY, 0f));
            }
        }
    }
}
