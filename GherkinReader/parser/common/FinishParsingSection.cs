namespace GherkinReader.parser.common;

internal class FinishParsingSection<TContext> : IParser<TContext> where TContext : new()
{
    bool IParser<TContext>.Parse(string line, GherkinParser<TContext> context)
    {
        if (!string.IsNullOrWhiteSpace(line))
            return false;
        context.State = ParseState.Idle;
        context.LastVerb = null;
        return true;
    }
}