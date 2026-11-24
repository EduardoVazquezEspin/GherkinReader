using System.Text.RegularExpressions;

namespace Gherkin.parser.test;

internal class AddSentence<TContext> : IParser<TContext> where TContext : new()
{
    private readonly Regex _sentenceRegex = new(@"^\s*(?<verb>(Given|When|Then|And))\s*(?<content>.*)\s*$", RegexOptions.IgnoreCase);
    public bool Parse(string line, GherkinParser<TContext> context)
    {
        var match = _sentenceRegex.Match(line);
        if (!match.Success)
            return false;

        var verbStr = match.Groups["verb"].Value;
        var verb = context.GetVerb(verbStr) ?? context.LastVerb;
        if(verb is null)
            throw new Exception($"And clause used wrongly: {line.Trim()}");
        context.LastVerb = verb;
        var content = match.Groups["content"].Value;
        var handler = context.GetHandler((Verb) verb, content, out var handlerMatch);
        if(handler is null)
            throw new Exception($"Step does not match any implementation: {line.Trim()}");
        
        context.File.Tests[^1].sentences.Add(new GherkinSentence<TContext>
        {
            Handler = handler,
            Groups = handlerMatch,
            OriginalSentence = line
        });
        return true;
    }
}