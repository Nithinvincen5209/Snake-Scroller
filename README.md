# Snake Scroller

Classic Snake built in Unity 6 (6000.5.6f1, URP 2D) with extras.

## How to play
- **Arrow keys / WASD** — move the snake
- **R or Space** — restart after Game Over
- Eat food to grow and score. Don't hit your own body.

## What I added beyond basic Snake
- **Wrap-around movement** — exiting one edge re-enters from the opposite side.
- **Speed scaling** — snake gets faster every 50 points.
- **Data-driven food types** — food is a ScriptableObject. New types need only a new asset.
- **Object-pooled snake segments** — reusable GameObjects, no per-tick Instantiate/Destroy.
- **Event-driven architecture** — scripts talk via System.Action events, no FindObjectOfType.

## Project structure
```
Assets/_Project/
  Scripts/Core/    GridConfig, GridUtils, TickService, GameManager, ScoreService, SpeedScaler
  Scripts/Snake/   SnakeController, SnakeInput, SnakeView
  Scripts/Food/    FoodData, FoodSpawner, SpeedBoost
  Scripts/UI/      ScoreView
  ScriptableObjects/  GridConfig, food type assets
  Prefabs/  Scenes/  Art/  Audio/
```

## AI tools used
- **OpenCode** — wrote code step by step following the AGENTS.md rules file.
- I reviewed, tested, and understood every script before committing.