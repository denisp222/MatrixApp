using System;
using System.IO;
using System.Net.Sockets;
using System.Linq;

class Client
{
    static void Main(string[] args)
    {
        // Подключаемся к серверу
        using (TcpClient client = new TcpClient("127.0.0.1", 8080))
        using (NetworkStream stream = client.GetStream())
        {
            Console.WriteLine("Connected to server.");

            // Вводим размер матрицы
            Console.Write("Enter the number of rows: ");
            int rows = int.Parse(Console.ReadLine());
            Console.Write("Enter the number of columns: ");
            int columns = int.Parse(Console.ReadLine());

            // Отправляем размерность матрицы
            byte[] sizeBuffer = BitConverter.GetBytes(rows);
            stream.Write(sizeBuffer, 0, sizeBuffer.Length);
            sizeBuffer = BitConverter.GetBytes(columns);
            stream.Write(sizeBuffer, 0, sizeBuffer.Length);

            // Вводим элементы матрицы построчно
            int[,] matrix = new int[rows, columns];
            Console.WriteLine("Enter the elements of the matrix, row by row:");
            for (int i = 0; i < rows; i++)
            {
                string input = Console.ReadLine();
                // Преобразуем строку в массив целых чисел
                var row = input.Split(' ').Select(int.Parse).ToArray();
                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = row[j];
                }
            }

            // Отправляем матрицу на сервер
            byte[] matrixBuffer = new byte[rows * columns * 4];
            Buffer.BlockCopy(matrix, 0, matrixBuffer, 0, matrixBuffer.Length);
            stream.Write(matrixBuffer, 0, matrixBuffer.Length);

            // Получаем результат от сервера
            byte[] resultBuffer = new byte[8]; // Результат - double
            stream.Read(resultBuffer, 0, resultBuffer.Length);
            double average = BitConverter.ToDouble(resultBuffer, 0);

            Console.WriteLine($"The average of the matrix is: {average}");
        }
    }
}
