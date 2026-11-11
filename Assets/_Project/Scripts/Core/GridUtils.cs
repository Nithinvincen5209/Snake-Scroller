using UnityEngine;

namespace SnakeScroller
{
    // Pure grid math. No GameObjects here, so game logic stays separate from visuals.
    // The grid is centered on the world origin. Cell (0,0) is the bottom-left cell.
    public static class GridUtils
    {
        // Converts a grid cell to the world position of its center, so visuals know where to go.
        public static Vector3 CellToWorld(Vector2Int cell, GridConfig config)
        {
            Vector2 bottomLeft = GetBottomLeftCorner(config);
            float half = config.CellSize * 0.5f;
            float x = bottomLeft.x + cell.x * config.CellSize + half;
            float y = bottomLeft.y + cell.y * config.CellSize + half;
            return new Vector3(x, y, 0f);
        }

        // Tells if a cell is on the grid, so we can detect hitting the border.
        public static bool IsInside(Vector2Int cell, GridConfig config)
        {
            bool insideX = cell.x >= 0 && cell.x < config.Width;
            bool insideY = cell.y >= 0 && cell.y < config.Height;
            return insideX && insideY;
        }

        // Wraps a cell around the grid edges, so exiting one side
        // re-enters from the opposite side (toroidal grid).
        public static Vector2Int WrapCell(Vector2Int cell, GridConfig config)
        {
            int x = ((cell.x % config.Width) + config.Width) % config.Width;
            int y = ((cell.y % config.Height) + config.Height) % config.Height;
            return new Vector2Int(x, y);
        }

        // Finds the world position of the grid's bottom-left corner, so the grid is centered.
        public static Vector2 GetBottomLeftCorner(GridConfig config)
        {
            float totalWidth = config.Width * config.CellSize;
            float totalHeight = config.Height * config.CellSize;
            return new Vector2(-totalWidth * 0.5f, -totalHeight * 0.5f);
        }
    }
}
