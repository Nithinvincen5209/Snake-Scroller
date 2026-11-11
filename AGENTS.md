# Project: Snake Scroller (Unity 6, C#)

This is a job test. I must explain every function in an interview.
So keep code SIMPLE, SMALL and READABLE.

## Code rules

- One script = one job. Keep scripts under about 150 lines.
- Functions do one thing, max about 20 lines. Clear names:
  MoveHead(), TryEat(), CheckDeath().
- Put a short comment above each function: WHY it exists.
- No magic numbers. Use [SerializeField] fields or ScriptableObjects.
- Keep game logic (Vector2Int cells) separate from visuals (GameObjects).
- Use C# events (System.Action) for talking between scripts.
  No FindObjectOfType, no singletons except GameManager if needed.
- Use object pooling for snake body segments.
- Namespace: SnakeScroller. Private fields: _camelCase.
  Public members and methods: PascalCase.
- No clever tricks. Prefer the simple solution.
- Do not add features I did not ask for.

## Workflow rules

- Do ONLY the step I ask for.
- After writing code, list the files you changed and explain
  each function in simple words.
- Tell me exactly what to set up in the Unity Editor
  (objects, components, references).

## Project facts

- Unity 6000.5.6f1, URP 2D template.
- Active Input Handling = Input System (new) only.
  Input code must use UnityEngine.InputSystem (Keyboard.current).
