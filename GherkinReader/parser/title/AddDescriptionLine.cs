namespace Gherkin.parser.common;

internal class AddDescriptionLine<TContext> : IParser<TContext> where TContext : new()
{
    public bool Parse(string line, GherkinParser<TContext> context)
    {
        context.File.Description.Add(line);
        return true;
    }
}