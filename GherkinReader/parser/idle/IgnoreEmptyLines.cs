namespace Gherkin.parser.idle;

internal class IgnoreEmptyLines<TContext> : IParser<TContext> where TContext : new()
{
    public bool Parse(string line, GherkinParser<TContext> context)
    {
        return string.IsNullOrWhiteSpace(line);
    }
}