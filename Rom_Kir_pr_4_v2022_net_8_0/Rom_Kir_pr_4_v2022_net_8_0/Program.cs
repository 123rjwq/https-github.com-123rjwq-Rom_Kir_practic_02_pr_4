using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Флаг для управления повторением выбора операций
        bool repeat = true;
        string kingPos = string.Empty; // Координаты ферзя
        string piecePos = string.Empty; // Координаты другой фигуры

        // Основной цикл программы, продолжается до тех пор, пока пользователь не выберет выход
        while (repeat)
        {
            // Вывод меню выбора действий
            Console.WriteLine("Выберите одно из действий:");
            Console.WriteLine("1. Разместить фигуры на шахматной доске");
            Console.WriteLine("2. Определить, бьет ли ферзь фигуру");
            Console.WriteLine("3. Выйти из программы");
            Console.Write("Ваш выбор: ");

            // Проверка на ввод числа
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Некорректный ввод. Попробуйте еще раз.");
                Console.WriteLine();
                continue;
            }

            // Выполнение выбранного действия
            switch (choice)
            {
                case 1:
                    // Размещение фигур на шахматной доске
                    SetupBoard(out kingPos, out piecePos);
                    break;
                case 2:
                    // Проверка возможности побить фигуру королем
                    if (kingPos != string.Empty && piecePos != string.Empty)
                    {
                        CheckCapture(kingPos, piecePos);
                    }
                    else
                    {
                        Console.WriteLine("Фигуры на шахматной доске не размещены");
                    }
                    break;
                case 3:
                    // Завершение программы
                    repeat = false;
                    break;
                default:
                    // Обработка неверного выбора
                    Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                    break;
            }

            // Переход на новую строку для удобства чтения
            Console.WriteLine();
        }
    }

    // Метод для размещения фигур на доске
    static void SetupBoard(out string kingPos, out string piecePos)
    {
        // Инициализация доски
        char[,] board = new char[8, 8];
        InitializeBoard(board);

        // Запрос координат для размещения короля и фигуры
        Console.WriteLine("Введите координаты ферзя и фигуры (в формате x1y1 x2y2):");
        string input = Console.ReadLine();

        // Разделение введённых координат
        string[] coordinates = input.Split(' ');

        // Проверка корректности введённых координат
        if (coordinates.Length != 2 || coordinates[0] == coordinates[1] || !ValidateCoordinates(coordinates[0]) || !ValidateCoordinates(coordinates[1]))
        {
            Console.WriteLine("Введены некорректные координаты");
            kingPos = string.Empty;
            piecePos = string.Empty;
            return;
        }

        // Размещение фигур на доске
        kingPos = coordinates[0];
        piecePos = coordinates[1];
        PlacePieces(board, kingPos, piecePos);

        // Вывод доски
        DrawBoard(board);
    }

    // Метод для проверки, может ли король побить фигуру
    static void CheckCapture(string kingPos, string piecePos)
    {
        // Вывод информации о проверке
        Console.WriteLine("Операция  2: Определение, бьет ли ферзь фигуру");
        Console.WriteLine();

        // Вычисление координат короля и фигуры
        int kingX = kingPos[0] - 'a';
        int kingY = kingPos[1] - '1';

        int pieceX = piecePos[0] - 'a';
        int pieceY = piecePos[1] - '1';

        // Проверка, может ли король побить фигуру
        if (Math.Abs(kingX - pieceX) <= 1 && Math.Abs(kingY - pieceY) <= 1)
        {
            Console.WriteLine("Король сможет побить фигуру за  1 ход");
        }
        else
        {
            Console.WriteLine("Король не может побить фигуру за  1 ход");
        }
    }

    // Метод для инициализации доски
    static void InitializeBoard(char[,] board)
    {
        // Заполнение доски пустыми клетками
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                board[row, col] = '-';
            }
        }
    }

    // Метод для размещения фигур на доске
    static void PlacePieces(char[,] board, string kingPos, string piecePos)
    {
        // Вычисление координат короля и фигуры
        int kingX = kingPos[0] - 'a';
        int kingY = kingPos[1] - '1';

        int pieceX = piecePos[0] - 'a';
        int pieceY = piecePos[1] - '1';

        // Размещение короля и фигуры на доске
        MoveKing(board, kingX, kingY);
        PlacePiece(board, pieceX, pieceY, 'F');
    }

    // Метод для размещения короля на доске
    static void MoveKing(char[,] board, int x, int y)
    {
        // Проверка корректности координат
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            board[y, x] = 'K'; // Размещаем короля на выбранных координатах
        }
    }

    // Метод для размещения фигуры на доске
    static void PlacePiece(char[,] board, int x, int y, char piece)
    {
        // Проверка корректности координат
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            board[y, x] = piece; // Размещаем фигуру на выбранных координатах
        }
    }

    // Метод для вывода доски
    static void DrawBoard(char[,] board)
    {
        // Вывод заголовка доски
        Console.WriteLine("   a b c d e f g h");

        // Вывод доски в обратном порядке (с верхней стороны)
        for (int row = 7; row >= 0; row--)
        {
            Console.Write($"{row + 1} ");

            for (int col = 0; col < 8; col++)
            {
                Console.Write(board[row, col] + " ");
            }

            Console.WriteLine();
        }
    }

    // Метод для проверки корректности введённых координат
    static bool ValidateCoordinates(string coordinate)
    {
        // Проверка длины строки координат
        if (coordinate.Length != 2)
        {
            return false;
        }

        // Проверка диапазона символов координат
        char file = coordinate[0];
        char rank = coordinate[1];

        if (file < 'a' || file > 'h' || rank < '1' || rank > '8')
        {
            return false;
        }

        return true;
    }
}
//1