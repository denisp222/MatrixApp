using NUnit.Framework;

[TestFixture]
public class Tests
{
    [Test]
    public void CalculateAverage_ValidMatrix_ReturnsCorrectAverage()
    {
        int[,] matrix = {
            {1, 2, 3},
            {4, 5, 6},
            {7, 8, 9}
        };

        double result = Server.CalculateAverage(matrix);

        Assert.AreEqual(5, result);
    }
    [Test]
    public void CalculateAverage_SingleElementMatrix_ReturnsElementValue()
    {
	int[,] matrix = { { 5 } };

    	double result = Server.CalculateAverage(matrix);

        Assert.AreEqual(5, result);
    }
    [Test]
    public void CalculateAverage_MatrixWithNegativeNumbers_ReturnsCorrectAverage()
    {
        int[,] matrix = {
            {-1, -2, -3},
            {-4, -5, -6},
            {-7, -8, -9}
    	};

    	double result = Server.CalculateAverage(matrix);

    	Assert.AreEqual(-5, result);
    }
    [Test]
    public void CalculateAverage_MatrixWithZeros_ReturnsCorrectAverage()
    {
    	int[,] matrix = {
            {0, 0, 0},
            {0, 0, 0},
            {0, 0, 0}
        };

        double result = Server.CalculateAverage(matrix);

        Assert.AreEqual(0, result);
    }
    [Test]
    public void CalculateAverage_EmptyMatrix_ReturnsZero()
    {
    	int[,] matrix = { };

   	double result = Server.CalculateAverage(matrix);

        Assert.AreEqual(0, result);
    }
}
