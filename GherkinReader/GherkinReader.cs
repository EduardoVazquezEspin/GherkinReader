using System.Text.RegularExpressions;
using GherkinReader.model;
using GherkinReader.parser;

namespace GherkinReader;

internal enum Verb
{
    Given,
    When,
    Then
}

public class GherkinReader<TContext> : IGherkinReader where TContext : new()
{
#region Given, When, Then syntax
    private readonly List<Handler> _givenHandlers = new();
    private readonly List<Handler> _whenHandlers = new();
    private readonly List<Handler> _thenHandlers = new();
    
    private void Add(Verb verb, string sentence, Action<TContext> handler)
    {
        AddToVerb(verb, new HandlerString<TContext>
        {
            Sentence = sentence,
            Handler = handler
        });
    }

    protected void Given(string sentence, Action<TContext> handler) => Add(Verb.Given, sentence, handler);
    protected void Then(string sentence, Action<TContext> handler) => Add(Verb.Then, sentence, handler);
    protected void When(string sentence, Action<TContext> handler) => Add(Verb.When, sentence, handler);
    
    private void Add(Verb verb, string sentence, Func<TContext, Task> handler)
    {
        AddToVerb(verb, new HandlerStringAsync<TContext>
        {
            Sentence = sentence,
            Handler = handler
        });
    }
    
    protected void Given(string sentence, Func<TContext, Task> handler) => Add(Verb.Given, sentence, handler);
    protected void Then(string sentence, Func<TContext, Task> handler) => Add(Verb.Then, sentence, handler);
    protected void When(string sentence, Func<TContext, Task> handler) => Add(Verb.When, sentence, handler);
    
    private void Add(Verb verb, Regex sentenceRegex, Action<TContext, GroupCollection> handler)
    {
        AddToVerb(verb, new HandlerRegex<TContext>
        {
            SentenceRegex = sentenceRegex,
            Handler = handler
        });
    }
    
    protected void Given(Regex sentenceRegex, Action<TContext, GroupCollection> handler) => Add(Verb.Given, sentenceRegex, handler);
    protected void Then(Regex sentenceRegex, Action<TContext, GroupCollection> handler) => Add(Verb.Then, sentenceRegex, handler);
    protected void When(Regex sentenceRegex, Action<TContext, GroupCollection> handler) => Add(Verb.When, sentenceRegex, handler);
    
    private void Add(Verb verb, Regex sentenceRegex, Func<TContext, GroupCollection, Task> handler)
    {
        AddToVerb(verb, new HandlerRegexAsync<TContext>
        {
            SentenceRegex = sentenceRegex,
            Handler = handler
        });
    }
    
    protected void Given(Regex sentenceRegex, Func<TContext, GroupCollection, Task> handler) => Add(Verb.Given, sentenceRegex, handler);
    protected void Then(Regex sentenceRegex, Func<TContext, GroupCollection, Task> handler) => Add(Verb.Then, sentenceRegex, handler);
    protected void When(Regex sentenceRegex, Func<TContext, GroupCollection, Task> handler) => Add(Verb.When, sentenceRegex, handler);
    
    private void Add(Verb verb, Regex sentenceRegex, Action<TContext> handler)
    {
        AddToVerb(verb, new HandlerRegexWithoutMatch<TContext>()
        {
            SentenceRegex = sentenceRegex,
            Handler = handler
        });
    }
    
    protected void Given(Regex sentenceRegex, Action<TContext>  handler) => Add(Verb.Given, sentenceRegex, handler);
    protected void Then(Regex sentenceRegex, Action<TContext>  handler) => Add(Verb.Then, sentenceRegex, handler);
    protected void When(Regex sentenceRegex, Action<TContext>  handler) => Add(Verb.When, sentenceRegex, handler);
    
    private void Add(Verb verb, Regex sentenceRegex, Func<TContext, Task> handler)
    {
        AddToVerb(verb, new HandlerRegexWithoutMatchAsync<TContext>()
        {
            SentenceRegex = sentenceRegex,
            Handler = handler
        });
    }
    
    protected void Given(Regex sentenceRegex, Func<TContext, Task>  handler) => Add(Verb.Given, sentenceRegex, handler);
    protected void Then(Regex sentenceRegex, Func<TContext, Task>  handler) => Add(Verb.Then, sentenceRegex, handler);
    protected void When(Regex sentenceRegex, Func<TContext, Task>  handler) => Add(Verb.When, sentenceRegex, handler);

    private void AddToVerb(Verb verb, Handler handler)
    {
        switch(verb)
        {
            case Verb.Given:
                _givenHandlers.Add(handler);
                break;
            case Verb.When:
                _whenHandlers.Add(handler);
                break;
            case Verb.Then: default: 
                _thenHandlers.Add(handler);
                break;
        }
    }
#endregion

#region Read Feature File
    private GherkinFile<TContext> _featureFile = null!;
    
    // This should be independent of folder structure and OS, and depend only on file content
    public void LoadFile(string filepath)
    {
        _featureFile = new GherkinFile<TContext> { Tests = new() };
        var content = File.ReadAllText(Environment.CurrentDirectory + filepath);
        var split = content.Split(Environment.NewLine);
        var parser = new GherkinParser<TContext>(_givenHandlers, _whenHandlers, _thenHandlers);
        _featureFile = parser.Parse(split);
    }
#endregion

#region Execute Feature File
    public IEnumerable<IGherkinTest> YieldTests()
    {
        return _featureFile.Tests;
    }
#endregion
}