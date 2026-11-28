using System.Text;

namespace GherkinReader.model;

public class TestException<T> : Exception where T : new()
{
    private const string Tab = "    ";
    private const string HereLeft = "  > ";
    private const string HereRight = " <";
    
    public TestException(int index, GherkinTest<T> test) : base(DecorateError(index, test)) { }

    private static string DecorateError(int index, GherkinTest<T> test)
    {
        var builder = new StringBuilder();
        builder.AppendLine();
        builder.AppendLine(test.title.Trim());
        for (int i = 0; i < index; i++)
        {
            builder.Append(Tab);
            builder.AppendLine(test.sentences[i].OriginalSentence.Trim());
        }
        
        builder.Append(HereLeft);
        builder.Append(test.sentences[index].OriginalSentence.Trim());
        builder.AppendLine(HereRight);

        for (int i = index + 1; i < test.sentences.Count; i++)
        {
            builder.Append(Tab);
            builder.AppendLine(test.sentences[i].OriginalSentence.Trim());
        }

        return builder.ToString();
    }
}

public class GherkinTest<TContext> : IGherkinTest where TContext : new()
{
    internal string title { get; init; } = null!;
    public string Title => title;
    internal List<GherkinSentence> sentences { get; init; } = null!;
    public List<GherkinSentence> Sentences => sentences;
    
    internal List<string> tags { get; init; }
    public List<string> Tags => tags;
    
    internal string? skip { get; set; }
    public string? Skip => skip;

    public override string ToString()
    {
        return title;
    }

    public async Task Run()
    {
        var context = new TContext();
        for (var sentenceIndex = 0; sentenceIndex < sentences.Count; sentenceIndex++)
        {
            var handler = sentences[sentenceIndex].Handler;
            try
            {
                switch (handler)
                {
                    case HandlerString<TContext> handlerString:
                        handlerString.Handler(context);
                        break;
                    case HandlerStringAsync<TContext> handlerStringAsync:
                        await handlerStringAsync.Handler(context);
                        break;
                    case HandlerRegex<TContext> handlerRegex:
                        handlerRegex.Handler(context, sentences[sentenceIndex].Groups!);
                        break;
                    case HandlerRegexAsync<TContext> handlerRegexAsync:
                        await handlerRegexAsync.Handler(context, sentences[sentenceIndex].Groups!);
                        break;
                    case HandlerRegexWithoutMatch<TContext> handlerRegexWithoutMatch:
                        handlerRegexWithoutMatch.Handler(context);
                        break;
                    case HandlerRegexWithoutMatchAsync<TContext> handlerRegexWithoutMatchAsync:
                        await handlerRegexWithoutMatchAsync.Handler(context);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new TestException<TContext>(sentenceIndex, this);
            }
        }
    }
}