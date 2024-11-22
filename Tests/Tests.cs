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
}
