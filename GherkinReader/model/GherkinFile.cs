namespace Gherkin;

public class GherkinFile<TContext> where TContext : new()
{
    public string Feature { get; set; }
    public List<string> Description { get; set; }
    public List<GherkinTest<TContext>> Tests { get; set; }
    public List<GherkinSentence<TContext>> CommonSteps { get; set; }
    public List<string> CommonTags { get; set; }
}