using HighStock.Model;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 Description : STOCK DB를 읽어와 변수화 시켜주는 클래스
     */

namespace HighStock.Type {
    class DBControl {
        List<TestPastStockInfo> si = new List<TestPastStockInfo>();
        private int year;
        public DBControl(int year) {
            Console.WriteLine("Test start");
            this.year = year;
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
                    Console.WriteLine("{0} {1} {2} {3}", i.Index, i.StockName, i.ShowVolume, i.StockClose);
                    StockBulish.Add(i);
                }
            }
            return new StockDate(dt,StockVolume, StockBulish);

        }

        String ValueChangeUnit(String value) { 
            //단위 일, 천, 백만, 십억, 일조 //단위가 int형 또는 long형의 표현 범위를 넘어가는 경우가 있어서 만들었지만 귀찮아서 string으로 전부 넘김 사용은 안하지만 혹시몰라 보존중
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
        public int GetYear() {
            return year;
        }
    }
}
