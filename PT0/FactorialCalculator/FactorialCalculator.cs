using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Factorial
{
    public class FactorialCalculator
    {
        public int Factorial(int n)
        {
            if (n < 0)
            {
                throw new ArgumentException("Value must be non-negative.");
            }
            if (n > Math.Sqrt(int.MaxValue))  // check if the factorial value won't cause overflow 
            {
                throw new OverflowException("Value is too big to calculate its factorial.");
            }

            int result = 1;
            for (int i = 1; i <= n; i++)
            {
                checked // overflow checking 
                {
                    result *= i;
                }
            }
            return result;
        }
    }
}
