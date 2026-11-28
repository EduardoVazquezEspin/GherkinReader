using System.Text.RegularExpressions;
using GherkinReader.model;

namespace GherkinReader.parser.idle;

internal class StartParsingTest<TContext> : IParser<TContext> where TContext : new()
{
    private readonly Regex _testTitleRegex = new(@"^\s*Scenario:\s*(?<title>.*)\s*$", RegexOptions.IgnoreCase);
    bool IParser<TContext>.Parse(string line, GherkinParser<TContext> context)
    {
        var match = _testTitleRegex.Match(line);
        if (!match.Success)
            return false;

        var title = match.Groups["title"].Value;
        context.State = ParseState.ParsingsTests;
        var test = new GherkinTest<TContext>
        {
            title = title,
            sentences = new(),
            tags = context.File.CommonTags.Concat(context.Tags).ToList()
        };
        context.Tags = new();
        
        // ReSharper disable once ConstantConditionalAccessQualifier
        context.File.CommonSteps?.ForEach(test.sentences.Add);
        context.File.Tests.Add(test);
        return true;
    }
}