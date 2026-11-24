using System.Text.RegularExpressions;

namespace Gherkin;

public class GherkinSentence<TContext> : IGherkinSentence where TContext : new()
{
    public string OriginalSentence { get; init; }
    public Handler<TContext> Handler { get; init; }
    public GroupCollection? Groups { get; init; }
}