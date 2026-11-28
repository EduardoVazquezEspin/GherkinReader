using System.Text.RegularExpressions;

namespace GherkinReader.model;

public class GherkinSentence
{
    public string OriginalSentence { get; init; } = null!;
    public Handler Handler { get; init; } = null!;
    public GroupCollection? Groups { get; init; }
}