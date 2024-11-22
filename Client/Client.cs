using System;
using System.Linq;
using System.Net.Sockets;

public class Client
{
    public static void Main(string[] args)
    {
        using (TcpClient client = new TcpClient("127.0.0.1", 8080))
        using (NetworkStream stream = client.GetStream())
        {
            Console.WriteLine("Connected to server.");

            // Вводим размер и элементы матрицы
            int[,] matrix = GetMatrixFromUser();

            // Отправляем матрицу на сервер
            SendMatrixToServer(stream, matrix);

            // Получаем результат от сервера
            double average = ReceiveResultFromServer(stream);

            Console.WriteLine($"The average of the matrix is: {average}");
        }
    }

    // Получение матрицы от пользователя
    public static int[,] GetMatrixFromUser()
    {
        Console.Write("Enter the number of rows: ");
        int rows = int.Parse(Console.ReadLine());

        Console.Write("Enter the number of columns: ");
        int columns = int.Parse(Console.ReadLine());

        int[,] matrix = new int[rows, columns];

        Console.WriteLine("Enter the elements of the matrix, row by row:");
        for (int i = 0; i < rows; i++)
        {
            string input = Console.ReadLine();
            int[] row = input.Split(' ').Select(int.Parse).ToArray();

            for (int j = 0; j < columns; j++)
            {
                matrix[i, j] = row[j];
            }
        }

        return matrix;
    }

    // Отправка матрицы на сервер
    public static void SendMatrixToServer(NetworkStream stream, int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int columns = matrix.GetLength(1);

        byte[] sizeBuffer = BitConverter.GetBytes(rows);
        stream.Write(sizeBuffer, 0, sizeBuffer.Length);

        sizeBuffer = BitConverter.GetBytes(columns);
        stream.Write(sizeBuffer, 0, sizeBuffer.Length);

        byte[] matrixBuffer = new byte[rows * columns * 4];
        Buffer.BlockCopy(matrix, 0, matrixBuffer, 0, matrixBuffer.Length);
        stream.Write(matrixBuffer, 0, matrixBuffer.Length);
    }

    // Получение результата от сервера
    public static double ReceiveResultFromServer(NetworkStream stream)
    {
        byte[] resultBuffer = new byte[8];
        stream.Read(resultBuffer, 0, resultBuffer.Length);
        return BitConverter.ToDouble(resultBuffer, 0);
    }
}
