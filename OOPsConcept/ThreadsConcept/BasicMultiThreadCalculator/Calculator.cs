namespace ThreadsConcept.BasicMultiThreadCalculator;

public class Calculator
{
    public double Add(double a, double b)
    {
        Thread.Sleep(2000);
        return a + b;
    }

    public double Subtract(double a, double b)
    {
        Thread.Sleep(2000);
        return a - b;
    }

    public double Multiply(double a, double b)
    {
        Thread.Sleep(2000);
        return a * b;
    }

    public double Divide(double a, double b)
    {
        // Simulate a long-running operation
        Thread.Sleep(2000);
        if (b == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero.");
        }
        return a / b;
    }
}
