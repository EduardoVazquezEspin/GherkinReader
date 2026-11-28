using System.Text.RegularExpressions;
using GherkinReader.model;

namespace GherkinReader.parser.background;

internal class AddCommonStep<TContext> : IParser<TContext> where TContext : new()
{
    private readonly Regex _sentenceRegex = new(@"^\s*(?<verb>(Given|And))\s*(?<content>.*)\s*$", RegexOptions.IgnoreCase);

    bool IParser<TContext>.Parse(string line, GherkinParser<TContext> context)
    {
        var match = _sentenceRegex.Match(line);
        if (!match.Success)
            return false;

        var content = match.Groups["content"].Value;
        var handler = context.GetHandler(Verb.Given, content, out var handlerMatch);
        if(handler is null)
            throw new Exception($"Step does not match any implementation: {line.Trim()}");
        
        context.File.CommonSteps.Add(new GherkinSentence
        {
            Handler = handler,
            Groups = handlerMatch,
            OriginalSentence = line
        });
        return true;
    }
}