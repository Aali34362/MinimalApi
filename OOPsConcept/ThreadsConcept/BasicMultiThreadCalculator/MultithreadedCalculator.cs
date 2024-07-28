namespace ThreadsConcept.BasicMultiThreadCalculator;

public class MultithreadedCalculator
{
    private readonly Calculator _calculator = new Calculator();

    public void Calculate(string operation, double a, double b)
    {
        Thread thread = new Thread(() => PerformOperation(operation, a, b));
        thread.Start();
    }

    private void PerformOperation(string operation, double a, double b)
    {
        double result = 0;
        try
        {
            switch (operation.ToLower())
            {
                case "add":
                    result = _calculator.Add(a, b);
                    break;
                case "subtract":
                    result = _calculator.Subtract(a, b);
                    break;
                case "multiply":
                    result = _calculator.Multiply(a, b);
                    break;
                case "divide":
                    result = _calculator.Divide(a, b);
                    break;
                default:
                    Console.WriteLine("Unknown operation.");
                    return;
            }
            Console.WriteLine($"{operation}({a}, {b}) = {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}