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

namespace HighStock.ViewModel {
    class MainViewModel : INotifyPropertyChanged {
        DBControl dbcontrol;
        SettingInfo settingInfo = new SettingInfo() { VolumeValue = 100, BullishValue = 20 };
        private ObservableCollection<StockDate> stockDates = new ObservableCollection<StockDate>();
        public ObservableCollection<StockDate> StockDates {
            set { stockDates = value; }
            get { return stockDates; }
        }
        public MainViewModel() { //생성자, new로 변수 생성하면 무조건 호출되는 함수다.
            int reProcess = RunProcess("StockDB\\SeachingStockData.exe");
            if (reProcess == 0) MessageBox.Show("DB Update success");
            else MessageBox.Show("DB Update fail");
            dbcontrol = new DBControl(2020);
            StockDates.Add(dbcontrol.GetSelectStockDate(DateTime.ParseExact("2020-11-20", "yyyy-MM-dd", null), 10000000, 29));
            //StockDates=dbcontrol.GetTotalStockDate( 10000000, 29);
        }

        SettingStockInfo settingStockInfo = new SettingStockInfo("000", "0", "0", "0", "0", "000", "0", "0", "0"); //세팅값 아직 적용전, dialog와 기본 구조만 만들어놓음

        private String txtDateData; //날짜입력 text창, calendar로 대체 예정
        public String TxtDateData {
            get { return this.txtDateData; }
            set {
                txtDateData = value;
                OnPropertyChanged("TxtDateData");
            }
        }

        private StockDate selectStockDate;
        public StockDate SelectStockDate { // 날짜 선택 변수
            get { return this.selectStockDate; }
            set {
                selectStockDate = value;
                OnPropertyChanged("SelectStockDate");
            }
        }
        private StockInfo selectStockInfo;
        public StockInfo SelectStockInfo {  // 주식 선택 변수
            get { return this.selectStockInfo; }
            set {
                selectStockInfo = value;
                OnPropertyChanged("SelectStockInfo");
            }
        }

        private int RunProcess(String FileName) { //외부 응용프로그램 실행 함수, 이 프로그램에서는 python 응용프로그램을 실행한다.
            Process p = new Process();

            p.StartInfo.FileName = FileName;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

            p.Start();
            p.WaitForExit();

            return p.ExitCode;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) { //WPF의 핵심, 변수가 변경되었을 경우 이벤트를 발생해 이 함수에 들어오게 된다. 
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            switch (propertyName) {
                case "SelectStockDate":
                    TxtDateData = selectStockDate.StrDate;
                    Console.WriteLine("selcetDate : " + selectStockDate.StrDate);
                    selectStockInfo = null;
                    break;
                case "SelectStockInfo": //주식 정보를 선택했을 경우에 dialog창을 생성해서 열어준다.
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
        public ICommand SettingCommand { //ICommand 는 기본적으로 버튼 클릭시, 메소드를 실행해준다.
            get { return (this.settingCommand) ?? (this.settingCommand = new DelegateCommand(Setting)); }
        }

        private void Setting() { //세팅 dialog인데 미완성
            Console.WriteLine("1. Setting : " + settingInfo.BullishValue + ", " + settingInfo.VolumeValue);
            var settingDialog = new SettingDialog();
            var settingViewModel = new SettingViewModel(settingInfo);
            settingDialog.DataContext = settingViewModel;
            settingDialog.ShowDialog();

            Console.WriteLine("1. Setting : " + settingInfo.BullishValue + ", " + settingInfo.VolumeValue);
        }
        private ICommand inputDateCommand; // 날짜 입력 command, 추후에 달력으로 대체 예정
        public ICommand InputDateCommand {
            get { return (this.inputDateCommand) ?? (this.inputDateCommand = new DelegateCommand(InputDate)); }
        }

        private void InputDate() {
            String tmpTxtDate = null;
            if (txtDateData == null) {
                MessageBox.Show("please input Date");
                return;
            } else {
                if (txtDateData.Contains("-") && txtDateData[4] == '-' && txtDateData[7] == '-') {
                    tmpTxtDate = txtDateData;
                } else if (txtDateData.Length == 8) {
                    int chknum = 0;
                    bool isnum = int.TryParse(txtDateData, out chknum);
                    if (isnum) {
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
                if (i.StrDate == tmpTxtDate) {
                    SelectStockDate = i;
                    return;
                }
            }

            DateTime dt = DateTime.ParseExact(tmpTxtDate, "yyyy-MM-dd", null);
            if (dbcontrol.GetYear() != dt.Year) {
                dbcontrol = new DBControl(dt.Year);
            }
            StockDate sd = dbcontrol.GetSelectStockDate(dt, 10000000, 29);
            if (sd == null) {
                MessageBox.Show("DB가 존재하지 않습니다. 날짜를 확인하세요.");
                return;
            } else StockDates.Add(sd);
            SelectStockDate = sd;


        }
        private DateTime cDate = DateTime.Now;
        private ICommand calendarCommand; // 달력
        public ICommand CalendarCommand {
            get { return (this.calendarCommand) ?? (this.calendarCommand = new DelegateCommand(OepnCalendar)); }
        }
        private void OepnCalendar() {
            var calendarDialog = new CalendarDialog();
            var calendarViewModel = new CalendarViewModel();//
            calendarDialog.DataContext = calendarViewModel;
            calendarDialog.ShowDialog();
            String startDate = calendarViewModel.StartDate;
            String endDate = calendarViewModel.EndDate;

            Console.WriteLine("StartDate : {0}, EndDate : {1}",startDate, endDate);
        }


        #region DelegateCommand Class 
        public class DelegateCommand : ICommand //하기 클래스의 경우, ICommand를 사용하기 위한 함수, 복붙하면 쉽게 가져오는 함수
        {

            private readonly Func<bool> canExecute;
            private readonly Action execute;

            /// <summary>
            /// Initializes a new instance of the DelegateCommand class.
            /// </summary>
            /// <param name="execute">indicate an execute function</param>
            public DelegateCommand(Action execute) : this(execute, null) {
            }

            /// <summary>
            /// Initializes a new instance of the DelegateCommand class.
            /// </summary>
            /// <param name="execute">execute function </param>
            /// <param name="canExecute">can execute function</param>
            public DelegateCommand(Action execute, Func<bool> canExecute) {
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
            public bool CanExecute(object o) {
                if (this.canExecute == null) {
                    return true;
                }
                return this.canExecute();
            }

            /// <summary>
            /// implement of icommand interface execute method
            /// </summary>
            /// <param name="o">parameter by default of icomand interface</param>
            public void Execute(object o) {
                this.execute();
            }

            /// <summary>
            /// raise ca excute changed when property changed
            /// </summary>
            public void RaiseCanExecuteChanged() {
                if (this.CanExecuteChanged != null) {
                    this.CanExecuteChanged(this, EventArgs.Empty);
                }
            }
        }
        #endregion


        public static class DispatcherService { //UI 쓰레드와 프로그램 내부 쓰레드 충돌방지를 위한 클래스인데 사용하나?? 모르겠음
            public static void Invoke(Action action) {
                Dispatcher dispatchObject = Application.Current != null ? Application.Current.Dispatcher : null;
                if (dispatchObject == null || dispatchObject.CheckAccess())
                    action();
                else
                    dispatchObject.Invoke(action);
            }
        }




    }
}
