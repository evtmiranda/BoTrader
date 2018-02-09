using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace BotTrader.Service
{
    public class Comunicacao
    {
        public static void EnviarMensagem(string message)
        {
            string messageLocal = DateTime.UtcNow.ToLocalTime() + " " + message;

            Console.WriteLine(messageLocal);
            EnviarMensagemSlack(messageLocal);
        }

        public static void EscreverNaTela(string mensagem)
        {
            string mensagemLocal = DateTime.UtcNow.ToLocalTime() + " " + mensagem;
            Console.WriteLine(mensagemLocal);
        }

        private static void EnviarMensagemSlack(string message)
        {
            string caminhoArquivoPython = ConfigurationManager.AppSettings.Get("CaminhoArquivoBotPy");
            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                WorkingDirectory = Path.GetDirectoryName(caminhoArquivoPython),
                Arguments = "/c START python " + Path.GetFileName(caminhoArquivoPython) + " " + "\"" + message + "\"",
                UseShellExecute = false
            };

            Process.Start(processInfo);
        }

        private static async Task EnviarMensagemTelegram()
        {
            //TODO: Repositório com exemplos em C#: https://github.com/TelegramBots/telegram.bot.examples

            var botClient = new Telegram.Bot.TelegramBotClient("");
            var me = await botClient.GetMeAsync();
            Console.WriteLine($"Hello! My name is {me.FirstName}");
        }

    }
}
