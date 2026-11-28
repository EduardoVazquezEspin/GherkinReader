using CalculatorAPI.NUnit.Test.gherkinreader;
using GherkinReader.model;
using GherkinReader.NUnit;

namespace CalculatorAPI.NUnit.Test;

[TestFixture]
public class CalculatorApiTests
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
        nameof(GherkinReaderNUnit<CalculatorPostFixSteps>.YieldTests),
        new object[] {FilePath},
        Category = "Gherkin")
    ]
    public async Task Testing(GherkinTest<CalculatorPostFixTestContext> test)
    {
        await test.Run();
        Assert.Pass();
    }
}