using System.Text.RegularExpressions;

namespace GherkinReader.parser.idle;

internal class StartCompilingTitle<TContext> : IParser<TContext> where TContext : new()
{
    private readonly Regex _featureTitleRegex = new (@"^\s*Feature:\s*(?<title>.*)\s*$", RegexOptions.IgnoreCase);
    bool IParser<TContext>.Parse(string line, GherkinParser<TContext> context)
    {
        var titleMatch = _featureTitleRegex.Match(line);
        if (!titleMatch.Success)
            return false;
        
        context.File.CommonTags.AddRange(context.Tags);
        context.Tags = new();
        
        var title = titleMatch.Groups["title"].Value;
        context.State = ParseState.ParsingTitle;
        if (context.File.Feature is not null)
            throw new Exception($"Invalid duplicated Feature definition: {line}");
        context.File.Feature = title;
        return true;
    }
}