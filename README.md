# Survive the Night — Sprint A: Playable 3D Foundation

A third-person zombie survival prototype made in **Unity 6 (6000.0.60f1)**. The player uses a sword to defend against zombies and must reach an exit to win. The project builds on earlier design-pattern exercises by integrating player input, combat, enemy AI, health, UI, and game states into one playable scene.

## Play the game

**Windows build:** Open the submitted Windows build folder and run the game executable. Keep the executable, its `_Data` folder, and other accompanying Unity build files together. The Windows build is submitted separately from this source repository.

**Unity source:** Open this repository as a project in Unity Hub using Unity **6000.0.60f1**. Open `Assets/Scenes/Task3_3D_Foundation.unity` and press **Play**. This is the main Sprint A scene; `SampleScene` and other imported demo scenes are not the intended gameplay scene.

## Controls

| Input | Action |
| --- | --- |
| **W / A / S / D** | Move the player |
| **Mouse** | Look around / control the third-person camera |
| **Left mouse button** | Swing the sword |
| **Shift** | Sprint (Starter Assets controller) |
| **Space** | Jump (Starter Assets controller) |
| **4** | Spawn a zombie (factory command) |
| **6** | Recycle the most recently spawned zombie (pool command) |
| **7** | Switch to Main Menu (testing shortcut) |
| **8** | Switch to Playing (testing shortcut) |
| **9** | Switch to Game Over (testing shortcut) |

The numbered keys are developer/demo controls used to demonstrate the Command, Factory, Object Pool, and game-state systems.

## Gameplay and features

- Third-person movement and camera using Unity Starter Assets.
- Sword combat triggered by left mouse click, with a sword-swing animation, attack cooldown, and range/damage configured through a `SwordData` ScriptableObject.
- Camera shake when a sword attack hits a zombie.
- Zombie enemies that detect, chase, and attack the player using a state machine.
- Player health and an event-driven health display. Reaching zero health triggers Game Over.
- Main Menu, Playing, Game Over, and Victory states with corresponding UI panels.
- An exit trigger that changes the game state to Victory when the player reaches it.
- Enemy creation and reuse through a factory and object pool.

## Design patterns

| Pattern | Implementation | Purpose |
| --- | --- | --- |
| **Singleton** | `GameManager`, `AudioManager` | Shared access to game-wide systems |
| **Observer** | Game-state, player-health, and other C# events; UI listeners | Keep UI and game systems updated without constant polling |
| **State Machine** | `PlayerStateMachine`, `EnemyStateMachine` | Organize player and zombie behavior into states |
| **Factory** | `EnemyFactory` and `EnemyData` | Create configured zombie enemies |
| **Object Pool** | `EnemyFactory` and `PooledEnemy` | Reuse inactive enemies instead of always instantiating new ones |
| **Command** | `ICommand`, `CommandHandler`, and input command classes | Separate player input from the actions it invokes |

Key source folders: `Assets/Command/`, `Assets/Factory/`, `Assets/StateMachine/`, and `Assets/scripts/`.

## Testing / demonstration checklist

1. Launch the main scene or Windows build and start gameplay from the menu.
2. Move, sprint, jump, and rotate the camera.
3. Spawn a zombie with **4** and observe its chase/attack behavior.
4. Attack with **left mouse** to demonstrate the sword animation, zombie damage, and hit camera shake.
5. Let a zombie damage the player to demonstrate health UI updates and the Game Over condition.
6. In a new playthrough, reach the exit to trigger Victory.
7. Use **6** to demonstrate enemy recycling and **7–9** to demonstrate state commands.

## Tools and assets

- Unity 6 and C#
- Unity Starter Assets (third-person controller)
- Imported zombie, sword, and sword-animation assets used in the prototype

Third-party assets remain subject to their respective licenses and attribution requirements.

## AI assistance

ChatGPT was used to help interpret assignment requirements, explain design patterns, troubleshoot Unity/C# errors. The project was assembled and tested in the Unity Editor by me.

## Sprint A scope

This is a **playable prototype**, not a finished commercial game. The primary goal is to demonstrate a complete third-person gameplay loop and integration of the required softwa
re design patterns.
