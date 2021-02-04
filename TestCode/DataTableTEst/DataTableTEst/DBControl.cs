using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTableTEst {
    class DBControl {
        DataTable dt = new DataTable("Table1");//DataTable 정의
        private int year;
        public DBControl(int year) {
            //Column 정의
            DataColumn stockDate = new DataColumn("stockDate", typeof(DateTime));
            DataColumn Code = new DataColumn("Code", typeof(string));
            DataColumn Name = new DataColumn("Name", typeof(string));
            DataColumn Close = new DataColumn("Close", typeof(int));
            DataColumn Changes = new DataColumn("Changes", typeof(int));
            DataColumn ChagesRatio = new DataColumn("ChagesRatio", typeof(double));
            DataColumn Volume = new DataColumn("Volume", typeof(int));
            DataColumn Amount = new DataColumn("Amount", typeof(string));
            DataColumn Open = new DataColumn("Open", typeof(int));
            DataColumn High = new DataColumn("High", typeof(int));
            DataColumn Low = new DataColumn("Low", typeof(int));
            DataColumn Marcap = new DataColumn("Marcap", typeof(string));
            DataColumn Stocks = new DataColumn("Stocks", typeof(string));

            dt.Columns.Add(stockDate);
            dt.Columns.Add(Code);
            dt.Columns.Add(Name);
            dt.Columns.Add(Close);
            dt.Columns.Add(Changes);
            dt.Columns.Add(ChagesRatio);
            dt.Columns.Add(Volume);
            dt.Columns.Add(Amount);
            dt.Columns.Add(Open);
            dt.Columns.Add(High);
            dt.Columns.Add(Low);
            dt.Columns.Add(Marcap);
            dt.Columns.Add(Stocks);

            this.year = year;
            ReadPastStockInfo(year);

        }


        void ReadPastStockInfo(int year) {
            string totalPath = "./StockDB\\" + year + ".txt";

            String[] textValue;
            textValue = System.IO.File.ReadAllLines(totalPath);

            if (textValue.Length > 0) {
                for (int i = 0; i < textValue.Length; i++) {
                    if (String.IsNullOrWhiteSpace(textValue[i])) continue;
                    String[] tmpValue = textValue[i].Split('^');
                    dt.Rows.Add(
                        DateTime.ParseExact(tmpValue[0], "yyyy-MM-dd", null),
                        tmpValue[1],
                        tmpValue[2],
                        Int32.Parse(tmpValue[3]),
                        Int32.Parse(tmpValue[4]),
                        Double.Parse(tmpValue[5]),
                        Int32.Parse(tmpValue[6]),
                        //(tmpValue[7].Length < 4) ? Int32.Parse(tmpValue[7]) : Int32.Parse(tmpValue[7].Substring(0, tmpValue[11].Length - 4)), //amount
                        //ValueChangeUnit(tmpValue[7]),
                        tmpValue[7],
                        Int32.Parse(tmpValue[8]),
                        Int32.Parse(tmpValue[9]),
                        Int32.Parse(tmpValue[10]),
                        //(tmpValue[11].Length < 7) ? Int32.Parse(tmpValue[11]) : Int32.Parse(tmpValue[11].Substring(0, tmpValue[11].Length - 7)), //marcap
                        //ValueChangeUnit(tmpValue[11]),
                        tmpValue[11],
                        //(tmpValue[12].Length < 4) ? Int32.Parse(tmpValue[12]) : Int32.Parse(tmpValue[12].Substring(0, tmpValue[11].Length - 4)) //stocks
                        //ValueChangeUnit(tmpValue[12])
                        tmpValue[12]
                        );
                    //(tmpValue[11] == "0") ? Int32.Parse(tmpValue[11]) : Int32.Parse(tmpValue[11].Substring(0, tmpValue[11].Length - 7)), //marcap
                }
                Console.WriteLine("done : " + textValue.Length);
            }

        }
        public StockDate GetSelectStockDate(DateTime selDateTime, int volumeSize, double bullishSize) {
            ObservableCollection<StockInfo> StockVolume = new ObservableCollection<StockInfo>();
            ObservableCollection<StockInfo> StockBulish = new ObservableCollection<StockInfo>();
            string selDate = string.Format("stockDate='{0}'", selDateTime.ToString());
            DataRow[] dt2 = dt.Select(selDate);
            foreach (DataRow i in dt2) {
                if (i["stockDate"].ToString() == selDateTime.ToString()) {
                    if (Int32.Parse(i["Volume"].ToString()) > volumeSize) {
                        StockVolume.Add(new StockInfo() {
                            StockCode = i["Code"].ToString(),
                            StockName = i["Name"].ToString(),
                            StockClose = Int32.Parse(i["Close"].ToString()),
                            StockChanges = Int32.Parse(i["Changes"].ToString()),
                            StockChagesRatio = Double.Parse(i["ChagesRatio"].ToString()),
                            StockVolume = Int32.Parse(i["Volume"].ToString()),
                            StockAmount = i["Amount"].ToString(),
                            StockOpen = Int32.Parse(i["Open"].ToString()),
                            StockHigh = Int32.Parse(i["High"].ToString()),
                            StockLow = Int32.Parse(i["Low"].ToString()),
                            StockMarcap = i["Marcap"].ToString(),
                            StockStocks = i["Stocks"].ToString()
                        });
                    }

                    if (double.Parse(i["ChagesRatio"].ToString()) > bullishSize) {
                        StockBulish.Add(new StockInfo() {
                            StockCode = i["Code"].ToString(),
                            StockName = i["Name"].ToString(),
                            StockClose = Int32.Parse(i["Close"].ToString()),
                            StockChanges = Int32.Parse(i["Changes"].ToString()),
                            StockChagesRatio = Double.Parse(i["ChagesRatio"].ToString()),
                            StockVolume = Int32.Parse(i["Volume"].ToString()),
                            StockAmount = i["Amount"].ToString(),
                            StockOpen = Int32.Parse(i["Open"].ToString()),
                            StockHigh = Int32.Parse(i["High"].ToString()),
                            StockLow = Int32.Parse(i["Low"].ToString()),
                            StockMarcap = i["Marcap"].ToString(),
                            StockStocks = i["Stocks"].ToString()
                        });
                    }
                }
            }
            if (StockVolume.Count == 0 && StockBulish.Count == 0) return null;
            if (StockVolume.Count != 0) {
                var tmpVolume = StockVolume.OrderByDescending(x => x.StockVolume).ToList();
                StockVolume.Clear();
                int index = 0;
                foreach (var i in tmpVolume) {
                    i.Index = index++;
                    StockVolume.Add(i);
                }
            }
            if (StockBulish.Count != 0) {
                var tmpBulish = StockBulish.OrderByDescending(x => x.StockChagesRatio).ToList();
                StockBulish.Clear();
                int index = 0;
                foreach (var i in tmpBulish) {

                    i.Index = index++;
                    Console.WriteLine("{0} {1} {2} {3}", i.Index, i.StockName, i.ShowVolume, i.StockClose);
                    StockBulish.Add(i);
                }
            }
            return new StockDate(selDateTime, StockVolume, StockBulish);

        }
    }
}
