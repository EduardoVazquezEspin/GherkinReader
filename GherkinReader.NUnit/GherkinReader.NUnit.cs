using NUnit.Framework;

namespace Gherkin.NUnit;

public static class GherkinReaderNUnit<TSteps> where TSteps : IGherkinReader, new()
{
    public static IEnumerable<TestCaseData> YieldTests(string filePath)
    {
        var steps = new TSteps();
        steps.LoadFile(filePath);
        foreach (var test in steps.YieldTests())
        {
            var testData = new TestCaseData(test);
            testData.SetName(test.Title);
            foreach (var tag in test.Tags)
                testData.SetCategory(tag);
            if (test.Skip is not null)
                testData.Ignore(test.Skip);
            
            yield return testData;
        }
    }
}