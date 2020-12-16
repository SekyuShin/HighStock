using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighStock.ViewModel {
    public class CalendarViewModel : INotifyPropertyChanged {
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
    }
}
