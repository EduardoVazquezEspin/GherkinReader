using GherkinReader;

namespace CalculatorAPI.NUnit.Test.gherkinreader;

public class GherkinReaderTestContext
{
    public GherkinReader<GherkinReaderTestContext> Reader { get; set; }
    public Exception? Exception { get; set; }
}