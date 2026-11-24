using System.Text.RegularExpressions;
using Gherkin;

namespace CalculatorAPI.Test;

public class GherkinReaderSteps : GherkinReader<GherkinReaderTestContext>
{
    public GherkinReaderSteps()
    {
        Given("I want to use the GherkinReader", context =>
        {
            context.Reader = new GherkinReaderSteps();
        });
        
        When(new Regex("I load \"(?<filename>.*)\""), (context, groups) =>
        {
            try
            {
                var fileName = groups["filename"].Value;
                context.Reader.LoadFile("/resources/" + fileName);
            }
            catch (Exception e)
            {
                context.Exception = e;
            }
        });
        
        Then(new Regex("I am (?<not>(not |))successful"), (context, groups) =>
        {
            var isPositive = groups["not"].Value != "not ";
            Assert.That(context.Exception, isPositive ? Is.Null : Is.Not.Null);
        });
        
        Then(new Regex(@"I have loaded (?<tests>\d+) tests?"), (context, groups) =>
        {
            var testsStr = groups["tests"].Value;
            var tests = int.Parse(testsStr);
            Assert.That(context.Reader.YieldTests().Count(), Is.EqualTo(tests));
        });
        
        Then(new Regex(@"the first test has (?<sentences>\d+) sentences"), (context, groups) =>
        {
            var sentencesStr = groups["sentences"].Value;
            var sentences = int.Parse(sentencesStr);
            var firstTest = context.Reader.YieldTests().First();
            Assert.That(firstTest.Sentences, Has.Count.EqualTo(sentences));
        });
        
        Then(new Regex("I get an exception with message \"(?<exception>.*)\""), (context, groups) =>
        {
            var exception = groups["exception"].Value;
            Assert.That(context.Exception?.Message, Does.Contain(exception));
        });
    }
}