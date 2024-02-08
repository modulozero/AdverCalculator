using GrpcCalculator.Server;

namespace GrpcCalculator.ServerTests;

public class CalculatorTests
{
    [Fact]
    public void SimpleAddition()
    {
        var calculator = new Calculator();
        calculator = calculator.DigitPressed("5");
        Assert.Equal("5", calculator.Display);
        calculator = calculator.OperatorPressed(CalculatorOperator.Addition);
        Assert.Equal("5", calculator.Display);
        calculator = calculator.DigitPressed("4");
        Assert.Equal("4", calculator.Display);
        calculator = calculator.EqualsPressed();
        Assert.Equal("9", calculator.Display);
    }

    [Fact]
    public void JustEquals()
    {
        var calculator = new Calculator();
        Assert.Equal("0", calculator.Display);
        calculator = calculator.EqualsPressed();
        Assert.Equal("0", calculator.Display);
    }

    [Fact]
    public void MultipleEquals()
    {
        var calculator = new Calculator();
        calculator = calculator.DigitPressed("2");
        Assert.Equal("2", calculator.Display);
        calculator = calculator.OperatorPressed(CalculatorOperator.Multiplication);
        Assert.Equal("2", calculator.Display);
        calculator = calculator.EqualsPressed();
        Assert.Equal("4", calculator.Display);
        calculator = calculator.EqualsPressed();
        Assert.Equal("8", calculator.Display);
    }
}