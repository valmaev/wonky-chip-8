using System;

namespace WonkyChip8.Interpreter
{
    public class RandomGenerator : IRandomGenerator
    {
        private readonly Random _random = new Random();

        public int Generate(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
    }
}