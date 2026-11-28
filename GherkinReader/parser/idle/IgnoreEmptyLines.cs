namespace GherkinReader.parser.idle;

internal class IgnoreEmptyLines<TContext> : IParser<TContext> where TContext : new()
{
    bool IParser<TContext>.Parse(string line, GherkinParser<TContext> context)
    {
        return string.IsNullOrWhiteSpace(line);
    }
}