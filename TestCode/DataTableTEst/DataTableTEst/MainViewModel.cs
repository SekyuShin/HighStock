using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTableTEst {
    class MainViewModel {
        DataTable dt = new DataTable("Table1");//DataTable 정의

        public MainViewModel() {
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

            ReadPastStockInfo(2020);
            DateTime now = DateTime.ParseExact("2020-01-02", "yyyy-MM-dd", null);
            string strTest = string.Format("stockDate='{0}'", now.ToString());
            DataRow[] dt2 = dt.Select(strTest);
            ShowTable(dt2);
            Console.WriteLine("test");
            strTest = string.Format("Code='5930'", now.ToString());
            dt2 = dt.Select(strTest);
            ShowTable(dt2);
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
                Console.WriteLine("done : "  +  textValue.Length);
            }

        }


        public void ShowTable(DataRow[] dt2) {
            foreach(DataRow row in dt2) {
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
