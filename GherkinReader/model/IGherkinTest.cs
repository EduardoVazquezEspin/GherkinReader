namespace Gherkin;

public interface IGherkinTest
{
    public string Title { get; }
    public List<IGherkinSentence> Sentences { get; }
    public List<string> Tags { get; }
    public string? Skip { get; }
}