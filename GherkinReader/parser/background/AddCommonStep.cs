using System.Text.RegularExpressions;

namespace Gherkin.parser.background;

internal class AddCommonStep<TContext> : IParser<TContext> where TContext : new()
{
    private readonly Regex _sentenceRegex = new(@"^\s*(?<verb>(Given|And))\s*(?<content>.*)\s*$", RegexOptions.IgnoreCase);
    public bool Parse(string line, GherkinParser<TContext> context)
    {
        var match = _sentenceRegex.Match(line);
        if (!match.Success)
            return false;

        var content = match.Groups["content"].Value;
        var handler = context.GetHandler(Verb.Given, content, out var handlerMatch);
        if(handler is null)
            throw new Exception($"Step does not match any implementation: {line.Trim()}");
        
        context.File.CommonSteps.Add(new GherkinSentence<TContext>
        {
            Handler = handler,
            Groups = handlerMatch,
            OriginalSentence = line
        });
        return true;
    }
}