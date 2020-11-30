using HighStock.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighStock.ViewModel {
    class StockViewModel : INotifyPropertyChanged {
        private StockInfo si;
        public StockInfo SI {
            get { return this.si; }
            set {
                si = value;
                OnPropertyChanged("SI");
            }
        }
        public StockViewModel(StockInfo mainStockInfo) {
            si = mainStockInfo;
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
}
