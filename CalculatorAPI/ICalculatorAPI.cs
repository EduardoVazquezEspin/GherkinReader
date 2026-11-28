using CalculatorAPI.model;

namespace CalculatorAPI;

public interface ICalculatorApi
{
    public Task<CalculatorResponse> SendAsync(CalculatorRequest request);
}