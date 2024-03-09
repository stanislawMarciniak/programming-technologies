using Sample_program;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            FactorialCalculator calculator = new FactorialCalculator();
            bool quit = false;

            while (!quit)
            {
                Console.WriteLine("Enter a (non-negative) integer to calculate factorial (or 'q' to quit): ");
                string inputString = Console.ReadLine();

                if (inputString.ToLower() == "q")
                {
                    quit = true;
                    continue;
                }
                try
                {
                    int inputInt = int.Parse(inputString);
                    int result = calculator.Factorial(inputInt);
                    Console.WriteLine($"Factorial: {inputInt}! = {result}");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                catch (OverflowException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Enter valid non-negative integer or 'q' to quit: ");
                }
            }
        }
    }
}