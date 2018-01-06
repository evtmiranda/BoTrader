using System;

namespace BotTrader.Service
{
    internal class Matematica
    {
        private decimal CalcularVariacao(decimal a, decimal b)
        {
            decimal difference = a - b;
            decimal variation = difference / a;
            return variation * 100;
        }
    }
}
