namespace CalculatorAPI;

public interface ICalculatorAPI
{
    public Task<CalculatorResponse> SendAsync(CalculatorRequest request);
}