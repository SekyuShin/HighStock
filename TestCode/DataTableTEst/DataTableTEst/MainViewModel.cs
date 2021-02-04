using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTableTEst {
    class MainViewModel {
        DBControl dbcontrol;
        private ObservableCollection<StockDate> stockDates = new ObservableCollection<StockDate>();
        public ObservableCollection<StockDate> StockDates {
            set { stockDates = value; }
            get { return stockDates; }
        }
        public MainViewModel() {
            dbcontrol = new DBControl(2020);
            StockDates.Add(dbcontrol.GetSelectStockDate(DateTime.ParseExact("2020-11-20", "yyyy-MM-dd", null), 10000000, 29));
        }
        

    }
}
