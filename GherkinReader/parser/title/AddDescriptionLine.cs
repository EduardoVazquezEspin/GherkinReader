namespace GherkinReader.parser.title;

internal class AddDescriptionLine<TContext> : IParser<TContext> where TContext : new()
{
    bool IParser<TContext>.Parse(string line, GherkinParser<TContext> context)
    {
        context.File.Description.Add(line);
        return true;
    }
}