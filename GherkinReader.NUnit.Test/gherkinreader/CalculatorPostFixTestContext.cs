namespace CalculatorAPI.Test;

public class CalculatorPostFixTestContext
{
    public ICalculatorAPI CalculatorApi { get; set; } = null!;
    public string RequestEndpoint { get; set; } = null!;
    public string RequestContent { get; set; } = null!;
    public int ResponseStatus { get; set; }
    public string ResponseContent { get; set; } = null!;
}