using CalculatorAPI.Test;
using Gherkin;
using Gherkin.NUnit;

namespace CalculatorAPI.NUnit.Test;

public class GherkinReader_NUnit_Tests
{
    private const string FilePath = "/resources/gherkinreader.gherkin";
    
    [Test, Category("Gherkin")]
    public void GherkinCompiles()
    {
        var reader = new GherkinReaderSteps();
        reader.LoadFile(FilePath);
        Assert.Pass();
    }
    
    [TestCaseSource(
        typeof(GherkinReaderNUnit<GherkinReaderSteps>), 
        nameof(GherkinReaderNUnit<>.YieldTests),
        new object[] {FilePath},
        Category = "Gherkin")
    ]
    public async Task Testing(GherkinTest<GherkinReaderTestContext> test)
    {
        await test.Run();
        Assert.Pass();
    }
}