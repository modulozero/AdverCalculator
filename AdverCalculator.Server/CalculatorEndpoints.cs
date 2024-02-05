namespace AdverCalculator.Server;

public static class CalculatorEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/add", (double left, double right) => Task.FromResult(left + right)).WithName("Add").WithOpenApi();
        app.MapGet("/subtract", (double left, double right) => Task.FromResult(left - right)).WithName("Subtract").WithOpenApi();
        app.MapGet("/multiply", (double left, double right) => Task.FromResult(left * right)).WithName("Multiply").WithOpenApi();
        app.MapGet("/divide", (double left, double right) => Task.FromResult(left / right)).WithName("Divide").WithOpenApi();
    }
}
