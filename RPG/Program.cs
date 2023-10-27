using RPG;
using System;
using System.Linq;
using RPG.Characters;
using System.Numerics;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Run();
    }
}

public class Game
{
    private char[,] gameField;
    private int fieldWidth = 10;
    private int fieldHeight = 10;
    private Character player;
    private GameDbContext dbContext;
    private GameScreen currentScreen;
    public List<Monster> monsters;

    public Game()
    {
        gameField = new char[fieldWidth, fieldHeight];
        dbContext = new GameDbContext();
        currentScreen = GameScreen.MainMenu;
        monsters = new List<Monster>();
    }

    public void MainMenuScreen()
    {
        Console.Clear();
        Console.WriteLine("Welcome!");
        Console.WriteLine("Press any key to play.");
        Console.ReadKey();
        currentScreen = GameScreen.CharacterSelect;
    }

    public void CharacterSelectScreen()
    {
        Console.Clear();
        Console.WriteLine("Choose character type:");
        Console.WriteLine("Options:");
        Console.WriteLine("1) Warrior");
        Console.WriteLine("2) Archer");
        Console.WriteLine("3) Mage");
        Console.Write("Your pick: ");
        int raceChoice = int.Parse(Console.ReadLine());

        switch (raceChoice)
        {
            case 1:
                player = new Warrior();
                break;
            case 2:
                player = new Archer();
                break;
            case 3:
                player = new Mage();
                break;
            default:
                Console.WriteLine("Invalid choice. Press any key to choose again.");
                Console.ReadKey();
                return;
        }

        Console.WriteLine("Would you like to buff up your stats before starting? (Limit: 3 points total)");
        Console.Write("Response (Y\\N): ");
        string buffChoice = Console.ReadLine().ToUpper();

        if (buffChoice == "Y")
        {
            Console.WriteLine("Remaining Points: " + player.RemainingPoints);

            Console.Write("Add to Strength: ");
            int addedStrength = int.Parse(Console.ReadLine());
            int addedAgility = 0;
            int addedIntelligence = 0;

            if (!(player.RemainingPoints - addedStrength <= 0))
            {
                Console.Write("Add to Agility: ");
                addedAgility = int.Parse(Console.ReadLine());

                if (!(player.RemainingPoints - (addedStrength + addedAgility) <= 0))
                {
                    Console.Write("Add to Intelligence: ");
                    addedIntelligence = int.Parse(Console.ReadLine());
                }
            }
            player.AddPoints(addedStrength, addedAgility, addedIntelligence);
        }
        else if (buffChoice != "N")
        {
            Console.WriteLine("Invalid choice. No points will be added. Press any key to play.");
            Console.ReadKey();
        }

        using (dbContext)
        {
            player.Setup();
            player.TimeOfCreation = DateTime.Now;
            dbContext.Characters.Add(player);
            dbContext.SaveChanges();
        }

        currentScreen = GameScreen.InGame;
    }

    public void InGameScreen()
    {
        Console.Clear();
        Console.WriteLine($"Health: {player.Health} Mana: {player.Mana}");
        Console.WriteLine();

        DrawGameField();

        Console.SetCursorPosition(0, 13);
        Console.WriteLine("Choose action:");
        Console.WriteLine("1) Attack");
        Console.WriteLine("2) Move");
        Console.Write("Your choice: ");
        var choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case 1:
                AttackMonsters();
                break;
            case 2:
                var key = Console.ReadKey().Key;
                int range = player.Range;
                switch (key)
                {
                    case ConsoleKey.W:
                        MovePlayer(0, -range); // Move up
                        break;
                    case ConsoleKey.S:
                        MovePlayer(0, range); // Move down
                        break;
                    case ConsoleKey.A:
                        MovePlayer(-range, 0); // Move left
                        break;
                    case ConsoleKey.D:
                        MovePlayer(range, 0); // Move right
                        break;
                    case ConsoleKey.E:
                        MovePlayer(range, -range); // Move diagonally up & right
                        break;
                    case ConsoleKey.X:
                        MovePlayer(range, range); // Move diagonally down & right
                        break;
                    case ConsoleKey.Q:
                        MovePlayer(-range, -range); // Move diagonally up & left
                        break;
                    case ConsoleKey.Z:
                        MovePlayer(-range, range); // Move diagonally down & left
                        break;
                }
                break;
            default:
                Console.WriteLine("Invalid choice. Press any key to choose again.");
                Console.ReadKey();
                break;
        }

        if (IsGameOver)
        {
            currentScreen = GameScreen.MainMenu;
        }
    }

    private void DrawGameField()
    {        
        char emptyCell = '▒';
        char monsterCell = 'O';

        for (int y = 2; y < fieldHeight + 2; y++)
        {
            for (int x = 0; x < fieldWidth; x++)
            {
                char cellContent = emptyCell;

                foreach (var monster in monsters)
                {
                    if (monster.PositionX == x && monster.PositionY == y)
                    {
                        cellContent = monsterCell;
                        break;
                    }
                }
                Console.SetCursorPosition(x, y);
                Console.Write(cellContent);
            }
        }
        Console.SetCursorPosition(player.PositionX, player.PositionY + 2);
        Console.Write(player.Symbol);
    }

    private void MovePlayer(int deltaX, int deltaY)
    {
        int newX = player.PositionX + deltaX;
        int newY = player.PositionY + deltaY;

        if (IsPositionValid(newX, newY))
        {
            Console.SetCursorPosition(player.PositionX, player.PositionY);
            Console.Write('▒'); 

            player.PositionX = newX;
            player.PositionY = newY;

            Console.SetCursorPosition(player.PositionX, player.PositionY);
            Console.Write(player.Symbol);
        }
        GenerateRandomMonster();
        MoveMonsters();
    }

    private bool IsPositionValid(int x, int y)
    {
        return x >= 0 && x < 10 && y >= 2 && y < 12;
    }

    private void AttackMonsters()
    {
        List<Monster> monstersInRange = monsters.Where(IsInPlayerAttackRange).ToList();
        if (monstersInRange.Count > 0)
        {
            Console.WriteLine(monstersInRange.Count);
            for (int i = 0; i < monstersInRange.Count; i++)
            {
                Console.WriteLine($"{i}) Target with remaining blood {monstersInRange[i].Health}");
            }
            Console.Write("Which one to attack: ");

            int selectedMonsterIndex;
            if (int.TryParse(Console.ReadLine(), out selectedMonsterIndex) && selectedMonsterIndex >= 0 && selectedMonsterIndex < monstersInRange.Count)
            {
                Monster targetMonster = monstersInRange[selectedMonsterIndex];

                targetMonster.TakeDamage(player.Damage);

                if (targetMonster.IsDead)
                {
                    monsters.Remove(targetMonster);
                }
            }
            else
            {
                Console.WriteLine("Invalid selection. Press any key to choose again.");
                Console.ReadKey();
            }
        }
        else
        {
            Console.WriteLine("No available targets in your range");
            Console.WriteLine("Press any key to choose again.");
            Console.ReadKey();
        }
    }

    private void GenerateRandomMonster()
    {
        Random random = new Random(); 
        int randomX = random.Next(0, 10);
        int randomY = random.Next(2, 13);

        var monster = new Monster(randomX, randomY);
        monster.Setup();
        monster.TimeOfCreation = DateTime.Now;
        monsters.Add(monster);
    }

    private void MoveMonsters()
    {
        foreach (var monster in monsters)
        {
            int distanceX = Math.Abs(player.PositionX - monster.PositionX);
            int distanceY = Math.Abs(player.PositionY - monster.PositionY);

            if (distanceX <= 1 && distanceY <= 1)
            {
                player.TakeDamage(monster.Damage);

                if (IsGameOver)
                {
                    Console.Clear();
                    Console.WriteLine("Game over! You have been defeated by the monsters.");
                    Console.WriteLine("Press any key to exit the game.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            else
            {
                int moveDirectionX = player.PositionX - monster.PositionX;
                int moveDirectionY = player.PositionY - monster.PositionY;

                int absDirectionX = Math.Abs(moveDirectionX);
                int absDirectionY = Math.Abs(moveDirectionY);

                if (absDirectionX > absDirectionY)
                {
                    if (moveDirectionX > 0)
                    {
                        monster.PositionX++;
                    }
                    else
                    {
                        monster.PositionX--;
                    }
                }
                else
                {
                    if (moveDirectionY > 0)
                    {
                        monster.PositionY++;
                    }
                    else
                    {
                        monster.PositionY--;
                    }
                }
            }
        }
    }

    private bool IsGameOver => player.IsDead;

    private bool IsInPlayerAttackRange(Monster monster)
    {
        int distanceX = Math.Abs(player.PositionX - monster.PositionX);
        int distanceY = Math.Abs(player.PositionY - monster.PositionY);

        return distanceX <= player.Range && distanceY <= player.Range;
    }

    public void Run()
    {
        while (currentScreen != GameScreen.Exit)
        {
            switch (currentScreen)
            {
                case GameScreen.MainMenu:
                    MainMenuScreen();
                    break;
                case GameScreen.CharacterSelect:
                    CharacterSelectScreen();
                    break;
                case GameScreen.InGame:
                    InGameScreen();
                    break;
            }
        }
        if (currentScreen == GameScreen.Exit)
        {
            Console.Clear();
            Console.WriteLine("Exiting the game.");
        }
    }
}