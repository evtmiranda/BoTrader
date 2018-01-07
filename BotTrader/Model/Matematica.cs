namespace BotTrader.Model
{
    internal class Matematica
    {
        internal class ResultadoAnalisaCrescimentoValorCompraEQuantidadeCompras
        {
            /// <summary>
            /// Variação média do crescimento do valor de compra das últimas 3 horas
            /// </summary>
            public decimal VariacaoMediaCrescimento { get; set; }

            /// <summary>
            /// % de crescimento da quantidade de compra da última hora em comparação com a hora anterior
            /// </summary>
            public decimal CrescimentoQtdCompra { get; set; }
        }
    }
}
