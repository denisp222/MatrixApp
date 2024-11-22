using NUnit.Framework;

[TestFixture]
public class Tests
{
    [Test]
    public void TestCalculateAverage()
    {
        var matrix = new int[,] {
            {1, 2, 3},
            {4, 5, 6},
            {7, 8, 9}
        };
        var result = CalculateAverage(matrix);
        Assert.AreEqual(5, result);
    }
}
