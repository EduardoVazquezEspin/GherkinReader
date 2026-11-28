using System.Text.RegularExpressions;
using GherkinReader.model;
using GherkinReader.parser.background;
using GherkinReader.parser.common;
using GherkinReader.parser.idle;
using GherkinReader.parser.test;
using GherkinReader.parser.title;

namespace GherkinReader.parser;

internal enum ParseState
{
    Idle,
    ParsingTitle,
    ParsingBackground,
    ParsingsTests
}

internal class GherkinParser<TContext> where TContext : new()
{
    private readonly List<Handler> _givenHandlers;
    private readonly List<Handler> _whenHandlers;
    private readonly List<Handler> _thenHandlers;

    public GherkinParser(
        List<Handler> givenHandlers, 
        List<Handler> whenHandlers,
        List<Handler> thenHandlers)
    {
        _givenHandlers = givenHandlers;
        _whenHandlers = whenHandlers;
        _thenHandlers = thenHandlers;
    }

    public ParseState State = ParseState.Idle;
    public Verb? LastVerb = null;
    public List<string> Tags = new();
    public readonly GherkinFile<TContext> File = new GherkinFile<TContext>
    {
        Tests = new(),
        Description = new (),
        CommonTags = new ()
    };
    private readonly Dictionary<ParseState, IParser<TContext>[]> _parsers = new()
    {
        { ParseState.Idle, new IParser<TContext>[]{
            new IgnoreEmptyLines<TContext>(), 
            new ParseTag<TContext>(), 
            new StartCompilingTitle<TContext>(), 
            new StartCompilingBackground<TContext>(), 
            new StartParsingTest<TContext>()} },
        { ParseState.ParsingTitle, new IParser<TContext>[]{
            new FinishParsingSection<TContext>(),
            new AddDescriptionLine<TContext>()} },
        { ParseState.ParsingBackground, new IParser<TContext>[]{
            new FinishParsingSection<TContext>(), 
            new AddCommonStep<TContext>()} },
        { ParseState.ParsingsTests, new IParser<TContext>[]{
            new FinishParsingSection<TContext>(), 
            new AddSentence<TContext>()}}
    };

    private readonly Regex _skipTag = new ("^skip$", RegexOptions.IgnoreCase);
    
    private void RemapSkipTag()
    {
        foreach (var test in File.Tests)
        {
            for (var i = 0; i < test.tags.Count; i++)
            {
                var match = _skipTag.Match(test.tags[i]);
                if (!match.Success) continue;
                test.skip = "";
                test.tags.RemoveAt(i--);
            }
        }
    }

    public GherkinFile<TContext> Parse(string[] file)
    {
        foreach (var line in file)
        {
            _parsers.TryGetValue(State, out var handlers);
            var hasBeenParsed = false;
            for (var handlerIndex = 0; !hasBeenParsed && handlerIndex < handlers!.Length; handlerIndex++)
                hasBeenParsed = handlers[handlerIndex].Parse(line, this);
            if(!hasBeenParsed)
                throw new Exception($"Sentence did not match any pattern: {line.Trim()}");
        }

        RemapSkipTag();
        
        return File;
    }
    
    public Handler? GetHandler(Verb verb, string line, out GroupCollection? match)
    {
        match = null;
        List<Handler> setToSearch = verb switch
        {
            Verb.Given => _givenHandlers,
            Verb.When => _whenHandlers,
            Verb.Then or _ => _thenHandlers
        };

        foreach (var handler in setToSearch)
        {
            if (handler is HandlerStringBase handlerString)
            {
                if (string.Compare(handlerString.Sentence, line.Trim(), StringComparison.CurrentCultureIgnoreCase) == 0)
                    return handler;
                continue;
            }

            var regex = ((handler as HandlerRegexBase)!).SentenceRegex;
            var localMatch = regex.Match(line.Trim());
            if (localMatch.Success)
            {
                match = localMatch.Groups;
                return handler;
            }
        }

        return null;
    }
    
    public Verb? GetVerb(string str)
    {
        if (string.Compare(str, "Given", StringComparison.CurrentCultureIgnoreCase) == 0)
            return Verb.Given;
        if (string.Compare(str, "When", StringComparison.CurrentCultureIgnoreCase) == 0)
            return Verb.When;
        if (string.Compare(str, "Then", StringComparison.CurrentCultureIgnoreCase) == 0)
            return Verb.Then;
        return null;
    }
}