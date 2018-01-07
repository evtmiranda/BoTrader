using System;
using BotTrader.Model;

namespace BotTrader
{
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">0 para processar somente os dados; 1 para gerar insights e alertas; 2 para processamento full</param>
        static void Main(string[] args)
        {
            if ((!int.TryParse(args[0], out int tipoProcessamento)) || (tipoProcessamento < 0 || tipoProcessamento > 2))
            {
                Console.WriteLine("O parâmetro só pode ser 0, 1 ou 2.");
                Console.ReadLine();
                return;
            }

            switch (tipoProcessamento)
            {
                case 0:
                    new Controller.Controller().ProcessamentoDados();
                    break;
                case 1:
                    new Controller.Controller().GeracaoInsightEAlerta();
                    break;
                case 2:
                    new Controller.Controller().ProcessamentoFull();
                    break;
            }
        }

    }
}
