using HighStock.Model;
using HighStock.Type;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighStock.ViewModel {
    class StockViewModel : INotifyPropertyChanged {

        private ObservableCollection<DateList> stockInfoDateList = new ObservableCollection<DateList>();
        public ObservableCollection<DateList> StockInfoDateList {
            set { stockInfoDateList = value; }
            get { return stockInfoDateList; }
        }
        private StockInfo si;
        public StockInfo SI {
            get { return this.si; }
            set {
                si = value;
                OnPropertyChanged("SI");
            }
        }
        public StockViewModel(StockInfo mainStockInfo, DataRow[] tmpDateList) {
            si = mainStockInfo;

            foreach (DataRow row in tmpDateList) {
                DateList tmp = new DateList(); //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>??????????????????????
                tmp.stockDate = row["stockDate"].ToString();
                tmp.Close = row["Close"].ToString();
                tmp.ChagesRatio = row["ChagesRatio"].ToString();
                tmp.Volume = row["Volume"].ToString() ;
                StockInfoDateList.Add(tmp);
                StockInfoDateList.OrderByDescending(x => x.stockDate );
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            switch (propertyName) {
                case "SI":
                    
                    break;
            }
        }
    }
    public class DateList {
        public string stockDate { get; set; }
        public string Close { get; set; }
        public String ShowClose { get { return inputDigitComma(Close); } }
        public string ChagesRatio { get; set; }
        public string Volume { get; set; }
        public String ShowVolume { get { return inputDigitComma(Volume); } }
        public String inputDigitComma(String str) {
            int len = str.Length;
            int count = 0;
            for (int i = len - 1; i > 0; i--) {
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
