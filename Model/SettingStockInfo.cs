using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighStock.Model {
    public class SettingStockInfo {
        public int VolumeValue { get; set; }
        public int BullishValue { get; set; }
        //Temp Setting
        public String setting_Volume_CO { get; set; }              //시장구분
        public String setting_Volume_Volume { get; set; }          //거래량 구분
        public String setting_Volume_Value { get; set; }           //가격 구분
        public String setting_Volume_ValueStandard { get; set; }   //거래대금구분
        public String setting_Volume_Time { get; set; }            //장운영구분


        //Temp Setting
        public String setting_Bulish_CO { get; set; }             //시장구분
        public String setting_Bulish_Bulish { get; set; }         //종목조건
        public String setting_Bulish_Volume { get; set; }         //거래량구분
        public String setting_Bulish_Value { get; set; }          //매매금
        public ObservableCollection<EmphasisWords> emphasisWord { get; set; }

        public SettingStockInfo(String setting_Volume_CO, String setting_Volume_Volume, String setting_Volume_Value, String setting_Volume_ValueStandard, String setting_Volume_Time,
                                String setting_Bulish_CO, String setting_Bulish_Bulish, String setting_Bulish_Volume,  String setting_Bulish_Value) {
            this.setting_Volume_CO = setting_Volume_CO;
            this.setting_Volume_Volume = setting_Volume_Volume;
            this.setting_Volume_Value = setting_Volume_Value;
            this.setting_Volume_ValueStandard = setting_Volume_ValueStandard;
            this.setting_Volume_Time = setting_Volume_Time;

            this.setting_Bulish_CO = setting_Bulish_CO;
            this.setting_Bulish_Bulish = setting_Bulish_Bulish;
            this.setting_Bulish_Volume = setting_Bulish_Volume;
            this.setting_Bulish_Value = setting_Bulish_Value;
        }

    }
    public class EmphasisWords {
        String word { get; set; }
    }
}
