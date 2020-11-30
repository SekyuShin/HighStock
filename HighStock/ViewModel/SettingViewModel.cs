using HighStock.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighStock.ViewModel {
    class SettingViewModel : INotifyPropertyChanged {
        private SettingInfo si;
        public SettingInfo SI {
            get { return this.si; }
            set {
                si = value;
                OnPropertyChanged("SI");
            }
        }
        public SettingViewModel(SettingInfo mainSettingInfo) {
            si = mainSettingInfo;
         }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            switch (propertyName) {
                case "SelectStockDate":
                    
                    break;
            }
        }
    }
}
