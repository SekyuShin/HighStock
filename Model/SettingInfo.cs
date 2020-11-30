using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighStock.Model {
    public class SettingInfo {
        public int VolumeValue { get; set; }
        public int BullishValue { get; set; }
        public ObservableCollection<EmphasisWords2> emphasisWord { get; set; }

    }
    public class EmphasisWords2 {
        String word { get; set; }
    }
}
