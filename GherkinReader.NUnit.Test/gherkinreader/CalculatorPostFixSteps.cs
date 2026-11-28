using System.Text.RegularExpressions;
using CalculatorAPI.model;
using GherkinReader;

namespace CalculatorAPI.NUnit.Test.gherkinreader;

public class CalculatorPostFixSteps : GherkinReader<CalculatorPostFixTestContext>
{
    public CalculatorPostFixSteps()
    {
        Given("I am a user of Calculator API", context =>
        {
            context.CalculatorApi = new CalculatorApi();
        });
        
        Given("I am using postfix notation", context =>
        {
            context.RequestEndpoint = "/postfix";
        });
        
        When(new Regex("^I send (?<content>.*)$"), async (context, groups) =>
        {
            var content = groups["content"].Value;
            context.RequestContent = content;

            var response = await context.CalculatorApi.SendAsync(new CalculatorRequest
            {
                Endpoint = context.RequestEndpoint,
                Body = context.RequestContent
            });

            context.ResponseContent = response.Body;
            context.ResponseStatus = response.Status;
        });
        
        Then(new Regex(@"^I obtain status (?<status>\d+)$"), (context, groups) =>
        {
            var status = groups["status"].Value;
            var statusInt = int.Parse(status);

            Assert.That(context.ResponseStatus, Is.EqualTo(statusInt));
        });
        
        Then(new Regex("^I obtain body (?<body>.*)$"), (context, groups) =>
        {
            var body = groups["body"].Value;

            Assert.That(context.ResponseContent.ToString(), Is.EqualTo(body));
        });
    }
}