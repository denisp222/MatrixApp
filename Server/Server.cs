using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

class Server
{
    static void Main(string[] args)
    {
        TcpListener server = new TcpListener(IPAddress.Any, 8080);
        server.Start();
        Console.WriteLine("Server started...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            Console.WriteLine("Client connected.");

            // Читаем размерность матрицы
            byte[] sizeBuffer = new byte[4];
            stream.Read(sizeBuffer, 0, sizeBuffer.Length);
            int rows = BitConverter.ToInt32(sizeBuffer, 0);
            stream.Read(sizeBuffer, 0, sizeBuffer.Length);
            int columns = BitConverter.ToInt32(sizeBuffer, 0);

            // Читаем саму матрицу
            byte[] matrixBuffer = new byte[rows * columns * 4];
            stream.Read(matrixBuffer, 0, matrixBuffer.Length);

            int[,] matrix = new int[rows, columns];
            Buffer.BlockCopy(matrixBuffer, 0, matrix, 0, matrixBuffer.Length);

            // Вычисляем среднее арифметическое
            double sum = 0;
            int count = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    sum += matrix[i, j];
                    count++;
                }
            }

            double average = sum / count;

            // Отправляем результат обратно клиенту
            byte[] resultBuffer = BitConverter.GetBytes(average);
            stream.Write(resultBuffer, 0, resultBuffer.Length);

            Console.WriteLine($"Average calculated: {average}");

            client.Close();
        }
    }
}
