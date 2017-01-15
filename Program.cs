using System;
using System.Collections.Generic;

namespace RobotController
{
    class Program
    {
        private const int GridSize = 10;
        private static Robot _robot;
        private static bool[,] _obstacles;

        static void Main(string[] args)
        {
            Console.WriteLine("=== Robot Controller ===\n");

            InitializeGrid();
            _robot = new Robot(0, 0, Direction.North);

            Console.WriteLine("Commands: F(orward), L(eft), R(ight), S(tatus), Q(uit)\n");

            ShowGrid();

            while (true)
            {
                Console.Write("\nCommand: ");
                var input = Console.ReadLine()?.ToUpper();

                if (string.IsNullOrEmpty(input)) continue;

                if (input == "Q") break;

                ProcessCommand(input);
                ShowGrid();
            }

            Console.WriteLine("\nGoodbye!");
        }

        private static void InitializeGrid()
        {
            _obstacles = new bool[GridSize, GridSize];
            var random = new Random();

            for (int i = 0; i < 5; i++)
            {
                int x = random.Next(GridSize);
                int y = random.Next(GridSize);
                if (x != 0 || y != 0)
                {
                    _obstacles[x, y] = true;
                }
            }
        }

        private static void ProcessCommand(string command)
        {
            switch (command)
            {
                case "F":
                    MoveForward();
                    break;
                case "L":
                    _robot.TurnLeft();
                    Console.WriteLine($"Turned left. Now facing {_robot.Facing}");
                    break;
                case "R":
                    _robot.TurnRight();
                    Console.WriteLine($"Turned right. Now facing {_robot.Facing}");
                    break;
                case "S":
                    ShowStatus();
                    break;
                default:
                    Console.WriteLine("Invalid command!");
                    break;
            }
        }

        private static void MoveForward()
        {
            int newX = _robot.X;
            int newY = _robot.Y;

            switch (_robot.Facing)
            {
                case Direction.North:
                    newY--;
                    break;
                case Direction.South:
                    newY++;
                    break;
                case Direction.East:
                    newX++;
                    break;
                case Direction.West:
                    newX--;
                    break;
            }

            if (newX < 0 || newX >= GridSize || newY < 0 || newY >= GridSize)
            {
                Console.WriteLine("Cannot move - boundary!");
                return;
            }

            if (_obstacles[newX, newY])
            {
                Console.WriteLine("Cannot move - obstacle!");
                return;
            }

            _robot.X = newX;
            _robot.Y = newY;
            Console.WriteLine($"Moved to ({newX}, {newY})");
        }

        private static void ShowStatus()
        {
            Console.WriteLine($"\nRobot Status:");
            Console.WriteLine($"  Position: ({_robot.X}, {_robot.Y})");
            Console.WriteLine($"  Facing: {_robot.Facing}");
        }

        private static void ShowGrid()
        {
            Console.WriteLine("\nGrid:");
            Console.Write("  ");
            for (int i = 0; i < GridSize; i++)
                Console.Write(i + " ");
            Console.WriteLine();

            for (int y = 0; y < GridSize; y++)
            {
                Console.Write(y + " ");
                for (int x = 0; x < GridSize; x++)
                {
                    if (_robot.X == x && _robot.Y == y)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(GetRobotSymbol() + " ");
                        Console.ResetColor();
                    }
                    else if (_obstacles[x, y])
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write("· ");
                    }
                }
                Console.WriteLine();
            }
        }

        private static char GetRobotSymbol()
        {
            return _robot.Facing switch
            {
                Direction.North => '↑',
                Direction.South => '↓',
                Direction.East => '→',
                Direction.West => '←',
                _ => 'R'
            };
        }
    }

    enum Direction
    {
        North,
        East,
        South,
        West
    }

    class Robot
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Direction Facing { get; set; }

        public Robot(int x, int y, Direction facing)
        {
            X = x;
            Y = y;
            Facing = facing;
        }

        public void TurnLeft()
        {
            Facing = Facing switch
            {
                Direction.North => Direction.West,
                Direction.West => Direction.South,
                Direction.South => Direction.East,
                Direction.East => Direction.North,
                _ => Facing
            };
        }

        public void TurnRight()
        {
            Facing = Facing switch
            {
                Direction.North => Direction.East,
                Direction.East => Direction.South,
                Direction.South => Direction.West,
                Direction.West => Direction.North,
                _ => Facing
            };
        }
    }
}
