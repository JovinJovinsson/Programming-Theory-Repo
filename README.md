# Programming Theory Repo
 Submission to close out Junior Programmer Pathway on Unity Learn. Key deliverables the project:
 * Use of Version Control (commits & branches)
 * Use of Scene Management (minimum Menu + Game)
 * Data Persistance (between scenes & between sessions)
 * OOP Principles (inheritance, polymorphism, abstraction, encapsulation)

The Project Design below will outline the approach used in each of them as layed out by the "Design Your Project" step of the Pathway module.
 
## Project Design: Monster Fighter
### Concept
Monster Fighter is an automatic (no user input) realtime-turn based (turns are based on the speed of the monsters) combat game where the player is pitted with their monster against progressively more difficult monsters. Initially the monsters are simple, however as the game progresses the monsters either become more numerous or more difficult as random modifiers are applied to them.

#### Game Flow
* **Main Screen**
  * Player inputs their name
  * Player selects their monster
* **Monster Build**
  * Player assigns stat points to their monster for 1st Level
  * Player starts the game when ready
* **Monster Fighter**
  * Player is pitted against random monsters over and over
  * Each monster yields experience, when the player receives enough experience they go back to the Monster Build screen to increase stats
* **Game Over Screen**
  * Displays the top 5 scores previously achieved detailing: Player Name, Challenge Level, Monster, Monster Stats
    * e.g. Phelx [8] Stone Monster [STR: 12 | DEF: 15 | SPD: 10]

## Project Deliverables
* **Inheritance**
  * Monster
    * Bat
    * Slime Rabbit
    * Mummy
    * Fantasy Bee
    * Mushroom
    * Molten Stone
  * All Monsters will have ```strength```, ```defense``` and ```speed``` properties
  * All Monsters will have a ```isPlayer``` property to determine if when defeated it is game over or next level
* **Polymorphism**
  * The ```Attack()``` function must be specified in each monster
* **Encapsulation**
  * The properties mentioned above will be encapsulated to ensure only valid modifications can be made to them
* **Abstraction**
  * ```CheckTurn()``` will check when the monsters turn to attack is ready
  * ```AdjustHitPoints(int change)``` will manage the change in hit points to the monster and trigger a win if defeated

*Note that this brief will change if anything is identified as missing from the criteria above, this is the minimum to have it functional however*

#### Attribution
All assets in game are from the [Unity Asset Store](https://assetstore.unity.com/).
Monsters are created by [amusedART](https://assetstore.unity.com/publishers/28394).
