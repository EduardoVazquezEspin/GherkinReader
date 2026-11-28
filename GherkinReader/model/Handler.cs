using System.Text.RegularExpressions;

namespace GherkinReader.model;

public abstract class Handler { }

public abstract class HandlerStringBase : Handler
{
    public string Sentence { get; init; } = null!;
}

public abstract class HandlerRegexBase : Handler
{
    public Regex SentenceRegex { get; init; } = null!;
}

public class HandlerString<TContext> : HandlerStringBase
{
    public Action<TContext> Handler { get; init; } = null!;
}

public class HandlerStringAsync<TContext> : HandlerStringBase
{
    public Func<TContext, Task> Handler { get; init; } = null!;
}

public class HandlerRegex<TContext> : HandlerRegexBase
{
    public Action<TContext, GroupCollection> Handler { get; init; } = null!;
}

public class HandlerRegexAsync<TContext> : HandlerRegexBase
{
    public Func<TContext, GroupCollection, Task> Handler { get; init; } = null!;
}

public class HandlerRegexWithoutMatch<TContext> : HandlerRegexBase
{
    public Action<TContext> Handler { get; init; } = null!;
}

public class HandlerRegexWithoutMatchAsync<TContext> : HandlerRegexBase
{
    public Func<TContext, Task> Handler { get; init; } = null!;
}