using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class Server
{
    public static void Main(string[] args)
    {
        TcpListener server = new TcpListener(IPAddress.Any, 8080);
        server.Start();
        Console.WriteLine("Server started...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            Console.WriteLine("Client connected.");

            try
            {
                // Читаем матрицу от клиента
                int[,] matrix = ReadMatrixFromStream(stream);

                // Вычисляем среднее
                double average = CalculateAverage(matrix);

                // Отправляем результат обратно клиенту
                SendResultToClient(stream, average);

                Console.WriteLine($"Average calculated and sent: {average}");
            }
            finally
            {
                client.Close();
            }
        }
    }

    // Чтение матрицы из потока
    public static int[,] ReadMatrixFromStream(NetworkStream stream)
    {
        byte[] sizeBuffer = new byte[4];
        stream.Read(sizeBuffer, 0, sizeBuffer.Length);
        int rows = BitConverter.ToInt32(sizeBuffer, 0);

        stream.Read(sizeBuffer, 0, sizeBuffer.Length);
        int columns = BitConverter.ToInt32(sizeBuffer, 0);

        byte[] matrixBuffer = new byte[rows * columns * 4];
        stream.Read(matrixBuffer, 0, matrixBuffer.Length);

        int[,] matrix = new int[rows, columns];
        Buffer.BlockCopy(matrixBuffer, 0, matrix, 0, matrixBuffer.Length);
        return matrix;
    }

    // Вычисление среднего арифметического
    public static double CalculateAverage(int[,] matrix)
    {
        double sum = 0;
        int count = 0;

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                sum += matrix[i, j];
                count++;
            }
        }

        return sum / count;
    }

    // Отправка результата клиенту
    public static void SendResultToClient(NetworkStream stream, double result)
    {
        byte[] resultBuffer = BitConverter.GetBytes(result);
        stream.Write(resultBuffer, 0, resultBuffer.Length);
    }
}
