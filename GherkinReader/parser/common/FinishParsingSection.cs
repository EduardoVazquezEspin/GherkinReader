namespace Gherkin.parser.common;

internal class FinishParsingSection<TContext> : IParser<TContext> where TContext : new()
{
    public bool Parse(string line, GherkinParser<TContext> context)
    {
        if (!string.IsNullOrWhiteSpace(line))
            return false;
        context.State = ParseState.Idle;
        context.LastVerb = null;
        return true;
    }
}