using Sample_program;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTestFactorial
    {
        [TestMethod]
        public void Factorial_ZeroInput_ReturnOne()
        {
            FactorialCalculator calculator = new FactorialCalculator();
            int result = calculator.Factorial(0);       // 0! = 1
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Factorial_PositiveNumberInput_ReturnCorrectValue()
        {
            FactorialCalculator calculator = new FactorialCalculator();
            int result = calculator.Factorial(6);       // 6! = 720
            Assert.AreEqual(720, result);
        }

        [TestMethod]
        public void Factorial_NegativeNumberInput_ThrowArgumentException()
        {
            FactorialCalculator calculator = new FactorialCalculator();
            Assert.ThrowsException<ArgumentException>(() => calculator.Factorial(-1));
        }

        [TestMethod]
        public void Factorial_TooBigNumberInput_ThrowOverflowException()
        {
            FactorialCalculator calculator = new FactorialCalculator();
            Assert.ThrowsException<OverflowException>(() => calculator.Factorial(int.MaxValue));
        }
    }
}