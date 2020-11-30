using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighStock.Model {
    public class StockInfo {
        public int Index { get; set; }
        public String StockCode { get; set; }
        public String StockName { get; set; }                                        //종목명
        public int StockClose { get; set; }                                          //현재가격 
        public String ShowClose { get { return String.Format("{0:#,0}", StockClose); } }                                          //현재가격 
        public int StockChanges { get; set; }                                          //전일대비
        public String ShowChanges { get { return String.Format("{0:#,0}", StockChanges); } }
        public double StockChagesRatio { get; set; }                               //전일비
        public int StockVolume { get; set; }                                    //거래량
        public String ShowVolume { get { return String.Format("{0:#,0}", StockVolume); } }
        public String StockAmount { get; set; }                                    //거래대금
        public String ShowAmount { get { return inputDigitComma(StockAmount); } }
        public int StockOpen { get; set; }                                    //시가
        public String ShowOpen { get { return String.Format("{0:#,0}", StockOpen); } }
        public int StockHigh { get; set; }                                    //고가
        public String ShowHigh { get { return String.Format("{0:#,0}", StockHigh); } }
        public int StockLow { get; set; }                                    //저가
        public String ShowLow { get { return String.Format("{0:#,0}", StockLow); } }
        public String StockMarcap { get; set; }                                     //시가총액
        public String ShowMarcap { get { return inputDigitComma(StockMarcap); } }
        public String StockStocks { get; set; }                                     //상장주식수
        public String ShowStocks { get { return inputDigitComma(StockStocks);  } }
        public ObservableCollection<StockInfoArticles> StockArticles { get; set; }   //종목기사
        public ObservableCollection<StockInfoArticles> StockDarts { get; set; }      //종목공시
        public ObservableCollection<StockInfoChart> StockDateChart { get; set; }     //종목일봉
        public ObservableCollection<StockInfoChart> StockThirtyMChart { get; set; }  //종목10분봉
        public StockInfo() {
            //StockArticles = new ObservableCollection<StockInfoArticles>();
            //StockDarts = new ObservableCollection<StockInfoArticles>();
            //StockDateChart = new ObservableCollection<StockInfoChart>();
            //StockThirtyMChart = new ObservableCollection<StockInfoChart>();
        }
        public StockInfo(StockInfo a) {
            //StockArticles = new ObservableCollection<StockInfoArticles>();
            //StockDarts = new ObservableCollection<StockInfoArticles>();
            //StockDateChart = new ObservableCollection<StockInfoChart>();
            //StockThirtyMChart = new ObservableCollection<StockInfoChart>();
        }
        public override string ToString() {
            return StockName;
        }
        public String inputDigitComma(String str) {
            int len = str.Length;
            int count = 0;
            for (int i = len - 1; i >0; i--) {
                if (count != 2) count++;
                else {
                    count = 0;
                    str = str.Insert(i, ",");
                }
            }
            return str;
        }
    }
}
