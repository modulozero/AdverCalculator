namespace GrpcCalculator.Server;

public static class CalculatorEndpoints
{
    public static RouteGroupBuilder MapCalculatorApi(this RouteGroupBuilder group)
    {
        group.MapPost("/operator/{op}", (
            CalculatorOperator op,
            Calculator calculator) => calculator.OperatorPressed(op));
        group.MapPost("/equals", (Calculator calculator) => calculator.EqualsPressed());
        group.MapPost("/digit/{digit}", (string digit, Calculator calculator) => calculator.DigitPressed(digit));

        return group;
    }
}
