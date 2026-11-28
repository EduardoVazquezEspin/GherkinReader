using CalculatorAPI.NUnit.Test.gherkinreader;
using GherkinReader.model;
using GherkinReader.NUnit;

namespace CalculatorAPI.NUnit.Test;

// ReSharper disable once InconsistentNaming
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
        nameof(GherkinReaderNUnit<GherkinReaderSteps>.YieldTests),
        new object[] {FilePath},
        Category = "Gherkin")
    ]
    public async Task Testing(GherkinTest<GherkinReaderTestContext> test)
    {
        await test.Run();
        Assert.Pass();
    }
}