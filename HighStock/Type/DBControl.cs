using HighStock.Model;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighStock.Type {
    class DBControl {
        List<TestPastStockInfo> si = new List<TestPastStockInfo>();

        public DBControl(int year) {

            ReadPastStockInfo(year);
            //StockDate sd = GetSelectStockDate(DateTime.ParseExact("2020-01-08", "yyyy-MM-dd", null), 10000000, 29);
            //ShowTest(sd);
            //python test
        }

        void ReadPastStockInfo(int year) {
            string totalPath = "./StockDB\\" + year+".txt";
            String[] textValue;
            textValue = System.IO.File.ReadAllLines(totalPath);
            if (textValue.Length > 0) {
                for (int i = 0; i < textValue.Length; i++) {
                    if (String.IsNullOrWhiteSpace(textValue[i])) continue;
                    String[] tmpValue = textValue[i].Split('^');
                    si.Add(new TestPastStockInfo(
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
                        ));
                    //(tmpValue[11] == "0") ? Int32.Parse(tmpValue[11]) : Int32.Parse(tmpValue[11].Substring(0, tmpValue[11].Length - 7)), //marcap
                }
                Console.WriteLine("done : " + si.Count + ", " + textValue.Length);
            }

        }

        public StockDate GetSelectStockDate(DateTime dt, int volumeSize, double bullishSize) {
            ObservableCollection<StockInfo> StockVolume = new ObservableCollection<StockInfo>();
            ObservableCollection<StockInfo> StockBulish = new ObservableCollection<StockInfo>();
            foreach (var i in si) {
                if (i.stockDate.CompareTo(dt) == 0) {
                    if (i.Volume > volumeSize) {
                        StockVolume.Add(new StockInfo() {
                            StockCode = i.Code,
                            StockName = i.Name,
                            StockClose = i.Close,
                            StockChanges = i.Changes,
                            StockChagesRatio = i.ChagesRatio,
                            StockVolume = i.Volume,
                            StockAmount = i.Amount,
                            StockOpen = i.Open,
                            StockHigh = i.High,
                            StockLow = i.Low,
                            StockMarcap = i.Marcap,
                            StockStocks = i.Stocks
                        });
                    }

                    if (i.ChagesRatio > bullishSize) {
                        StockBulish.Add(new StockInfo() {
                            StockCode = i.Code,
                            StockName = i.Name,
                            StockClose = i.Close,
                            StockChanges = i.Changes,
                            StockChagesRatio = i.ChagesRatio,
                            StockVolume = i.Volume,
                            StockAmount = i.Amount,
                            StockOpen = i.Open,
                            StockHigh = i.High,
                            StockLow = i.Low,
                            StockMarcap = i.Marcap,
                            StockStocks = i.Stocks
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
                    StockBulish.Add(i);
                }
            }
            return new StockDate(dt,StockVolume, StockBulish);

        }
        //public ObservableCollection<StockDate> GetTotalStockDate(int volumeSize, double bullishSize) {

        //    DateTime dt = DateTime.ParseExact("1995-05-01", "yyyy-MM-dd", null);
            
        //    return sd;

        //}

        String ValueChangeUnit(String value) { //단위 일, 천, 백만, 십억, 일조 
            String rValue = "";
            int len = value.Length;
            if (len < 7) { // 일, 천
                if (len < 4) {
                    rValue = value + "(일원)";
                } else {
                    rValue = value.Substring(0, len - 3) + "," + value.Substring(len - 3, 3) + "(일원)"; //.1
                }
            } else if (len > 3 && len < 10) { //천, 백만
                rValue = value.Substring(0, len - 6) + "," + value.Substring(len - 6, 3) + "(일천)";
            } else if (len > 6 && len < 13) { // 백만, 십억
                rValue = value.Substring(0, len - 9) + "," + value.Substring(len - 9, 3) + "(백만)";
            } else if (len > 9 && len < 17) { // 십억, 일조
                rValue = value.Substring(0, len - 12) + "," + value.Substring(len - 12, 3) + "(십억)";
            } else {
                rValue = value;
            }

            return rValue;
        }
    }
}
