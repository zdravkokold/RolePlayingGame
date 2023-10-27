## :space_invader: RPG Console Game

Welcome to the world of RPG Console Game, a text-based role-playing game where you'll embark on a quest to battle monsters in a 10x10 matrix world. This documentation provides an overview of the game's features, mechanics, and how to play.

### Table of Contents

1. [Introduction](#introduction)
2. [Game Features](#game-features)
3. [Character Selection](#character-selection)
4. [Gameplay](#gameplay)
5. [Entity Framework and Database](#entity-framework-and-database)

### 1. Introduction <a name="introduction"></a>

In the RPG Console Game, you'll step into a fantasy world filled with monsters, challenges, and adventures. Your mission is to choose your character, explore the game world, and engage in battles with menacing creatures. Are you ready to become a hero?

### 2. Game Features <a name="game-features"></a>

#### Character Customization

Choose your character from three distinct races, each with its own characteristics:

- **Mage:** Strength = 2; Agility = 1; Intelligence = 3; Range = 3; Symbol on the field: *
- **Warrior:** Strength = 3; Agility = 3; Intelligence = 0; Range = 1; Symbol on the field: @
- **Archer:** Strength = 2; Agility = 4; Intelligence = 0; Range = 2; Symbol on the field: #

Each character class is designed for different playstyles. Make your choice wisely!

#### Diverse Monsters

Prepare to face various monsters with random Strength, Agility, Intelligence, and a Range of 1. These creatures are represented by the symbol ◙ on the game field.

#### Four Game Screens

The game consists of four screens represented by an enum:

1. **MainMenu:** Displays a welcome message and proceeds to the CharacterSelect screen upon receiving input.
2. **CharacterSelect:** Allows you to choose your character and distribute points among your character's stats.
3. **InGame:** The main game screen where you move your character on a 10x10 matrix field, battle monsters, and make decisions.
4. **Exit:** The game exits when this screen is reached.

### 3. Character Selection <a name="character-selection"></a>

In the **CharacterSelect** screen, you have the following options:

- Choose your character type (Warrior, Archer, or Mage).
- Add up to 3 points to your character's stats (Strength, Agility, Intelligence).

Make your selections carefully, and keep an eye on your remaining points as you distribute them across your character's stats.

### 4. Gameplay <a name="gameplay"></a>

#### In-Game Actions

- Move your character on the 10x10 matrix field using the following keys:
  - W: Move up
  - S: Move down
  - A: Move left
  - D: Move right
  - E: Move diagonally up & right
  - X: Move diagonally down & right
  - Q: Move diagonally up & left
  - Z: Move diagonally down & left

#### Health and Mana

Keep an eye on your character's Health and Mana displayed at the top of the screen. Your character's stats and abilities are crucial for your success in battles.

#### Combat

Choose between "Move" or "Attack" in each turn. If you select "Attack," the game will display all available monsters within your range, along with their remaining health. You can then choose which monster to attack.

- If a monster is adjacent to you, it will always attack you.
- If a monster is not adjacent, it will move closer to you.

When you attack a monster, the damage you deal is subtracted from its health. Be strategic and make your moves wisely!

### 5. Entity Framework and Database <a name="entity-framework-and-database"></a>

The game uses Entity Framework to store character data and game logs. When you select your character and distribute points, your character's information is saved in the database, including the time of creation.
