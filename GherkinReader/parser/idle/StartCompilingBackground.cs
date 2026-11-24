using System.Text.RegularExpressions;

namespace Gherkin.parser.idle;

internal class StartCompilingBackground<TContext> : IParser<TContext> where TContext : new()
{
    private readonly Regex _featureTitleRegex = new (@"^\s*Background:\s*$", RegexOptions.IgnoreCase);
    public bool Parse(string line, GherkinParser<TContext> context)
    {
        var backgroundMatch = _featureTitleRegex.Match(line);
        if (!backgroundMatch.Success)
            return false;
        
        context.File.CommonTags.AddRange(context.Tags);
        context.Tags = new();
        
        context.State = ParseState.ParsingBackground;
        if (context.File.CommonSteps is not null)
            throw new Exception($"Duplicated Background clause: {line}");
        context.File.CommonSteps = new ();
        return true;
    }
}