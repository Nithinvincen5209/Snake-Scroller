# Snake Scroller

Classic Snake in Unity 6 (6000.5.6f1, URP 2D) with extras.

## How to play
- **Arrow keys / WASD** — move the snake
- **R or Space** — restart after Game Over
- Eat food to grow and score. Don't hit your own body. Wrap-around edges (no wall death).

## What I added beyond basic Snake (and why)
- **Wrap-around movement** — snake exits one edge and re-enters the opposite. Keeps gameplay fast and fun.
- **Speed scaling** — snake speeds up every 50 points. Adds dynamic difficulty.
- **Data-driven food types** — food is a ScriptableObject (apple, speed-up, double score, etc.). New types = new asset, zero code changes.
- **Object-pooled segments** — reusable GameObjects, no per-tick Instantiate/Destroy.
- **Event-driven architecture** — scripts communicate via System.Action events. No FindObjectOfType or singletons.

## AI tools used and for what
- **Claude** — project planning, step-by-step build plan, architecture decisions, AGENTS.md rules file.
- **ChatGPT** — created game sprites and art assets (snake, food, background, wall tile).
- **OpenCode** — wrote all C# scripts step by step, following AGENTS.md. Reviewed and tested each before committing.