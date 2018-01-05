using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace BotTrader.Service
{
    internal class Comunicacao
    {
        private void EnviarMensagem(string message)
        {
            string messageLocal = DateTime.UtcNow.ToLocalTime() + " " + message;

            Console.WriteLine(messageLocal);
            EnviarMensagemSlack(messageLocal);
            //EnviarMensagemTelegram();
        }

        private void EnviarMensagemSlack(string message)
        {
            string caminhoArquivoPython = ConfigurationManager.AppSettings.Get("pathArchivePython");
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

        private async Task EnviarMensagemTelegram()
        {
            //TODO: Repositório com exemplos em C#: https://github.com/TelegramBots/telegram.bot.examples

            var botClient = new Telegram.Bot.TelegramBotClient("64099364b969471dfa57590d2305f000");
            var me = await botClient.GetMeAsync();
            Console.WriteLine($"Hello! My name is {me.FirstName}");
        }

        internal void EscreverNaTela(string mensagem)
        {
            string mensagemLocal = DateTime.UtcNow.ToLocalTime() + " " + mensagem;
            Console.WriteLine(mensagemLocal);
        }
    }
}
