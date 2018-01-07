namespace BotTrader.Model
{
    public class Matematica
    {
        public class ResultadoCalculoCrescimentoValorCompraEQuantidadeCompras
        {
            /// <summary>
            /// Percentual médio do crescimento do valor de compra das últimas 3 horas
            /// </summary>
            public decimal PercentualMedioCrescimentoValorCompra { get; set; }

            /// <summary>
            /// Percentual de crescimento da quantidade de compra da última hora em comparação com a hora anterior
            /// </summary>
            public decimal CrescimentoQtdCompra { get; set; }
        }
    }
}
