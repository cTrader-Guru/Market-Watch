/*  CTRADER GURU --> Indicator Template 1.0.8

    Homepage    : https://ctrader.guru/
    Telegram    : https://t.me/ctraderguru
    Twitter     : https://twitter.com/cTraderGURU/
    Facebook    : https://www.facebook.com/ctrader.guru/
    YouTube     : https://www.youtube.com/channel/UCKkgbw09Fifj65W5t5lHeCQ
    GitHub      : https://github.com/ctrader-guru

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

namespace cAlgo
{

    // --> Estensioni che rendono il codice più leggibile
    #region Extensions

    /// <summary>
    /// Estensione che fornisce metodi aggiuntivi per il simbolo
    /// </summary>
    public static class SymbolExtensions
    {

        /// <summary>
        /// Converte il numero di pips corrente da digits a double
        /// </summary>
        /// <param name="Pips">Il numero di pips nel formato Digits</param>
        /// <returns></returns>
        public static double DigitsToPips(this Symbol MySymbol, double Pips)
        {

            return Math.Round(Pips / MySymbol.PipSize, 2);

        }

        /// <summary>
        /// Converte il numero di pips corrente da double a digits
        /// </summary>
        /// <param name="Pips">Il numero di pips nel formato Double (2)</param>
        /// <returns></returns>
        public static double PipsToDigits(this Symbol MySymbol, double Pips)
        {

            return Math.Round(Pips * MySymbol.PipSize, MySymbol.Digits);

        }

    }

    /// <summary>
    /// Estensione che fornisce metodi aggiuntivi per le Bars
    /// </summary>
    public static class BarsExtensions
    {

        /// <summary>
        /// Converte l'indice di una bar partendo dalla data di apertura
        /// </summary>
        /// <param name="MyTime">La data e l'ora di apertura della candela</param>
        /// <returns></returns>
        public static int GetIndexByDate(this Bars MyBars, DateTime MyTime)
        {

            for (int i = MyBars.ClosePrices.Count - 1; i >= 0; i--)
            {

                if (MyTime == MyBars.OpenTimes[i]) return i;

            }

            return -1;

        }

    }

    #endregion

    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    [Levels(0)]
    public class MarketWatch : Indicator
    {

        #region Enums

        public class SymbolData
        {

            public string Name = string.Empty;
            public double Pips = double.NaN;
            public double Price = double.NaN;

        }

        public enum MyColors
        {

            AliceBlue,
            AntiqueWhite,
            Aqua,
            Aquamarine,
            Azure,
            Beige,
            Bisque,
            Black,
            BlanchedAlmond,
            Blue,
            BlueViolet,
            Brown,
            BurlyWood,
            CadetBlue,
            Chartreuse,
            Chocolate,
            Coral,
            CornflowerBlue,
            Cornsilk,
            Crimson,
            Cyan,
            DarkBlue,
            DarkCyan,
            DarkGoldenrod,
            DarkGray,
            DarkGreen,
            DarkKhaki,
            DarkMagenta,
            DarkOliveGreen,
            DarkOrange,
            DarkOrchid,
            DarkRed,
            DarkSalmon,
            DarkSeaGreen,
            DarkSlateBlue,
            DarkSlateGray,
            DarkTurquoise,
            DarkViolet,
            DeepPink,
            DeepSkyBlue,
            DimGray,
            DodgerBlue,
            Firebrick,
            FloralWhite,
            ForestGreen,
            Fuchsia,
            Gainsboro,
            GhostWhite,
            Gold,
            Goldenrod,
            Gray,
            Green,
            GreenYellow,
            Honeydew,
            HotPink,
            IndianRed,
            Indigo,
            Ivory,
            Khaki,
            Lavender,
            LavenderBlush,
            LawnGreen,
            LemonChiffon,
            LightBlue,
            LightCoral,
            LightCyan,
            LightGoldenrodYellow,
            LightGray,
            LightGreen,
            LightPink,
            LightSalmon,
            LightSeaGreen,
            LightSkyBlue,
            LightSlateGray,
            LightSteelBlue,
            LightYellow,
            Lime,
            LimeGreen,
            Linen,
            Magenta,
            Maroon,
            MediumAquamarine,
            MediumBlue,
            MediumOrchid,
            MediumPurple,
            MediumSeaGreen,
            MediumSlateBlue,
            MediumSpringGreen,
            MediumTurquoise,
            MediumVioletRed,
            MidnightBlue,
            MintCream,
            MistyRose,
            Moccasin,
            NavajoWhite,
            Navy,
            OldLace,
            Olive,
            OliveDrab,
            Orange,
            OrangeRed,
            Orchid,
            PaleGoldenrod,
            PaleGreen,
            PaleTurquoise,
            PaleVioletRed,
            PapayaWhip,
            PeachPuff,
            Peru,
            Pink,
            Plum,
            PowderBlue,
            Purple,
            Red,
            RosyBrown,
            RoyalBlue,
            SaddleBrown,
            Salmon,
            SandyBrown,
            SeaGreen,
            SeaShell,
            Sienna,
            Silver,
            SkyBlue,
            SlateBlue,
            SlateGray,
            Snow,
            SpringGreen,
            SteelBlue,
            Tan,
            Teal,
            Thistle,
            Tomato,
            Transparent,
            Turquoise,
            Violet,
            Wheat,
            White,
            WhiteSmoke,
            Yellow,
            YellowGreen

        }

        #endregion

        #region Identity

        /// <summary>
        /// Nome del prodotto, identificativo, da modificare con il nome della propria creazione
        /// </summary>
        public const string NAME = "Market Watch";

        /// <summary>
        /// La versione del prodotto, progressivo, utilie per controllare gli aggiornamenti se viene reso disponibile sul sito ctrader.guru
        /// </summary>
        public const string VERSION = "1.0.3";

        #endregion

        #region Params

        /// <summary>
        /// Identità del prodotto nel contesto di ctrader.guru
        /// </summary>
        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://ctrader.guru/product/market-watch/")]
        public string ProductInfo { get; set; }

        [Parameter("EMA Period", Group = "Params", DefaultValue = 500)]
        public int MyEMAPeriod { get; set; }

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

        [Parameter("Label Trigger Hover", Group = "Styles", DefaultValue = 2.5, MinValue = 0, Step = 0.5)]
        public double LabelSense { get; set; }

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

        #endregion

        #region Property

        List<SymbolData> AllSymbols = new List<SymbolData>();
        bool CanDraw;

        #endregion

        #region Indicator Events

        /// <summary>
        /// Viene generato all'avvio dell'indicatore, si inizializza l'indicatore
        /// </summary>
        protected override void Initialize()
        {

            // --> Stampo nei log la versione corrente
            Print("{0} : {1}", NAME, VERSION);

            CanDraw = (RunningMode == RunningMode.RealTime || RunningMode == RunningMode.VisualBacktesting);

            // --> Controllo la presenza di simboli non validi
            SymbolCode1 = SymbolCode1.Trim().ToUpper();
            SymbolCode2 = SymbolCode2.Trim().ToUpper();
            SymbolCode3 = SymbolCode3.Trim().ToUpper();
            SymbolCode4 = SymbolCode4.Trim().ToUpper();
            SymbolCode5 = SymbolCode5.Trim().ToUpper();
            SymbolCode6 = SymbolCode6.Trim().ToUpper();
            SymbolCode7 = SymbolCode7.Trim().ToUpper();
            SymbolCode8 = SymbolCode8.Trim().ToUpper();
            SymbolCode9 = SymbolCode9.Trim().ToUpper();
            SymbolCode10 = SymbolCode10.Trim().ToUpper();

            // --> Handle per la visualizzazione del cross
            Chart.IndicatorAreas[0].MouseMove += _mouseOnChart;

        }

        /// <summary>
        /// Generato ad ogni tick, vengono effettuati i calcoli dell'indicatore
        /// </summary>
        /// <param name="index">L'indice della candela in elaborazione</param>
        public override void Calculate(int index)
        {

            // --> Resetto la lista
            AllSymbols.RemoveAll(item => true);

            // --> Calcolo i valori dei simboli richiesti
            AllSymbols.Add(_setValue(SymbolCode1, index, Symbol1));

            AllSymbols.Add(_setValue(SymbolCode2, index, Symbol2));

            AllSymbols.Add(_setValue(SymbolCode3, index, Symbol3));

            AllSymbols.Add(_setValue(SymbolCode4, index, Symbol4));

            AllSymbols.Add(_setValue(SymbolCode5, index, Symbol5));

            AllSymbols.Add(_setValue(SymbolCode6, index, Symbol6));

            AllSymbols.Add(_setValue(SymbolCode7, index, Symbol7));

            AllSymbols.Add(_setValue(SymbolCode8, index, Symbol8));

            AllSymbols.Add(_setValue(SymbolCode9, index, Symbol9));

            AllSymbols.Add(_setValue(SymbolCode10, index, Symbol10));

        }

        #endregion

        #region Private Methods
        
        private void _mouseOnChart( ChartMouseEventArgs eventArgs ) {

            SymbolData hoveredItem = AllSymbols.Find( item => (item.Pips != double.NaN && eventArgs.YValue > item.Pips - LabelSense && eventArgs.YValue < item.Pips + LabelSense) );
            

            if( CanDraw)
            {

                string label = "LabelCross";
                string labelLine = "LabelCrossLine";

                if (hoveredItem != null)
                {

                    if (hoveredItem.Name != string.Empty)
                    {

                        string CROSStext = string.Format("   {0} {1:0.00} ( {2:0.00000} )", hoveredItem.Name, hoveredItem.Pips, hoveredItem.Price);
                        Chart.IndicatorAreas[0].DrawText(label, CROSStext, Bars.OpenTimes.LastValue, hoveredItem.Pips, Color.Gray);
                        Chart.IndicatorAreas[0].DrawTrendLine(labelLine,Bars.OpenTimes.LastValue,hoveredItem.Pips,eventArgs.TimeValue,hoveredItem.Pips, Color.Gray,1,LineStyle.DotsRare);

                    }
                    else
                    {

                        Chart.IndicatorAreas[0].RemoveObject(label);
                        Chart.IndicatorAreas[0].RemoveObject(labelLine);

                    }

                }
                else
                {

                    Chart.IndicatorAreas[0].RemoveObject(label);
                    Chart.IndicatorAreas[0].RemoveObject(labelLine);

                }
                
            }

        }

        private SymbolData _setValue(string MySymbol, int index, IndicatorDataSeries Result)
        {

            // --> Si esce se non ci sono le condizioni per continuare
            if (!Symbols.Exists(MySymbol)) return new SymbolData();


            Symbol CROSS = Symbols.GetSymbol(MySymbol);
            Bars CROSS_Bars = MarketData.GetBars(TimeFrame, CROSS.Name);

            // --> Potrei avere un indice diverso perchè non carico le stesse barre
            int CROSS_Index = CROSS_Bars.GetIndexByDate(Bars.OpenTimes[index]);
            if (CROSS_Index < 0) return new SymbolData();

            ExponentialMovingAverage CROSS_ema = Indicators.ExponentialMovingAverage(CROSS_Bars.ClosePrices, MyEMAPeriod);
            ExponentialMovingAverage Current_CROSS_ema = Indicators.ExponentialMovingAverage(Bars.ClosePrices, MyEMAPeriod);

            double CROSSpips = 0;

            // --> Devo uniformare il numero di pips, i digits saranno di sicuro diversi
            CROSSpips = CROSS.DigitsToPips(Math.Round(CROSS_Bars.ClosePrices[CROSS_Index] - CROSS_ema.Result[CROSS_Index], CROSS.Digits));
            Result[index] = CROSSpips;

            SymbolData response = new SymbolData
            {
                Name = MySymbol,
                Price = Result[index],
                Pips = CROSSpips
            };

            return response;

        }

        #endregion

    }

}
