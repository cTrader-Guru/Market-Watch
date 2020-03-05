/*  CTRADER GURU --> Template 1.0.2

    Homepage    : https://ctrader.guru/
    Telegram    : https://t.me/ctraderguru
    Twitter     : https://twitter.com/cTraderGURU/
    Facebook    : https://www.facebook.com/ctrader.guru/
    YouTube     : https://www.youtube.com/channel/UCKkgbw09Fifj65W5t5lHeCQ
    GitHub      : https://github.com/cTraderGURU/
    TOS         : https://ctrader.guru/termini-del-servizio/

*/

using System;
using cAlgo.API;
using System.Net;
using System.Text;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

// --> Microsoft Visual Studio 2017 --> Strumenti --> Gestione pacchetti NuGet --> Gestisci pacchetti NuGet per la soluzione... --> Installa
using Newtonsoft.Json;

namespace cAlgo
{
    [Indicator(TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    [Levels(0)]
    public class MarketWatch : Indicator
    {

        /// <summary>
        /// ID prodotto, identificativo, viene fornito da ctrader.guru
        /// </summary>
        public const int ID = 60787;

        /// <summary>
        /// Nome del prodotto, identificativo, da modificare con il nome della propria creazione
        /// </summary>
        public const string NAME = "Market Watch";

        /// <summary>
        /// La versione del prodotto, progressivo, utilie per controllare gli aggiornamenti se viene reso disponibile sul sito ctrader.guru
        /// </summary>
        public const string VERSION = "1.0.2";

        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://ctrader.guru/product/market-watch/")]
        public string ProductInfo { get; set; }

        [Parameter("EMA Period", Group = "Parameters", DefaultValue = 500)]
        public int EMAPeriods { get; set; }

        [Parameter("1° Symbol", Group = "Symbols", DefaultValue = "EURUSD")]
        public string SymbolCode1 { get; set; }

        [Parameter("2° Symbol", Group = "Symbols", DefaultValue = "USDJPY")]
        public string SymbolCode2 { get; set; }

        [Parameter("3° Symbol", Group = "Symbols", DefaultValue = "USDCAD")]
        public string SymbolCode3 { get; set; }

        [Parameter("4° Symbol", Group = "Symbols", DefaultValue = "AUDUSD")]
        public string SymbolCode4 { get; set; }

        [Parameter("5° Symbol", Group = "Symbols", DefaultValue = "GBPUSD")]
        public string SymbolCode5 { get; set; }

        [Parameter("6° Symbol", Group = "Symbols", DefaultValue = "NZDUSD")]
        public string SymbolCode6 { get; set; }

        [Parameter("7° Symbol", Group = "Symbols", DefaultValue = "EURGBP")]
        public string SymbolCode7 { get; set; }

        [Parameter("8° Symbol", Group = "Symbols", DefaultValue = "EURJPY")]
        public string SymbolCode8 { get; set; }

        [Parameter("9° Symbol", Group = "Symbols", DefaultValue = "AUDCAD")]
        public string SymbolCode9 { get; set; }

        [Parameter("10° Symbol", Group = "Symbols", DefaultValue = "GBPJPY")]
        public string SymbolCode10 { get; set; }

        [Output("1° Symbol", LineColor = "#fe5f55")]
        public IndicatorDataSeries Symbol1 { get; set; }

        [Output("2° Symbol", LineColor = "#4b644a")]
        public IndicatorDataSeries Symbol2 { get; set; }

        [Output("3° Symbol", LineColor = "#4f6367")]
        public IndicatorDataSeries Symbol3 { get; set; }

        [Output("4° Symbol", LineColor = "#7a9e9f")]
        public IndicatorDataSeries Symbol4 { get; set; }

        [Output("5° Symbol", LineColor = "#4b644a")]
        public IndicatorDataSeries Symbol5 { get; set; }

        [Output("6° Symbol", LineColor = "#967aa1")]
        public IndicatorDataSeries Symbol6 { get; set; }

        [Output("7° Symbol", LineColor = "#192a51")]
        public IndicatorDataSeries Symbol7 { get; set; }

        [Output("8° Symbol", LineColor = "#9d44b5")]
        public IndicatorDataSeries Symbol8 { get; set; }

        [Output("9° Symbol", LineColor = "#4b644a")]
        public IndicatorDataSeries Symbol9 { get; set; }

        [Output("10° Symbol", LineColor = "#1c1c1c")]
        public IndicatorDataSeries Symbol10 { get; set; }

        List<string> RealSymbol = new List<string>(); 

        /// <summary>
        /// La prima procedura che viene eseguita non appena viene inserito l'indicatore sul grafico
        /// </summary>
        protected override void Initialize()
        {

            // --> Stampo nei log la versione corrente
            Print("{0} : {1}", NAME, VERSION);

            // --> Se viene settato l'ID effettua un controllo per verificare eventuali aggiornamenti
            _checkProductUpdate();

            // --> Controllo la presenza di simboli non validi
            SymbolCode1 = SymbolCode1.Trim();
            SymbolCode2 = SymbolCode2.Trim();
            SymbolCode3 = SymbolCode3.Trim();
            SymbolCode4 = SymbolCode4.Trim();
            SymbolCode5 = SymbolCode5.Trim();
            SymbolCode6 = SymbolCode6.Trim();
            SymbolCode7 = SymbolCode7.Trim();
            SymbolCode8 = SymbolCode8.Trim();
            SymbolCode9 = SymbolCode9.Trim();
            SymbolCode10 = SymbolCode10.Trim();

            // --> Nella routin Calculate mostrerebbe un errore nei log, meglio fissare il ritmo da questo punto 
            RealSymbol.Add((SymbolCode1 != "" && Symbols.GetSymbol(SymbolCode1) != null) ? SymbolCode1 : "");
            RealSymbol.Add((SymbolCode2 != "" && Symbols.GetSymbol(SymbolCode2) != null) ? SymbolCode2 : "");
            RealSymbol.Add((SymbolCode3 != "" && Symbols.GetSymbol(SymbolCode3) != null) ? SymbolCode3 : "");
            RealSymbol.Add((SymbolCode4 != "" && Symbols.GetSymbol(SymbolCode4) != null) ? SymbolCode4 : "");
            RealSymbol.Add((SymbolCode5 != "" && Symbols.GetSymbol(SymbolCode5) != null) ? SymbolCode5 : "");
            RealSymbol.Add((SymbolCode6 != "" && Symbols.GetSymbol(SymbolCode6) != null) ? SymbolCode6 : "");
            RealSymbol.Add((SymbolCode7 != "" && Symbols.GetSymbol(SymbolCode7) != null) ? SymbolCode7 : "");
            RealSymbol.Add((SymbolCode8 != "" && Symbols.GetSymbol(SymbolCode8) != null) ? SymbolCode8 : "");
            RealSymbol.Add((SymbolCode9 != "" && Symbols.GetSymbol(SymbolCode9) != null) ? SymbolCode9 : "");
            RealSymbol.Add((SymbolCode10 != "" && Symbols.GetSymbol(SymbolCode10) != null) ? SymbolCode10 : "");

        }

        /// <summary>
        /// Viene eseguito ad ogni tick
        /// </summary>
        /// <param name="index"></param>
        public override void Calculate(int index)
        {

            // --> Calcolo i valori dei simboli richiesti
            if (RealSymbol[0] == SymbolCode1) _setValue(Symbol1, index, SymbolCode1);
            if (RealSymbol[1] == SymbolCode2) _setValue(Symbol2, index, SymbolCode2);
            if (RealSymbol[2] == SymbolCode3) _setValue(Symbol3, index, SymbolCode3);
            if (RealSymbol[3] == SymbolCode4) _setValue(Symbol4, index, SymbolCode4);
            if (RealSymbol[4] == SymbolCode5) _setValue(Symbol5, index, SymbolCode5);
            if (RealSymbol[5] == SymbolCode6) _setValue(Symbol6, index, SymbolCode6);
            if (RealSymbol[6] == SymbolCode7) _setValue(Symbol7, index, SymbolCode7);
            if (RealSymbol[7] == SymbolCode8) _setValue(Symbol8, index, SymbolCode8);
            if (RealSymbol[8] == SymbolCode9) _setValue(Symbol9, index, SymbolCode9);
            if (RealSymbol[9] == SymbolCode10) _setValue(Symbol10, index, SymbolCode10);

        }

        /// <summary>
        /// Effettua un controllo sul sito ctrader.guru per mezzo delle API per verificare la presenza di aggiornamenti, solo in realtime
        /// </summary>
        private void _checkProductUpdate()
        {

            // --> Controllo solo se solo in realtime, evito le chiamate in backtest
            if (RunningMode != RunningMode.RealTime)
                return;

            // --> Organizzo i dati per la richiesta degli aggiornamenti
            Guru.API.RequestProductInfo Request = new Guru.API.RequestProductInfo
            {

                MyProduct = new Guru.Product
                {

                    ID = ID,
                    Name = NAME,
                    Version = VERSION

                },
                AccountBroker = Account.BrokerName,
                AccountNumber = Account.Number

            };

            // --> Effettuo la richiesta
            Guru.API Response = new Guru.API(Request);

            // --> Controllo per prima cosa la presenza di errori di comunicazioni
            if (Response.ProductInfo.Exception != "")
            {

                Print("{0} Exception : {1}", NAME, Response.ProductInfo.Exception);

            }
            // --> Chiedo conferma della presenza di nuovi aggiornamenti
            else if (Response.HaveNewUpdate())
            {

                string updatemex = string.Format("{0} : Updates available {1} ( {2} )", NAME, Response.ProductInfo.LastProduct.Version, Response.ProductInfo.LastProduct.Updated);

                // --> Informo l'utente con un messaggio sul grafico e nei log del cbot
                Chart.DrawStaticText(NAME + "Updates", updatemex, VerticalAlignment.Top, HorizontalAlignment.Left, Color.Red);
                Print(updatemex);

            }

        }

        /// <summary>
        /// Valorizza la linea nel buffer dell'indicatore
        /// </summary>
        /// <param name="View">Il buffer nell'indice</param>
        /// <param name="index">L'indice della barra corrente</param>
        /// <param name="CROSSSymbol">Il nome della coppia o strumento</param>
        private void _setValue(IndicatorDataSeries View, int index, string CROSSSymbol)
        {

            // --> Alcuni controlli preliminari sul testo
            CROSSSymbol = CROSSSymbol.Trim().ToUpper();
            if (CROSSSymbol == "") return;

            // --> Potrebbe essere uno strumento inesistente
            Symbol CROSS = Symbols.GetSymbol(CROSSSymbol);
            if (CROSS == null) return;

            // --> Prelevo i dati per il symbolo in lavorazione
            MarketSeries CROSSsr = MarketData.GetSeries(CROSS.Code, TimeFrame);
            int index2 = _getIndexByDate(CROSSsr, MarketSeries.OpenTime[index]);

            ExponentialMovingAverage CROSSema = Indicators.ExponentialMovingAverage(CROSSsr.Close, EMAPeriods);

            double CROSSpips = (CROSSsr.Close[index2] - CROSSema.Result[index2]) / CROSS.PipSize;

            View[index] = Math.Round(CROSSpips, 2);


            string CROSStext = string.Format("« {0} {1}", CROSS.Code, Math.Round(View[index], CROSS.Digits));
            
            ChartObjects.DrawText(CROSS.Code, CROSStext, index + 1, View[index], VerticalAlignment.Center, HorizontalAlignment.Right, Colors.Red);

        }

        private int _getIndexByDate(MarketSeries series, DateTime time)
        {
            for (int i = series.Close.Count - 1; i >= 0; i--)
                if (time == series.OpenTime[i])
                    return i;
            return -1;
        }

    }

}

/// <summary>
/// NameSpace che racchiude tutte le feature ctrader.guru
/// </summary>
namespace Guru
{
    /// <summary>
    /// Classe che definisce lo standard identificativo del prodotto nel marketplace ctrader.guru
    /// </summary>
    public class Product
    {

        public int ID = 0;
        public string Name = "";
        public string Version = "";
        public string Updated = "";

    }

    /// <summary>
    /// Offre la possibilità di utilizzare le API messe a disposizione da ctrader.guru per verificare gli aggiornamenti del prodotto.
    /// Permessi utente "AccessRights = AccessRights.FullAccess" per accedere a internet ed utilizzare JSON
    /// </summary>
    public class API
    {
        /// <summary>
        /// Costante da non modificare, corrisponde alla pagina dei servizi API
        /// </summary>
        private const string Service = "https://ctrader.guru/api/product_info/";

        /// <summary>
        /// Costante da non modificare, utilizzata per filtrare le richieste
        /// </summary>
        private const string UserAgent = "cTrader Guru";

        /// <summary>
        /// Variabile dove verranno inserite le direttive per la richiesta
        /// </summary>
        private RequestProductInfo RequestProduct = new RequestProductInfo();

        /// <summary>
        /// Variabile dove verranno inserite le informazioni identificative dal server dopo l'inizializzazione della classe API
        /// </summary>
        public ResponseProductInfo ProductInfo = new ResponseProductInfo();

        /// <summary>
        /// Classe che formalizza i parametri di richiesta, vengono inviate le informazioni del prodotto e di profilazione a fini statistici
        /// </summary>
        public class RequestProductInfo
        {

            /// <summary>
            /// Il prodotto corrente per il quale richiediamo le informazioni
            /// </summary>
            public Product MyProduct = new Product();

            /// <summary>
            /// Broker con il quale effettiamo la richiesta
            /// </summary>
            public string AccountBroker = "";

            /// <summary>
            /// Il numero di conto con il quale chiediamo le informazioni
            /// </summary>
            public int AccountNumber = 0;

        }

        /// <summary>
        /// Classe che formalizza lo standard per identificare le informazioni del prodotto
        /// </summary>
        public class ResponseProductInfo
        {

            /// <summary>
            /// Il prodotto corrente per il quale vengono fornite le informazioni
            /// </summary>
            public Product LastProduct = new Product();

            /// <summary>
            /// Eccezioni in fase di richiesta al server, da utilizzare per controllare l'esito della comunicazione
            /// </summary>
            public string Exception = "";

            /// <summary>
            /// La risposta del server
            /// </summary>
            public string Source = "";

        }

        /// <summary>
        /// Richiede le informazioni del prodotto richiesto
        /// </summary>
        /// <param name="Request"></param>
        public API(RequestProductInfo Request)
        {

            RequestProduct = Request;

            // --> Non controllo se non ho l'ID del prodotto
            if (Request.MyProduct.ID <= 0)
                return;

            // --> Dobbiamo supervisionare la chiamata per registrare l'eccexione
            try
            {

                // --> Strutturo le informazioni per la richiesta POST
                NameValueCollection data = new NameValueCollection
                {
                    {
                        "account_broker",
                        Request.AccountBroker
                    },
                    {
                        "account_number",
                        Request.AccountNumber.ToString()
                    },
                    {
                        "my_version",
                        Request.MyProduct.Version
                    },
                    {
                        "productid",
                        Request.MyProduct.ID.ToString()
                    }
                };

                // --> Autorizzo tutte le pagine di questo dominio
                Uri myuri = new Uri(Service);
                string pattern = string.Format("{0}://{1}/.*", myuri.Scheme, myuri.Host);

                Regex urlRegEx = new Regex(pattern);
                WebPermission p = new WebPermission(NetworkAccess.Connect, urlRegEx);
                p.Assert();

                // --> Protocollo di sicurezza https://
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;

                // -->> Richiedo le informazioni al server
                using (var wb = new WebClient())
                {

                    wb.Headers.Add("User-Agent", UserAgent);

                    var response = wb.UploadValues(myuri, "POST", data);
                    ProductInfo.Source = Encoding.UTF8.GetString(response);

                }

                // -->>> Nel cBot necessita l'attivazione di "AccessRights = AccessRights.FullAccess"
                ProductInfo.LastProduct = JsonConvert.DeserializeObject<Product>(ProductInfo.Source);

            }
            catch (Exception Exp)
            {

                // --> Qualcosa è andato storto, registro l'eccezione
                ProductInfo.Exception = Exp.Message;

            }

        }

        /// <summary>
        /// Esegue un confronto tra le versioni per determinare la presenza di aggiornamenti
        /// </summary>
        /// <returns></returns>
        public bool HaveNewUpdate()
        {

            // --> Voglio essere sicuro che stiamo lavorando con le informazioni giuste
            return (ProductInfo.LastProduct.ID == RequestProduct.MyProduct.ID && ProductInfo.LastProduct.Version != "" && RequestProduct.MyProduct.Version != "" && new Version(RequestProduct.MyProduct.Version).CompareTo(new Version(ProductInfo.LastProduct.Version)) < 0);

        }

    }

}
