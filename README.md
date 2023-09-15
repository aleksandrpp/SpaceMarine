## Space Marine
### Unity game architecture sample

![SpaceMarine](Media/SpaceMarine.gif)

The most convenient OOP game architecture sample that I can made with the lowest lines of code with no threading tricks.

```mermaid
---
title: Base Architecture Diagram
---
erDiagram
    
    Entry ||--o{ World : Create
    Entry ||--o{ Maze : Create
    Entry ||--o{ Grid : Create
    Entry ||--o{ Pathfinding : Create
    Pathfinding ||--|| Grid : Use
    Entry ||--|{ UI : Use
    Entry ||--|{ Input : Read
    Entry ||--|| UserData : Load
    UI ||--|| World : Use
    World ||--|{ Tag : Fetch
    Input ||--o{ Actor : Use
    World ||--o{ Actor : Handle
    World }|--|| UserData : Save
    Actor ||--|{ Pathfinding : Use
    Actor ||--|{ Weapon : Shoot
    Weapon ||--|{ Bullet : Launch
    Actor }|--|{ Grid : Occupy
    Maze }|--|{ Grid : Occupy
   
```

#### In use
 - OOP architecture with actor-based entities.
 - Pure DI, the game uses constructor injection and bind methods to inject dependencies into objects. This ensures that there are no static references.
 - AStar grid pathfinding algorithm with a priority queue and runtime generation. Additionally, the game updates tile occupation with dynamic cost.
 - Behaviour Tree AI system. 
 - Weapons with perks, bullet pools, and simple verlet ballistics.
 - Simple save system.
 - Full-screen sharpen shader, as URP render feature.
 - Widgets that poll data from actors.

 #### Behaviour tree

```mermaid
---
title: Enemy AI
---
stateDiagram-v2
    
    state "&&" as n0
    [*] --> n0 : Sequence
    n0 --> Alive
    n0 --> Reload
    n0 --> CheckHero

    state "||" as n1
    n0 --> n1 : Selector
    n1 --> EyeContact*1
    n1 --> ChaseHero*1

    state "||" as n2
    n0 --> n2 : Selector
    n2 --> CheckDistance
    n2 --> ChaseHero*2

    n0 --> EyeContact*2
    n0 --> Attack
    
```

### Gameplay
- The game features a procedurally generated maze where players must survive by battling enemies.
- Enemies spawn from outposts placed on the map, and victory is achieved by destroying all of them.
- Points are awarded for killing enemies and taking down outposts, with health packs dropped by enemies to aid in healing.
- Player progress is saved as best score.

### Possible optimization points
- Handle bullets and actors transforms via TransformArrayAccess.
- Custom physics system.
- Health bars and trackers via GPU.

_Unity 2022.3.3_
<br>

https://github.com/aleksandrpp/SpaceMarine