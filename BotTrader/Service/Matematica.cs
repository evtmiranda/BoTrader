using System;

namespace BotTrader.Service
{
    internal class Matematica
    {
        private double CalcularVariacao(double a, double b)
        {
            double difference = a - b;
            double variation = difference / a;
            return variation * 100;
        }
    }
}
