using CalculatorAPI.Test;
using Gherkin;
using Gherkin.NUnit;

namespace CalculatorAPI.NUnit.Test;

[TestFixture]
public class CalculatorAPITests
{
    private const string FilePath = "/resources/calculator.postfix.gherkin";
    
    [Test, Category("Gherkin")]
    public void GherkinCompiles()
    {
        var reader = new CalculatorPostFixSteps();
        reader.LoadFile(FilePath);
        Assert.Pass();
    }

    [TestCaseSource(
        typeof(GherkinReaderNUnit<CalculatorPostFixSteps>), 
        nameof(GherkinReaderNUnit<>.YieldTests),
        new object[] {FilePath},
        Category = "Gherkin")
    ]
    public async Task Testing(GherkinTest<CalculatorPostFixTestContext> test)
    {
        await test.Run();
        Assert.Pass();
    }
}