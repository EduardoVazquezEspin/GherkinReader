namespace GherkinReader.model;

public interface IGherkinTest
{
    public string Title { get; }
    public List<GherkinSentence> Sentences { get; }
    public List<string> Tags { get; }
    public string? Skip { get; }
}