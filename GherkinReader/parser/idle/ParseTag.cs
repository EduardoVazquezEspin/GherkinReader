using System.Text.RegularExpressions;

namespace GherkinReader.parser.idle;

internal class ParseTag<TContext> : IParser<TContext> where TContext : new()
{
    private readonly Regex _tagsRegex = new (@"\s*@(?<tag>[^\s]+)\s*", RegexOptions.IgnoreCase);

    bool IParser<TContext>.Parse(string line, GherkinParser<TContext> context)
    {
        var startIndex = 0;
        var tags = new List<string>();
        do
        {
            var match = _tagsRegex.Match(line, startIndex);
            if (!match.Success)
                return false;
            startIndex += match.Groups["0"].Value.Length;
            tags.Add(match.Groups["tag"].Value);
        } while (startIndex < line.Length);
        
        context.Tags.AddRange(tags);
        return true;
    }
}