using Gherkin;

namespace CalculatorAPI.Test;

public class GherkinReaderTestContext
{
    public GherkinReader<GherkinReaderTestContext> Reader { get; set; }
    public Exception? Exception { get; set; }
}