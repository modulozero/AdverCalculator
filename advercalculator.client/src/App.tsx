import { useState } from 'react';
import './App.css';

const OOperator = {
    Addition: "add",
    Subtraction: "subtract",
    Multiplication: "multiply",
    Division: "divide",
} as const;
type Operator = typeof OOperator[keyof typeof OOperator];

enum CalculatorState {
    AFTER_EQUALS,
    AFTER_OPERATOR,
    AFTER_DIGIT,
}

interface Calculator {
    readonly leftOperand: string;
    readonly rightOperand: string;
    readonly operator?: Operator;
    readonly state: CalculatorState;
}

function makeCalculator(): Calculator {
    return {
        leftOperand: "0",
        rightOperand: "0",
        operator: undefined,
        state: CalculatorState.AFTER_EQUALS
    }
}

async function evaluate(c: Calculator) {
    if (c.operator === undefined) {
        return c.rightOperand;
    }

    return await fetch(c.operator.toString() + "?" + new URLSearchParams({
        left: c.leftOperand,
        right: c.rightOperand,
    })).then(r => r.text());
}

async function operator(c: Calculator, op: Operator): Promise<Calculator> {
    switch (c.state) {
        case CalculatorState.AFTER_EQUALS:
        case CalculatorState.AFTER_OPERATOR:
            return {
                ...c,
                leftOperand: c.rightOperand,
                operator: op,
                state: CalculatorState.AFTER_OPERATOR,
            }
        case CalculatorState.AFTER_DIGIT:
            const result = await evaluate(c);
            return {
                ...c,
                operator: op,
                leftOperand: result,
                state: CalculatorState.AFTER_OPERATOR,
            }
    }
}

async function digit(c: Calculator, digit: string): Promise<Calculator> {
    switch (c.state) {
        case CalculatorState.AFTER_EQUALS:
        case CalculatorState.AFTER_OPERATOR:
            return {
                ...c,
                leftOperand: c.rightOperand,
                rightOperand: digit,
                state: CalculatorState.AFTER_DIGIT,
            }
        case CalculatorState.AFTER_DIGIT:
            return {
                ...c,
                rightOperand: c.rightOperand + digit,
            }
    }
}

async function equals(c: Calculator): Promise<Calculator> {
    return {
        ...c,
        leftOperand: await evaluate(c),
        state: CalculatorState.AFTER_EQUALS,
    }
}

function App() {
    const [calculator, setCalculator] = useState<Calculator>(makeCalculator());
    const [uiEnabled, setUiEnabled] = useState<boolean>(true);

    async function operatorClicked(op: Operator) {
        setUiEnabled(false);
        setCalculator(await operator(calculator, op));
        setUiEnabled(true);
    }

    async function digitClicked(d: number) {
        setUiEnabled(false);
        setCalculator(await digit(calculator, d.toString()));
        setUiEnabled(true);
    }

    async function equalsClicked() {
        setUiEnabled(false);
        setCalculator(await equals(calculator));
        setUiEnabled(true);
    }

    return (
        <div>
            <div>{calculator.state == CalculatorState.AFTER_DIGIT ? calculator.rightOperand : calculator.leftOperand}</div>
            <div>
                <button disabled={!uiEnabled} onClick={() => operatorClicked(OOperator.Addition)}>+</button>
                <button disabled={!uiEnabled} onClick={() => operatorClicked(OOperator.Subtraction)}>-</button>
                <button disabled={!uiEnabled} onClick={() => operatorClicked(OOperator.Multiplication)}>*</button>
                <button disabled={!uiEnabled} onClick={() => operatorClicked(OOperator.Division)}>/</button>
                <button disabled={!uiEnabled} onClick={() => equalsClicked()}>=</button>
            </div>
            <div>{
                    [1, 2, 3].map((r) => (
                        <div key={r}>{
                            [1, 2, 3].map((c) => (
                                <button key={r * c} disabled={!uiEnabled} onClick={() => digitClicked(r * c)}>{r * c}</button>
                            ))
                        }</div>
                    ))
                
            }</div>
            <div> <button disabled={!uiEnabled} onClick={() => digitClicked(0)}>0</button></div>
        </div>
    );
}

export default App;