using System.Collections.Generic;

namespace Meetup.Api.Financeiro.ViewModel
{
    public class MoedaViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Simbolo { get; set; }
        public int Rank { get; set; }
        public decimal Preco { get; set; }
    }

    public class ListaCriptoMoedas
    {
        public IEnumerable<CriptoMoeda> data { get; set; }
    }


    public class Cotacao
    {
        public decimal price { get; set; }
    }
    public class ListaCotacao
    {
        public Dictionary<string, Cotacao> quotes { get; set; }
    }

    public class CriptoMoeda
    {
        public int id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public int cmc_rank { get; set; }
        public Dictionary<string, Cotacao> quote { get; set; }
    }


    public class Cotacoes
    {
        public  Moeda USD { get; set; }
    }

    public class Moeda
    {
        public string code { get; set; }
        public string codein { get; set; }
        public string name { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string varBid { get; set; }
        public string pctChange { get; set; }
        public string bid { get; set; }
        public string ask { get; set; }
        public string timestamp { get; set; }
        public string create_date { get; set; }
    }

}
