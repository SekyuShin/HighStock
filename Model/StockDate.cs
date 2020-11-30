using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighStock.Model {
    public class StockDate {

        public DateTime Date { get; set; }
        public String StrDate { get {return Date.ToString("yyyy-MM-dd"); } }
        public ObservableCollection<StockInfo> StockVolume { get; set; } //거래량 주식
        public ObservableCollection<StockInfo> StockBullish { get; set; } //상승주
        public StockDate() {
            StockVolume = new ObservableCollection<StockInfo>();
            StockBullish = new ObservableCollection<StockInfo>();
        }
        //public override string ToString() {
        //    return Date.ToString("yyyy-MM-dd");
        //}
        public StockDate(DateTime Date, ObservableCollection<StockInfo> sv, ObservableCollection<StockInfo> sb) {
            this.Date = Date;
            StockVolume = sv;
            StockBullish = sb;
        }
    }
}
