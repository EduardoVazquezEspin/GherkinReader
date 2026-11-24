using System.Text.RegularExpressions;

namespace CalculatorAPI.PostFix;

internal class PostFixCalculator
{
    private readonly List<int> _stack = new ();
    private const string EmptyRequest = "Invalid Empty Request";
    private readonly Regex _isIntegerRegex = new (@"^\d$");
    
    public CalculatorResponse Process(string request)
    {
        for (int index = 0; index < request.Length; index++)
        {
            var character = request[index];
            
            if(character == ' ') continue;

            var value = IsInteger(character);
            if (value != -1)
            {
                int result = value;
                while (++index < request.Length && (value = IsInteger(request[index])) != -1)
                    result = 10 * result + value;

                index--;
                _stack.Add(result);
                continue;
            }

            if (character == '+' && index + 1 < request.Length && request[index + 1] == '+')
            {
                index++;
                var result = _stack.Sum();
                _stack.RemoveRange(0, _stack.Count);
                _stack.Add(result);
                continue;
            }
            else if(character == '+')
            {
                if (_stack.Count < 2)
                    return new CalculatorResponse
                    {
                        Status = 400,
                        Body = $"Invalid Operation at position {index}. Two previous values are required"
                    };

                var result = _stack[^1] + _stack[^2];
                _stack.RemoveRange(_stack.Count - 2, 2);
                _stack.Add(result);
                continue;
            }
            
            if (character == '*')
            {
                if (_stack.Count < 2)
                    return new CalculatorResponse
                    {
                        Status = 400,
                        Body = $"Invalid Operation at position {index}. Two previous values are required"
                    };

                var result = _stack[^1] * _stack[^2];
                _stack.RemoveRange(_stack.Count - 2, 2);
                _stack.Add(result);
                continue;
            }
            
            return new CalculatorResponse
            {
                Status = 400,
                Body = $"Invalid Character at position {index}"
            };
        }

        if (_stack.Count == 0)
            return new CalculatorResponse
            {
                Status = 400,
                Body = EmptyRequest
            };

        return new CalculatorResponse
        {
            Status = 200,
            Body = string.Join(' ', _stack.Select(it => it.ToString()))
        };
    }

    private int IsInteger(char c)
    {
        if (!_isIntegerRegex.IsMatch(c.ToString())) return -1;
        return int.Parse(c.ToString());
    }
}