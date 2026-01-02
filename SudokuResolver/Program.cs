//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2025
// </copyright>
// <Template>
// 	Version 2.0.2025.0, 28.4.2025
// </Template>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>04.05.2025 19:34:00</date>
//
// <summary>
// Konsolen Applikation mit Menü
// </summary>
//-----------------------------------------------------------------------

/* Imports from NET Framework */
using System;

namespace SudokuResolver
{
    public class Program
    {
        private static Dictionary<string, int> sudoku = new();

        private static void Main(string[] args)
        {
            ConsoleMenu.Add("1", "Start", () => MenuPoint1());
            ConsoleMenu.Add("X", "Beenden", () => ApplicationExit());

            do
            {
                _ = ConsoleMenu.SelectKey(2, 2);
            }
            while (true);
        }

        private static void ApplicationExit()
        {
            Environment.Exit(0);
        }

        private static void MenuPoint1()
        {
            Console.Clear();

            InitializeMatrix();
            LoadDemoValues();

            Console.WriteLine("Start Sudoku:\n");
            PrintSudoku();

            if (SolveSudoku())
            {
                Console.WriteLine("\nGelöstes Sudoku:\n");
                PrintSudoku();
            }
            else
            {
                Console.WriteLine("Keine Lösung gefunden.");
            }

            ConsoleMenu.Wait();
        }

        // 1. Matrix A1–I9 erzeugen
        private static void InitializeMatrix()
        {
            for (char row = 'A'; row <= 'I'; row++)
            {
                for (int col = 1; col <= 9; col++)
                {
                    sudoku[$"{row}{col}"] = 0;
                }
            }
        }

        // 2. Demo-Vorgabewerte setzen
        private static void LoadDemoValues()
        {
            Dictionary<string, int> demoValues = new()
            {
                { "A2", 7 },
                { "A3", 4 },
                { "A4", 8 },
                { "A6", 6 },

                { "B2", 9 },
                { "B5", 2 },
                { "B7", 3 },

                { "C5", 7 },
                { "C6", 3 },
                { "C8", 6 },
                { "C9", 5 },

                { "D1", 5 },
                { "D7", 2 },
                { "D8", 4 },

                { "E4", 3 },
                { "E6", 9 },
                { "E9", 1 },

                { "F2", 1 },
                { "F5", 4 },
                { "F8", 3 },

                { "G1", 2 },
                { "G2", 8 },
                { "G4", 1 },
                { "G6", 4 },

                { "H2", 6 },
                { "H4", 5 },
                { "H6", 7 },
                { "H7", 9 },
                { "H8", 2 },

                { "I1", 4 },
                { "I3", 9 },
                { "I5", 3 },
            };

            foreach (var entry in demoValues)
            {
                sudoku[entry.Key] = entry.Value % 10; // letzte Ziffer = Sudoku-Wert
            }
        }

        // 3. Backtracking-Solver
        private static bool SolveSudoku()
        {
            var emptyCell = sudoku.FirstOrDefault(c => c.Value == 0);
            if (emptyCell.Key == null)
                return true;

            for (int num = 1; num <= 9; num++)
            {
                if (IsValid(emptyCell.Key, num))
                {
                    sudoku[emptyCell.Key] = num;

                    if (SolveSudoku())
                        return true;

                    sudoku[emptyCell.Key] = 0;
                }
            }
            return false;
        }

        // Sudoku-Regeln prüfen
        private static bool IsValid(string cell, int value)
        {
            char row = cell[0];
            int col = int.Parse(cell[1].ToString());

            // Zeile
            if (sudoku.Any(c => c.Key[0] == row && c.Value == value))
                return false;

            // Spalte
            if (sudoku.Any(c => c.Key[1].ToString() == col.ToString() && c.Value == value))
                return false;

            // 3x3 Block
            int blockRow = (row - 'A') / 3;
            int blockCol = (col - 1) / 3;

            foreach (var c in sudoku)
            {
                int r = (c.Key[0] - 'A') / 3;
                int co = (int.Parse(c.Key[1].ToString()) - 1) / 3;

                if (r == blockRow && co == blockCol && c.Value == value)
                    return false;
            }

            return true;
        }

        // 4. Ausgabe
        private static void PrintSudoku()
        {
            for (char row = 'A'; row <= 'I'; row++)
            {
                for (int col = 1; col <= 9; col++)
                {
                    int val = sudoku[$"{row}{col}"];
                    Console.Write(val == 0 ? ". " : val + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
