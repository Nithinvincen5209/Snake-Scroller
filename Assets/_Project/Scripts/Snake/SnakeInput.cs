using UnityEngine;
using UnityEngine.InputSystem;

namespace SnakeScroller
{
    // Reads keyboard input and turns it into a safe grid direction.
    // It only reports; SnakeController still owns the movement.
    public class SnakeInput : MonoBehaviour
    {
        [SerializeField] private SnakeController _snakeController;

        // The last direction the player asked for between two ticks.
        public Vector2Int WantedDirection { get; private set; } = Vector2Int.right;

        private void Update()
        {
            if (Keyboard.current == null) return;

            Vector2Int input = ReadInput();
            if (input == Vector2Int.zero) return;

            // Never allow a 180-degree turn into the snake's own neck.
            if (_snakeController != null && input == -_snakeController.Direction) return;

            WantedDirection = input;
            _snakeController?.SetDirection(input);
        }

        // Reads arrow keys and WASD. Last pressed key in this frame wins.
        private Vector2Int ReadInput()
        {
            Vector2Int result = Vector2Int.zero;
            var kb = Keyboard.current;

            if (kb.upArrowKey.wasPressedThisFrame || kb.wKey.wasPressedThisFrame)
                result = Vector2Int.up;
            if (kb.downArrowKey.wasPressedThisFrame || kb.sKey.wasPressedThisFrame)
                result = Vector2Int.down;
            if (kb.leftArrowKey.wasPressedThisFrame || kb.aKey.wasPressedThisFrame)
                result = Vector2Int.left;
            if (kb.rightArrowKey.wasPressedThisFrame || kb.dKey.wasPressedThisFrame)
                result = Vector2Int.right;

            return result;
        }
    }
}
