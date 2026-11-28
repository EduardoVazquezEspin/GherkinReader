using CalculatorAPI.model;
using CalculatorAPI.PostFix;

namespace CalculatorAPI;

public class CalculatorApi : ICalculatorApi
{
    private const string PostFixEndpoint = "/postfix";
    private const string EndpointNotFound = "Endpoint not found";
    
    public async Task<CalculatorResponse> SendAsync(CalculatorRequest request)
    {
        await Task.Delay(1000); 
        
        return request.Endpoint switch
        {
            PostFixEndpoint => new PostFixCalculator().Process(request.Body),
            _ => new CalculatorResponse
            {
                Body = EndpointNotFound,
                Status = 404
            }
        };
    }
}