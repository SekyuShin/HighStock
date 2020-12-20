using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HighStock.ViewModel {
    public class CalendarViewModel : INotifyPropertyChanged {
        int count = 0;
        private String startDate;
        public String StartDate {
            get { return startDate; }
            set {
                startDate = value;
                RaisePropertyChanged("StartDate");
            }
        }
        private String endDate;
        public String EndDate {
            get { return endDate; }
            set {
                endDate = value;
                RaisePropertyChanged("EndDate");
            }
        }

        private List<DateTime> selectedDates;
        public List<DateTime> SelectedDates {
            get { return selectedDates; }
            set {
                selectedDates = value;
                RaisePropertyChanged("SelectedDates");
            }
        }
        private DateTime selDate;
        public DateTime SelDate {
            get { return selDate; }
            set {
                selDate = value;
                Console.WriteLine("Test : " + selDate);
                RaisePropertyChanged("SelDate");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public CalendarViewModel() {
            SelectedDates = new List<DateTime>();
            SelectedDates.Add(DateTime.Now.Date);
            SelectedDates.Add(DateTime.Now.Date.AddDays(1));
            SelectedDates.Add(DateTime.Now.Date.AddDays(2));
        }

        private void RaisePropertyChanged(String Name) {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(Name));
        }
        public void TestClick(object sender, EventArgs e) {
            Console.WriteLine("Click Event : " + sender.ToString());
            var btn = sender as Button;
            btn.Content = "count : " + (++count);
        }
        public void TestSelectedDate(object sender, SelectionChangedEventArgs e) {
            Console.WriteLine("Select Event : " + sender.ToString());
            var cld = sender as Calendar;
            var selectDates = cld.SelectedDates;
            startDate = null;
            endDate = null;
            foreach (var i in selectDates) {
                if (startDate == null) {
                    StartDate = i.ToString();
                    EndDate = i.ToString();
                } else {
                    EndDate = i.ToString();
                }
            }
            //cld.SelectedDate
            //cld.Foreground
        }
        
    }
}
