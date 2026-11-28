namespace GherkinReader.model;

public class GherkinFile<TContext> where TContext : new()
{
    public string Feature { get; set; } = null!;
    public List<string> Description { get; set; } = null!;
    public List<GherkinTest<TContext>> Tests { get; set; } = null!;
    public List<GherkinSentence> CommonSteps { get; set; } = null!;
    public List<string> CommonTags { get; set; } = null!;
}