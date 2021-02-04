using HighStock.Model;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 Description : STOCK DB를 읽어와 변수화 시켜주는 클래스
     */

namespace HighStock.Type {
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

        public DataRow[] StockInfoDateList(String codeName, int volumeSize, double bullishSize) {
            Console.WriteLine("show StockInfoDateList");
            string selDate = string.Format("Code='{0}' AND (ChagesRatio>'{1}' OR Volume>'{2}')", codeName, bullishSize, volumeSize);
            //string selDate = string.Format("Code='{0}'", codeName);
            DataRow[] dt2 = dt.Select(selDate, "stockDate DESC");
            ShowTable(dt2);
            return dt2;
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
        public void ShowTable(DataRow[] dt2) {
            foreach (DataRow row in dt2) {
                //if (Int32.Parse(row["Volume"].ToString()) < 10000000 && Double.Parse(row["ChagesRatio"].ToString()) < 29) continue; 
                string strRow = row["stockDate"].ToString() + " " +
                                row["Code"].ToString() + " " +
                                row["Name"].ToString() + " " +
                                row["Close"].ToString() + " " +
                                row["Changes"].ToString() + " " +
                                row["ChagesRatio"].ToString() + " " +
                                row["Volume"].ToString() + " " +
                                row["Amount"].ToString() + " " +
                                row["Open"].ToString() + " " +
                                row["High"].ToString() + " " +
                                row["Low"].ToString() + " " +
                                row["Marcap"].ToString() + " " +
                                row["Stocks"].ToString();
                Console.WriteLine(strRow);

            }
        }
    }

}
