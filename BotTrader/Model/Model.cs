namespace BotTrader.Model
{
    public class ValorTrade
    {
        public double Valor { get; set; }
        public TipoTrade TipoTrade { get; set; }
    }

    public enum TipoTrade
    {
        Compra,
        Venda
    }
}
