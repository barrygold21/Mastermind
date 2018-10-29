using System;
using Mastermind.Properties;

namespace Mastermind {
    public class Variables {
        public int turnNumber = 0;
        public static readonly int BoardY = 14;
        public static readonly int BoardX = 11;
        public string[,] gameBoard = new string[BoardX, BoardY];
        public int[] usersCodeGuess = new int[4];
        public int[] codeToBreak = new int[4];
        public readonly string redPeg = "R";
        public readonly string whitePeg = "W";
        public bool difficultyUnlocked;
        public int difficultySelected = 6;
    }

    class Program : Variables {

        void Main() {
            Program p = new Program();
            difficultyUnlocked = IsDifficultyUnlocked();
            InitialiseBoard();
            GenerateCode();
            Console.Title = "Mastermind";
            bool flashControl = false;
            do {
                Console.Clear();
                if (flashControl) {
                    Console.WriteLine("      Mastermind      ");
                    Console.WriteLine("----------------------");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Press any key to start");
                    Console.ForegroundColor = ConsoleColor.White;
                    System.Threading.Thread.Sleep(600);
                }
                else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("      Mastermind      ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("----------------------");
                    Console.WriteLine("Press any key to start");
                    System.Threading.Thread.Sleep(600);
                }
                flashControl = !flashControl;
            } while (!Console.KeyAvailable);
            Console.ReadKey();
            p.MainMenu();
        }

        bool UnlockDifficulty() {
            Settings.Default["DifficultyUnlocked"] = true;
            Settings.Default.Save();
            return true;
        }

        bool LockDifficulty() {
            Settings.Default["DifficultyUnlocked"] = false;
            Settings.Default.Save();
            return false;
        }

        static bool IsDifficultyUnlocked() {
            switch (Settings.Default["DifficultyUnlocked"]) {
                case true:
                    return true;
                default:
                    return false;
            }
        }

        bool Error(int code) {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error: ");
            Console.ForegroundColor = ConsoleColor.White;
            switch (code) {
                case 1:
                    Console.Write("Valid choice not entered. Please enter a number between 1 and 4.");
                    break;
                case 2:
                    Console.Write("Please enter a number.");
                    break;
                case 3:
                    Console.Write("Please enter debug mode first.");
                    break;
                case 4:
                    Console.Write("Debug code not recognised. Debug mode disabled");
                    break;
                case 5:
                    Console.Write("Difficulty selection not unlocked. Beat the game once to unlock it!");
                    break;
                case 6:
                    Console.Write("Valid digit not entered. Please enter a digit between 1 and {0}.", difficultySelected);
                    break;
                default:
                    Console.Write("Error code not found");
                    break;
            }
            Console.WriteLine();
            if (code == 3) {
                Console.WriteLine("Since you know about the command, I've gone ahead and done that for you.");
                System.Threading.Thread.Sleep(500);
                Console.Clear();
                return true;
            }
            else {
                System.Threading.Thread.Sleep(1000);
                Console.Clear();
                return false;
            }
        }

        bool DebugSettings(string code, bool debugMode) {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Debug: ");
            switch (code) {
                case "debug.Activate":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("ACTIVE");
                    debugMode = true;
                    break;
                case "debug.Deactivate":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("DISABLED");
                    debugMode = true;
                    break;
                case "debug.NewCode":
                    if (debugMode) {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("GENERATING NEW CODE");
                        GenerateCode();
                    }
                    else { debugMode = Error(3); }
                    break;
                case "debug.UnlockDifficulty":
                    if (debugMode) {
                        difficultyUnlocked = UnlockDifficulty(); ;
                        Console.Write("DIFFICULTY UNLOCKED");
                    }
                    else { debugMode = Error(3); }
                    break;
                case "debug.LockDifficulty":
                    if (debugMode) {
                        difficultyUnlocked = LockDifficulty(); ;
                        Console.Write("DIFFICULTY LOCKED");
                    }
                    else { debugMode = Error(3); }
                    break;
                default:
                    Console.Clear();
                    debugMode = Error(4);
                    break;
            }
            System.Threading.Thread.Sleep(1500);
            return debugMode;
        }

        void MainMenu() {
            int menuChoice = 0;
            bool debugMode = false;
            bool inputValid = false;
            do {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Clear();
                Console.WriteLine("        Main Menu        ");
                Console.WriteLine();
                Console.WriteLine("-------------------------");
                Console.WriteLine("1. Play        ");
                if (!difficultyUnlocked) {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("2. Change Difficulty ({0})", difficultySelected);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else {
                    Console.WriteLine("2. Difficulty ({0})", difficultySelected);
                }
                Console.WriteLine("3. How to play ");
                Console.WriteLine("4. Quit game   ");
                Console.WriteLine("-------------------------");
                if (debugMode) {
                    DebugInfo();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("All debug codes:");
                    Console.WriteLine("debug.Activate");
                    Console.WriteLine("debug.Deactivate");
                    Console.WriteLine("debug.NewCode");
                    Console.WriteLine("debug.UnlockDifficulty");
                    Console.WriteLine("debug.LockDifficulty");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Please enter your menu choice:");
                System.Threading.Thread.Sleep(100);
                string userInput = Console.ReadLine();
                bool isNumeric = int.TryParse(userInput, out menuChoice);
                if (isNumeric) {
                    switch (menuChoice) {
                        case 1:
                            inputValid = true;
                            PlayGame(debugMode);
                            break;
                        case 2:
                            if (difficultyUnlocked) {
                                SelectDifficulty();
                            }
                            else { Error(5); }
                            break;
                        case 3:
                            HowToPlay(debugMode);
                            break;
                        case 4:
                            Console.Clear();
                            Console.WriteLine("Mastermind will now close.");
                            System.Threading.Thread.Sleep(1000);
                            Environment.Exit(0);
                            break;
                        default:
                            Error(1);
                            break;
                    }
                }
                else if (userInput.StartsWith("debug.")) {
                    debugMode = DebugSettings(userInput, debugMode);
                }
                else {
                    Error(2);
                }
            } while (!inputValid);
        }

        void SelectDifficulty() {
            string userInput;
            bool inputValid = false;
            do {
                Console.Clear();
                Console.WriteLine("Here you can select the difficulty for the game.");
                Console.WriteLine("This is only unlocked after you have beat the game once.");
                Console.WriteLine("You can set a difficulty of 6, 7, 8 or 9 numbers");
                Console.WriteLine("The default difficulty is 6 numbers.");
                Console.WriteLine("The current difficulty is {0}", difficultySelected);
                Console.WriteLine("Please enter a new difficulty to use.");
                Console.WriteLine("If you do not wish to change the difficulty, enter the current difficulty.");
                Console.WriteLine();
                Console.WriteLine("New difficulty:");
                userInput = Console.ReadLine();
                bool isNumeric = int.TryParse(userInput, out difficultySelected);
                if (isNumeric) {
                    if (difficultySelected >= 6 && difficultySelected <= 9) {
                        Console.WriteLine("Difficulty sucessfully set to {0}", difficultySelected);
                        inputValid = true;
                    }
                    else {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Error: ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Valid digit not entered.");
                        Console.WriteLine();
                        Console.WriteLine("Please enter a digit between 6 and 9. Selected difficulty has been set to 6.");
                        difficultySelected = 6;
                        System.Threading.Thread.Sleep(1500);
                    }
                }
                else {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Error: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Please enter a number.");
                    Console.WriteLine();
                    System.Threading.Thread.Sleep(1500);
                }
            } while (!inputValid);
        }

        void HowToPlay(bool debugMode) {
            Console.Clear();
            Console.WriteLine("How to play:");
            Console.WriteLine();
            Console.WriteLine("In this problem solving game, a random four-digit sequence is generated. ");
            Console.WriteLine("You are given twelve guesses to get the right sequence.");
            Console.WriteLine("After each guess, you will receive feedback on the board. ");
            Console.WriteLine("If you get a red peg, one of the digits you input was correct and in the same place.");
            Console.WriteLine("For each correct digit in the correct place, you get another red peg.");
            Console.WriteLine("If you get a white peg, one of the digits you input wasn't in the right place, but is in the code.");
            Console.WriteLine("For each digit in the code but not in the right place, you get one white peg.");
            Console.WriteLine("Red pegs are to the right of your guesses and white pegs are to the left.");
            Console.WriteLine("And of course, if you get four red pegs, you win!");
            Console.WriteLine();
            Console.WriteLine("This is what the board looks like at the start of the game:");
            Console.WriteLine();
            DrawBoard(debugMode);
            Console.WriteLine();
            Console.WriteLine("That's everything you need to know! Have fun!");
            Console.WriteLine();
            Console.WriteLine("Press any key to go back to the main menu.");
            Console.ReadKey();
        }

        void PlayGame(bool debugMode) {
            bool gameWon = false;
            bool gameOver = false;
            do {
                if (turnNumber == 11) {
                    gameOver = true;
                    break;
                }
                Console.Clear();
                DrawBoard(debugMode);
                Console.WriteLine();
                EnterNextGuess(debugMode);
                System.Threading.Thread.Sleep(1000);
                AddGuessToBoard();
                gameWon = CalculateRedPegs();
                CalculateWhitePegs();
                turnNumber++;
                if (gameWon) {
                    break;
                }
            } while (!gameWon | !gameOver);
            if (gameWon == true) {
                YouWin(debugMode);
            }
            else if (gameOver == true) {
                YouLose(debugMode);
            }
        }

        void GenerateCode() {
            Random randomCodeNumber = new Random();
            for (int i = 0; i < 4; i++) {
                codeToBreak[i] = randomCodeNumber.Next(1, difficultySelected + 1);
            }
        }

        void InitialiseBoard() {
            for (int i = 0; i < BoardX; i++) {
                for (int j = 0; j < BoardY; j++) {
                    if (j == 4 || j == 9) {
                        gameBoard[i, j] = "|";
                    }
                    else {
                        if (j > 9 || j < 4) {
                            gameBoard[i, j] = " ";
                        }
                        else {
                            gameBoard[i, j] = ".";
                        }
                    }
                }
            }
        }

        void DrawBoard(bool debugMode) {
            int i = 0;
            foreach (var boardItem in gameBoard) {
                if (boardItem.Equals("R")) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("{0} ", boardItem);
                    Console.ForegroundColor = ConsoleColor.White;
                    i++;
                    if (i == BoardY) {
                        Console.WriteLine();
                        i = 0;
                    }
                }
                else {
                    Console.Write("{0} ", boardItem);
                    i++;
                    if (i == BoardY) {
                        Console.WriteLine();
                        i = 0;
                    }
                }
            }
            if (debugMode) { DebugInfo(); }
        }

        void DebugInfo() {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine("Answer: {0} {1} {2} {3} ", codeToBreak[0], codeToBreak[1], codeToBreak[2], codeToBreak[3]);
            Console.WriteLine();
            Console.WriteLine("Turn Number: {0} ({1})", turnNumber + 1, turnNumber);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        void EnterNextGuess(bool debugMode) {
            bool[] inputValid = new bool[4];
            do {
                for (int i = 0; i < 4; i++) {
                    inputValid[i] = false;
                }
                string userInput;
                bool isNumeric = false;
                Console.WriteLine("Please enter each digit guess in a new line.");
                for (int i = 0; i < 4; i++) {
                    Console.Write("Digit {0}: ", i + 1);
                    userInput = Console.ReadLine();
                    isNumeric = int.TryParse(userInput, out usersCodeGuess[i]);
                    if (isNumeric) {
                        if (usersCodeGuess[i] >= 1 && usersCodeGuess[i] <= difficultySelected) {
                            inputValid[i] = true;
                        }
                        else {
                            for (int j = 0; j < 4; j++) {
                                inputValid[j] = false;
                            }
                            Error(6);
                            DrawBoard(debugMode);
                        }
                    }
                    else {
                        for (int j = 0; j < 4; j++) {
                            inputValid[j] = false;
                        }
                        Error(2);
                        DrawBoard(debugMode);
                    }
                    if (!inputValid[i]) {
                        break;
                    }
                }
            } while (!inputValid[0] && !inputValid[1] && !inputValid[2] && !inputValid[3]);
        }

        void AddGuessToBoard() {
            for (int i = 0; i < 4; i++) {
                gameBoard[turnNumber, i + 5] = Convert.ToString(usersCodeGuess[i]);
            }
        }

        bool CalculateRedPegs() {
            bool gameWon = false;
            int redPegCount = 0;
            for (int i = 0; i < 4; i++) {
                if (usersCodeGuess[i] == codeToBreak[i]) {
                    gameBoard[turnNumber, redPegCount + 10] = redPeg;
                    redPegCount++;
                }
            }
            if (redPegCount == 4) {
                gameWon = true;
            }
            return gameWon;
        }

        void CalculateWhitePegs() {
            int whitePegCount = 3;
            for (int i = 0; i < 4; i++) {
                if (usersCodeGuess[i] == codeToBreak[0]) {
                    gameBoard[turnNumber, whitePegCount] = whitePeg;
                    whitePegCount--;
                }
                else if (usersCodeGuess[i] == codeToBreak[1]) {
                    if (i != 1) {
                        gameBoard[turnNumber, whitePegCount] = whitePeg;
                        whitePegCount--;
                    }
                }
                else if (usersCodeGuess[i] == codeToBreak[2]) {
                    if (i != 2) {
                        gameBoard[turnNumber, whitePegCount] = whitePeg;
                        whitePegCount--;
                    }
                }
                else if (usersCodeGuess[i] == codeToBreak[3]) {
                    if (i != 3) {
                        gameBoard[turnNumber, whitePegCount] = whitePeg;
                        whitePegCount--;
                    }
                }
            }
        }

        void YouWin(bool debugMode) {
            bool userPlayAgain = false;
            do {
                Console.Clear();
                DrawBoard(debugMode);
                Console.WriteLine();
                Console.WriteLine("Congratulations! You win.");
                if (!difficultyUnlocked) {
                    UnlockDifficulty();
                    Console.Clear();
                    Console.WriteLine("Difficulty Modifications Unlocked!");
                    Console.WriteLine("You can now change the difficulty in the main menu.");
                    System.Threading.Thread.Sleep(1000);
                }
                Console.WriteLine();
                Console.WriteLine("Would you like to play again? (Y/N)");
                string playAgain = Console.ReadLine();
                playAgain = playAgain.ToUpper();
                Console.Clear();
                if (playAgain == "Y") {
                    userPlayAgain = true;
                    Main();
                }
                else if (playAgain == "N") {
                    userPlayAgain = false;
                    Console.WriteLine("Thank you for playing!");
                    System.Threading.Thread.Sleep(1000);
                    Environment.Exit(0);
                }
                else {
                    userPlayAgain = false;
                    Console.WriteLine("Please enter a valid character!");
                    System.Threading.Thread.Sleep(5000);
                }
            } while (userPlayAgain == false);
        }

        void YouLose(bool debugMode) {
            bool userPlayAgain = false;
            do {
                Console.Clear();
                DrawBoard(debugMode);
                Console.WriteLine();
                Console.WriteLine("Ouch! Seems like you didn't win this time around.");
                Console.WriteLine("Better luck next time!");
                Console.WriteLine("The code was:");
                Console.WriteLine("{0} {1} {2} {3}", codeToBreak[0], codeToBreak[1], codeToBreak[2], codeToBreak[3]);
                Console.WriteLine();
                Console.WriteLine("Would you like to play again? (Y/N)");
                string playAgain = Console.ReadLine();
                playAgain = playAgain.ToUpper();
                Console.Clear();
                if (playAgain == "Y") {
                    userPlayAgain = true;
                    Main();
                }
                else if (playAgain == "N") {
                    userPlayAgain = false;
                    Console.WriteLine("Thank you for playing!");
                    System.Threading.Thread.Sleep(1000);
                    Environment.Exit(0);
                }
                else {
                    userPlayAgain = false;
                    Console.WriteLine("Please enter a valid character!");
                    System.Threading.Thread.Sleep(1000);
                }
            } while (userPlayAgain == false);
        }
    }
}