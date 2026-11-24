namespace CalculatorAPI.NUnit.Test;

public class TestTests
{
    public static IEnumerable<TestCaseData> YieldTests()
    {
        var firstTestCase = new TestCaseData(3);
        firstTestCase.SetCategory("Aweba");
        firstTestCase.SetName("Testing the 3");
        yield return firstTestCase;
        var secondTestCase = new TestCaseData(5);
        secondTestCase.SetName("Testing the 5");
        yield return secondTestCase;
    }

    [TestCaseSource(nameof(YieldTests), Category = "TestCaseSource Tests")]
    public void Testing(int n)
    {
        Assert.That(n, Is.GreaterThan(0));
    }
}