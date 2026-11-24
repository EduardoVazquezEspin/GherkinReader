namespace Gherkin.parser;

internal interface IParser<TContext> where TContext : new()
{
    internal bool Parse(string line, GherkinParser<TContext> context);
}