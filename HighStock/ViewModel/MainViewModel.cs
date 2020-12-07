using HighStock.Model;
using HighStock.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Diagnostics;
using HighStock.Type;
using System.Globalization;

namespace HighStock.ViewModel
{
    class MainViewModel : INotifyPropertyChanged {
        DBControl dbcontrol;
        SettingInfo settingInfo = new SettingInfo() { VolumeValue = 100, BullishValue = 20 };
        private ObservableCollection<StockDate> stockDates = new ObservableCollection<StockDate>();
        public ObservableCollection<StockDate> StockDates {
            set { stockDates = value; }
            get { return stockDates; }
        }
        public MainViewModel() {
            int reProcess = RunProcess("StockDB\\SeachingStockData.exe");
            if (reProcess == 0) MessageBox.Show("DB Update success");
            else MessageBox.Show("DB Update fail");
            dbcontrol = new DBControl(2020);
            StockDates.Add(dbcontrol.GetSelectStockDate(DateTime.ParseExact("2020-11-20", "yyyy-MM-dd", null), 10000000, 29));
            //StockDates=dbcontrol.GetTotalStockDate( 10000000, 29);

        }

        
   
        //TempSetting
        SettingStockInfo settingStockInfo = new SettingStockInfo("000","0", "0", "0", "0","000", "0", "0", "0");

        private String txtDateData; //날짜입력 text창
        public String TxtDateData {
            get { return this.txtDateData; }
            set {
                txtDateData = value;
                OnPropertyChanged("TxtDateData");
            }
        }

        private StockDate selectStockDate;
        public StockDate SelectStockDate {
            get { return this.selectStockDate; }
            set {
                selectStockDate = value;
                OnPropertyChanged("SelectStockDate");
            }
        }
        private StockInfo selectStockInfo;
        public StockInfo SelectStockInfo {
            get { return this.selectStockInfo; }
            set {
                selectStockInfo = value;
                OnPropertyChanged("SelectStockInfo");
            }
        }

        private int RunProcess(String FileName) {
            Process p = new Process();

            p.StartInfo.FileName = FileName;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

            p.Start();
            p.WaitForExit();

            return p.ExitCode;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            switch(propertyName) {
                case "SelectStockDate":
                    TxtDateData = selectStockDate.StrDate;
                    Console.WriteLine("selcetDate : " + selectStockDate.StrDate);
                    selectStockInfo = null;
                    // 나중에 위치 이동 예정 delete
                    break;
                case "SelectStockInfo":
                    //Console.WriteLine("SelectStockInfo : "+ SelectStockInfo.StockName);
                    if (selectStockInfo != null) {
                        var stockWindow = new StockWindow();
                        var stockViewModel = new StockViewModel(selectStockInfo);//
                        stockWindow.DataContext = stockViewModel;
                        stockWindow.ShowDialog();
                    }
                    break;
            }
        }

        private ICommand settingCommand;
        public ICommand SettingCommand {
            get { return (this.settingCommand) ?? (this.settingCommand = new DelegateCommand(Setting)); }
        }
        
        private void Setting() {
            Console.WriteLine("1. Setting : "+settingInfo.BullishValue +", "+ settingInfo.VolumeValue);
            var settingDialog = new SettingDialog();
            var settingViewModel = new SettingViewModel(settingInfo);
            settingDialog.DataContext = settingViewModel;
            settingDialog.ShowDialog();

            Console.WriteLine("1. Setting : " + settingInfo.BullishValue + ", " + settingInfo.VolumeValue);
        }
        private ICommand inputDateCommand; // 날짜 입력 command
        public ICommand InputDateCommand {
            get { return (this.inputDateCommand) ?? (this.inputDateCommand = new DelegateCommand(InputDate)); }
        }

        private void InputDate() {
            String tmpTxtDate = null;
            if (txtDateData == null) {
                MessageBox.Show("please input Date");
                return;
            } else {
                if(txtDateData.Contains("-") && txtDateData[4] == '-' && txtDateData[7] == '-') {
                    tmpTxtDate = txtDateData;
                }else if(txtDateData.Length == 8) {
                    int chknum = 0;
                    bool isnum = int.TryParse(txtDateData, out chknum);
                    if(isnum) {
                        tmpTxtDate = txtDateData;
                        tmpTxtDate = tmpTxtDate.Insert(6, "-");
                        tmpTxtDate = tmpTxtDate.Insert(4, "-");
                    }
                } else {     
                    MessageBox.Show("날짜 형식이 잘못되었습니다.");
                    return;
                }
            }
            foreach (var i in stockDates) {
                if(i.StrDate == tmpTxtDate) {
                    SelectStockDate = i;
                    return;
                }
            }

            StockDate sd = dbcontrol.GetSelectStockDate(DateTime.ParseExact(tmpTxtDate, "yyyy-MM-dd", null), 10000000, 29);
            if (sd == null) {
                MessageBox.Show("DB가 존재하지 않습니다. 날짜를 확인하세요.");
                return;
            } else StockDates.Add(sd);
            SelectStockDate = sd;


        }
        //private ICommand showStockWindowCommand; // SHowStockWindow
        //public ICommand ShowStockWindowCommand {
        //    get { return (this.showStockWindowCommand) ?? (this.showStockWindowCommand = new DelegateCommand(ShowStockWindow)); }
        //}
        //private void ShowStockWindow() {
        //    var stockWindow = new StockWindow();
        //    var stockViewModel = new StockViewModel(stockinfo);//
        //    stockWindow.DataContext = stockViewModel;
        //    stockWindow.ShowDialog();
        //}

        
        
    }
    #region DelegateCommand Class
    public class DelegateCommand : ICommand
    {

        private readonly Func<bool> canExecute;
        private readonly Action execute;

        /// <summary>
        /// Initializes a new instance of the DelegateCommand class.
        /// </summary>
        /// <param name="execute">indicate an execute function</param>
        public DelegateCommand(Action execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DelegateCommand class.
        /// </summary>
        /// <param name="execute">execute function </param>
        /// <param name="canExecute">can execute function</param>
        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        /// <summary>
        /// can executes event handler
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// implement of icommand can execute method
        /// </summary>
        /// <param name="o">parameter by default of icomand interface</param>
        /// <returns>can execute or not</returns>
        public bool CanExecute(object o)
        {
            if (this.canExecute == null)
            {
                return true;
            }
            return this.canExecute();
        }

        /// <summary>
        /// implement of icommand interface execute method
        /// </summary>
        /// <param name="o">parameter by default of icomand interface</param>
        public void Execute(object o)
        {
            this.execute();
        }

        /// <summary>
        /// raise ca excute changed when property changed
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
    #endregion

    //public class Test : ObservableCollection<Score> {
    //    public Test() {
    //        Add(new Score() { SUBJECT = "Englsh", SCORE = 95 });
    //        Add(new Score() { SUBJECT = "Mathmatics", SCORE = 55 });
    //        Add(new Score() { SUBJECT = "History", SCORE = 65 });
    //    }
    //}
    //public class Score {
    //    public String SUBJECT { get; set; }
    //    public int SCORE { get; set; }
    //}
    public static class DispatcherService {
        public static void Invoke(Action action) {
            Dispatcher dispatchObject = Application.Current != null ? Application.Current.Dispatcher : null;
            if (dispatchObject == null || dispatchObject.CheckAccess())
                action();
            else
                dispatchObject.Invoke(action);
        }
    }

 


}
