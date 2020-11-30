using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    public class TestPastStockInfo {
        public DateTime stockDate;
        public String Code; //: 종목코드
        public String Name; //: 종목이름
        public int Close; //: 종가
        public int Changes; //: 전일대비
        public double ChagesRatio; //: 전일비
        public int Volume;//: 거래량
        public String Amount; //: 거래대금
        public int Open; //: 시가
        public int High; //: 고가
        public int Low; //: 저가
        public String Marcap;//: 시가총액
        public String Stocks;//: 상장주식수
        public TestPastStockInfo(DateTime stockDate, String Code, String Name, int Close, int Changes, double ChagesRatio, int Volume, String Amount, int Open, int High, int Low, String Marcap, String Stocks) {
            this.stockDate = stockDate;
            this.Code = Code;
            this.Name = Name;
            this.Close = Close;
            this.Changes = Changes;
            this.ChagesRatio = ChagesRatio;
            this.Volume = Volume;
            this.Amount = Amount;
            this.Open = Open;
            this.High = High;
            this.Low = Low;
            this.Marcap = Marcap;
            this.Stocks = Stocks;
        }


    }
}
