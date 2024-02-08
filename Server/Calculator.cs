using System.Globalization;
using System.Text.Json.Serialization;

namespace GrpcCalculator.Server;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CalculatorState
{
    AfterEquals,
    AfterOperator,
    AfterDigit,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CalculatorOperator
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
}

public static class CalculatorOperatorMethods
{
    public static string Evaluate(this CalculatorOperator op, string leftOperand, string rightOperand)
    {
        var leftOperandValue = double.Parse(leftOperand);
        var rightOperandValue = double.Parse(rightOperand);

        return op switch
        {
            CalculatorOperator.Addition => (leftOperandValue + rightOperandValue).ToString(CultureInfo.InvariantCulture),
            CalculatorOperator.Subtraction => (leftOperandValue - rightOperandValue).ToString(CultureInfo.InvariantCulture),
            CalculatorOperator.Multiplication => (leftOperandValue * rightOperandValue).ToString(CultureInfo.InvariantCulture),
            CalculatorOperator.Division => (leftOperandValue / rightOperandValue).ToString(CultureInfo.InvariantCulture),
        };
    }
}

public record Calculator(CalculatorState State, string LeftOperand, CalculatorOperator? Operator, string RightOperand)
{
    private string Evaluate() => Operator.HasValue switch {
        true => Operator.Value.Evaluate(LeftOperand, RightOperand),
        false => RightOperand,
    };

    public Calculator() : this(CalculatorState.AfterEquals, "0", null, "0") { }

    public Calculator OperatorPressed(CalculatorOperator op) => new(
        LeftOperand: State switch {
            CalculatorState.AfterOperator or CalculatorState.AfterEquals => Display,
            CalculatorState.AfterDigit => Evaluate(),
        }, 
        Operator: op, 
        RightOperand: Display, 
        State: CalculatorState.AfterOperator);

    public Calculator EqualsPressed() => this with {
        LeftOperand = Evaluate(),
        State = CalculatorState.AfterEquals,
    };

    public Calculator DigitPressed(string digit) => this with {
        LeftOperand = State switch {
            CalculatorState.AfterEquals or CalculatorState.AfterOperator => Display,
            CalculatorState.AfterDigit => LeftOperand,
        },
        RightOperand = State switch {
            CalculatorState.AfterOperator or CalculatorState.AfterEquals => digit,
            CalculatorState.AfterDigit => RightOperand + digit,
        },
        State = CalculatorState.AfterDigit,
    };

    public string Display =>
        State switch
        {
            CalculatorState.AfterDigit => RightOperand,
            CalculatorState.AfterOperator or CalculatorState.AfterEquals => LeftOperand,
        };
}
