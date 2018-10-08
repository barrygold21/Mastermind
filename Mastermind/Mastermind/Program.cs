using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind {
    public class GlobalVars {
        public static int turnNumber = 0;
        public static int boardX = 14;
        public static int boardY = 11;
        public static string[,] gameBoard = new string[boardY, boardX];
        public static int[] usersCodeGuess = new int[4];
        public static int[] codeToBreak = new int[4];
        public static readonly string redPeg = "R";
        public static readonly string whitePeg = "W";
        public static bool difficultyUnlocked = false;
        public static int difficultySelected = 6;
    }

    class Program {
        static void Main() {
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
            MainMenu();
        }

        public static void MainMenu() {
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
                if (!GlobalVars.difficultyUnlocked) {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("2. Change Difficulty ({0})", GlobalVars.difficultySelected);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else {
                    Console.WriteLine("2. Difficulty ({0})", GlobalVars.difficultySelected);
                }
                Console.WriteLine("3. How to play ");
                Console.WriteLine("4. Quit game   ");
                Console.WriteLine("-------------------------");
                if (debugMode) {
                    DebugModeInfo();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("All debug codes:");
                    Console.WriteLine("ACTIVATE_DEBUG");
                    Console.WriteLine("DEACTIVATE_DEBUG");
                    Console.WriteLine("PLAY_WITH_DEBUG");
                    Console.WriteLine("GENERATE_NEW_CODE");
                    Console.WriteLine("UNLOCK_DIFFICULTY");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else {
                    Console.WriteLine();
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Please enter your menu choice:");
                System.Threading.Thread.Sleep(100);
                string userInput = Console.ReadLine();
                bool isNumeric = int.TryParse(userInput, out menuChoice);
                if (isNumeric) {
                    if (menuChoice == 1) {
                        inputValid = true;
                        PlayGame(debugMode);
                    }
                    else if (menuChoice == 2) {
                        if (GlobalVars.difficultyUnlocked) {
                            SelectDifficulty();
                        }
                        else {
                            Console.Clear();
                            Console.WriteLine();
                            Console.Write("Difficulty selection not unlocked. Beat the game once to unlock it!");
                            Console.WriteLine();
                            System.Threading.Thread.Sleep(1500);
                        }
                    }
                    else if (menuChoice == 3) {
                        HowToPlay(debugMode);
                    }
                    else if (menuChoice == 4) {
                        Console.Clear();
                        Console.WriteLine("Mastermind will now close.");
                        System.Threading.Thread.Sleep(1000);
                        Environment.Exit(0);
                    }
                    else {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Error: ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Valid choice not entered. Please enter a number between 1 and 3.");
                        Console.WriteLine();
                        System.Threading.Thread.Sleep(1500);
                    }
                }
                else {
                    if (userInput == "ACTIVATE_DEBUG") {
                        debugMode = true;
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("Debug: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("ACTIVE");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.White;
                        System.Threading.Thread.Sleep(500);
                    }
                    else if (userInput == "DEACTIVATE_DEBUG") {
                        debugMode = false;
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("Debug: ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("DISABLED");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.White;
                        System.Threading.Thread.Sleep(500);
                    }
                    else if (userInput == "PLAY_WITH_DEBUG") {
                        inputValid = true;
                        debugMode = true;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("Debug: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("ACTIVE");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.White;
                        System.Threading.Thread.Sleep(500);
                        PlayGame(debugMode);
                    }
                    else if (userInput == "GENERATE_NEW_CODE") {
                        if (debugMode) {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("Debug: GENERATING NEW CODE");
                            Console.ForegroundColor = ConsoleColor.White;
                            System.Threading.Thread.Sleep(500);
                            GenerateCode();
                        }
                        else {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Error: ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("Please enter debug mode first.");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Since you know about the command, I've gone ahead and done that for you.");
                            Console.ForegroundColor = ConsoleColor.White;
                            debugMode = true;
                            System.Threading.Thread.Sleep(2000);
                        }
                    }
                    else if (userInput == "UNLOCK_DIFFICULTY") {
                        if (debugMode) {
                            GlobalVars.difficultyUnlocked = true;
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("Debug: DIFFICULTY UNLOCKED");
                            Console.ForegroundColor = ConsoleColor.White;
                            System.Threading.Thread.Sleep(500);
                        }
                        else {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Error: ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("Please enter debug mode first.");
                            Console.WriteLine();
                            Console.WriteLine("Since you know about the command, I've gone ahead and done that for you.");
                            debugMode = true;
                            System.Threading.Thread.Sleep(2000);
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
                }
            } while (!inputValid);
        }

        public static void SelectDifficulty() {
            string userInput;
            bool inputValid = false;
            do {
                Console.Clear();
                Console.WriteLine("Here you can select the difficulty for the game.");
                Console.WriteLine("This is only unlocked after you have beat the game once.");
                Console.WriteLine("You can set a difficulty of 6, 7, 8 or 9 numbers");
                Console.WriteLine("The default difficulty is 6 numbers.");
                Console.WriteLine("The current difficulty is {0}", GlobalVars.difficultySelected);
                Console.WriteLine("Please enter a new difficulty to use.");
                Console.WriteLine("If you do not wish to change the difficulty, enter the current difficulty.");
                Console.WriteLine();
                Console.WriteLine("New difficulty:");
                userInput = Console.ReadLine();
                bool isNumeric = int.TryParse(userInput, out GlobalVars.difficultySelected);
                if (isNumeric) {
                    if (GlobalVars.difficultySelected >= 6 && GlobalVars.difficultySelected <= 9) {
                        Console.WriteLine("Difficulty sucessfully set to {0}", GlobalVars.difficultySelected);
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
                        GlobalVars.difficultySelected = 6;
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

        public static void HowToPlay(bool debugMode) {
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
            DrawBoard();
            Console.WriteLine();
            Console.WriteLine("That's everything you need to know! Have fun!");
            Console.WriteLine();
            Console.WriteLine("Press any key to go back to the main menu.");
            Console.ReadKey();
        }

        public static void PlayGame(bool debugMode) {
            bool gameWon = false;
            bool gameOver = false;
            do {
                if (GlobalVars.turnNumber == 11) {
                    gameOver = true;
                    break;
                }
                Console.Clear();
                DrawBoard();
                if (debugMode) {
                    DebugModeInfo();
                }
                Console.WriteLine();
                EnterNextGuess(debugMode);
                System.Threading.Thread.Sleep(1000);
                AddGuessToBoard();
                gameWon = CalculateRedPegs();
                CalculateWhitePegs();
                GlobalVars.turnNumber++;
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

        public static void GenerateCode() {
            Random randomCodeNumber = new Random();
            for (int i = 0; i < 4; i++) {
                GlobalVars.codeToBreak[i] = randomCodeNumber.Next(1, GlobalVars.difficultySelected + 1);
            }
        }

        public static void InitialiseBoard() {
            for (int i = 0; i < GlobalVars.boardY; i++) {
                for (int j = 0; j < GlobalVars.boardX; j++) {
                    if (j == 4 || j == 9) {
                        GlobalVars.gameBoard[i, j] = "|";
                    }
                    else {
                        if (j > 9 || j < 4) {
                            GlobalVars.gameBoard[i, j] = " ";
                        }
                        else {
                            GlobalVars.gameBoard[i, j] = ".";
                        }
                    }
                }
            }
        }

        public static void DrawBoard() {
            int i = 0;
            foreach (var boardItem in GlobalVars.gameBoard) {
                if (boardItem.Equals("R")) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("{0} ", boardItem);
                    Console.ForegroundColor = ConsoleColor.White;
                    i++;
                    if (i == GlobalVars.boardX) {
                        Console.WriteLine();
                        i = 0;
                    }
                }
                else {
                    Console.Write("{0} ", boardItem);
                    i++;
                    if (i == GlobalVars.boardX) {
                        Console.WriteLine();
                        i = 0;
                    }
                }
            }
        }

        public static void DebugModeInfo() {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine("Answer: {0} {1} {2} {3} ", GlobalVars.codeToBreak[0], GlobalVars.codeToBreak[1], GlobalVars.codeToBreak[2], GlobalVars.codeToBreak[3]);
            Console.WriteLine();
            Console.WriteLine("Turn Number: {0} ({1})", GlobalVars.turnNumber + 1, GlobalVars.turnNumber);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        public static void EnterNextGuess(bool debugMode) {
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
                    isNumeric = int.TryParse(userInput, out GlobalVars.usersCodeGuess[i]);
                    if (isNumeric) {
                        if (GlobalVars.usersCodeGuess[i] >= 1 && GlobalVars.usersCodeGuess[i] <= GlobalVars.difficultySelected) {
                            inputValid[i] = true;
                        }
                        else {
                            for (int j = 0; j < 4; j++) {
                                inputValid[j] = false;
                            }
                            PleaseEnterValidNumber(debugMode);
                        }
                    }
                    else {
                        for (int j = 0; j < 4; j++) {
                            inputValid[j] = false;
                        }
                        PleaseEnterNumber(debugMode);
                    }
                    if (!inputValid[i]) {
                        break;
                    }
                }
            } while (!inputValid[0] && !inputValid[1] && !inputValid[2] && !inputValid[3]);
        }

        public static void PleaseEnterNumber(bool debugMode) {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Please enter a number.");
            Console.WriteLine();
            System.Threading.Thread.Sleep(1500);
            Console.Clear();
            DrawBoard();
            if (debugMode) {
                DebugModeInfo();
            }
        }

        public static void PleaseEnterValidNumber(bool debugMode) {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Valid digit not entered. Please enter a digit between 1 and {0}.", GlobalVars.difficultySelected);
            Console.WriteLine();
            System.Threading.Thread.Sleep(1500);
            Console.Clear();
            DrawBoard();
            if (debugMode) {
                DebugModeInfo();
            }
        }

        public static void AddGuessToBoard() {
            for (int i = 0; i < 4; i++) {
                GlobalVars.gameBoard[GlobalVars.turnNumber, i + 5] = Convert.ToString(GlobalVars.usersCodeGuess[i]);
            }
        }

        public static bool CalculateRedPegs() {
            bool gameWon = false;
            int redPegCount = 0;
            for (int i = 0; i < 4; i++) {
                if (GlobalVars.usersCodeGuess[i] == GlobalVars.codeToBreak[i]) {
                    GlobalVars.gameBoard[GlobalVars.turnNumber, redPegCount + 10] = GlobalVars.redPeg;
                    redPegCount++;
                }
            }
            if (redPegCount == 4) {
                gameWon = true;
            }
            return gameWon;
        }

        public static void CalculateWhitePegs() {
            int whitePegCount = 3;
            for (int i = 0; i < 4; i++) {
                if (GlobalVars.usersCodeGuess[i] == GlobalVars.codeToBreak[0]) {
                    GlobalVars.gameBoard[GlobalVars.turnNumber, whitePegCount] = GlobalVars.whitePeg;
                    whitePegCount--;
                }
                else if (GlobalVars.usersCodeGuess[i] == GlobalVars.codeToBreak[1]) {
                    if (i != 1) {
                        GlobalVars.gameBoard[GlobalVars.turnNumber, whitePegCount] = GlobalVars.whitePeg;
                        whitePegCount--;
                    }
                }
                else if (GlobalVars.usersCodeGuess[i] == GlobalVars.codeToBreak[2]) {
                    if (i != 2) {
                        GlobalVars.gameBoard[GlobalVars.turnNumber, whitePegCount] = GlobalVars.whitePeg;
                        whitePegCount--;
                    }
                }
                else if (GlobalVars.usersCodeGuess[i] == GlobalVars.codeToBreak[3]) {
                    if (i != 3) {
                        GlobalVars.gameBoard[GlobalVars.turnNumber, whitePegCount] = GlobalVars.whitePeg;
                        whitePegCount--;
                    }
                }
            }
        }

        public static void YouWin(bool debugMode) {
            bool userPlayAgain = false;
            do {
                Console.Clear();
                DrawBoard();
                if (debugMode) {
                    DebugModeInfo();
                }
                Console.WriteLine();
                Console.WriteLine("Congratulations! You win.");
                if (!GlobalVars.difficultyUnlocked) {
                    Console.WriteLine("Difficulty slection unlocked!");
                    Console.WriteLine("You can now change your difficulty in the main menu.");
                    GlobalVars.difficultyUnlocked = true;
                    Console.WriteLine("Keep in mind if you quit the game, you will have to unlock it again!");
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

        public static void YouLose(bool debugMode) {
            bool userPlayAgain = false;
            do {
                Console.Clear();
                DrawBoard();
                if (debugMode) {
                    DebugModeInfo();
                }
                Console.WriteLine();
                Console.WriteLine("Ouch! Seems like you didn't win this time around.");
                Console.WriteLine("Better luck next time!");
                Console.WriteLine("The code was:");
                Console.WriteLine("{0} {1} {2} {3}", GlobalVars.codeToBreak[0], GlobalVars.codeToBreak[1], GlobalVars.codeToBreak[2], GlobalVars.codeToBreak[3]);
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