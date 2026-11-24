using System.Text.RegularExpressions;

namespace Gherkin;

public abstract class Handler<TContext> { }

public abstract class HandlerStringBase<TContext> : Handler<TContext>
{
    public required string Sentence { get; init; }
}

public abstract class HandlerRegexBase<TContext> : Handler<TContext>
{
    public required Regex SentenceRegex { get; init; }
}

public class HandlerString<TContext> : HandlerStringBase<TContext>
{
    public required Action<TContext> Handler { get; init; }
}

public class HandlerStringAsync<TContext> : HandlerStringBase<TContext>
{
    public required Func<TContext, Task> Handler { get; init; }
}

public class HandlerRegex<TContext> : HandlerRegexBase<TContext>
{
    public required Action<TContext, GroupCollection> Handler { get; init; }
}

public class HandlerRegexAsync<TContext> : HandlerRegexBase<TContext>
{
    public required Func<TContext, GroupCollection, Task> Handler { get; init; }
}

public class HandlerRegexWithoutMatch<TContext> : HandlerRegexBase<TContext>
{
    public required Action<TContext> Handler { get; init; }
}

public class HandlerRegexWithoutMatchAsync<TContext> : HandlerRegexBase<TContext>
{
    public required Func<TContext, Task> Handler { get; init; }
}