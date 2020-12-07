from pykrx import stock
import pandas as pd
import numpy as np
from datetime import datetime, timedelta
import os
from pathlib import Path
import math
def divide(x,y):
    try:
        return x/y*100
    except ZeroDivisionError:
        return 0
def GetSTRStocksData(selDate):
    print("Searching for "+selDate.strftime('%Y-%m-%d'))
    date = selDate.strftime('%Y%m%d')
    df = stock.get_market_ohlcv_by_ticker(date)
    df['날짜'] = selDate.strftime('%Y-%m-%d')
    del df['시총비중']
    return df
#TT("20201118")

now = datetime.now()

if now.hour < 16: #posible udate after at 4
    now = now - timedelta(days=1)
nowDate = now.strftime('%Y-%m-%d')
nowDate= datetime.strptime(nowDate, '%Y-%m-%d')

openFileName = now.strftime("%Y") + ".txt"
nowfilepath = Path(os.path.abspath("./StockDB")+"\\"+openFileName)
pastfilepath = Path(os.path.abspath("./StockDB")+"\\"+openFileName)
if(not pastfilepath.is_file()):
    pastfilepath =  Path(os.path.abspath("./StockDB")+"\\" + str(int(now.strftime("%Y")) -1)+".txt")
rawData = pd.read_csv(pastfilepath,names=['날짜','종목코드','종목명','종가','변동폭','등락률','거래량','거래대금','시가','고가','저가','시가총액','상장주식수'],sep='^')
rawData = rawData.convert_dtypes()
lastDateIndex = rawData.loc[[len(rawData)-1]]
lastDateIndex = lastDateIndex['날짜'].values[0]

lastDateIndex = datetime.strptime(lastDateIndex, '%Y-%m-%d')
for i in range((nowDate - lastDateIndex).days) :
    selDate = lastDateIndex + timedelta(days=i+1)
    print(selDate)
    if(selDate.weekday() >= 5) :
        continue
    testDate = GetSTRStocksData(selDate)
    if(testDate['종목명'].notnull().sum() == 0) :
        continue
    
    lastDate = rawData.loc[[len(rawData)-1]]
    lastDate = lastDate['날짜'].values[0]

    tmpData = rawData[rawData['날짜'] == lastDate]
    tmpData = tmpData[['종목코드','종가']]
   
    
    tmpData = tmpData.rename({'종가':'등락률'},axis='columns') #등락률로 변경후 전날 종가 저장

    testDate = pd.merge(testDate,tmpData,how='outer',on='종목코드') 
    testDate['등락률'] = np.where( pd.notnull(testDate['등락률']) == True , testDate['등락률'] ,testDate['시가'])
    testDate['변동폭'] = np.where(testDate['등락률'] != 0, testDate['종가']-testDate['등락률'], 0)
    #testDate.loc[testDate['등락률'] == 0,'등락률'] = np.nan
    #print(testDate.dtypes)
    testDate['등락률'] = testDate.apply(lambda x: divide(x["변동폭"], x["등락률"]), axis=1)

    #print(testDate.loc[(testDate['등락률'] >30)|(testDate['등락률'] <-30)])
    #testDate.to_csv(os.path.abspath("./StockDB/")+'\\'+"checking_zero.csv",float_format='%.2f',mode = 'w',encoding='UTF-8-sig',index = False) #

    ##이상 주식들, 엔케이물산 , 루켄테크놀러지스, 엔에이치스팩14호 등
    testDate.loc[(testDate['등락률'] >30)|(testDate['등락률'] <-30),'변동폭'] = testDate['종가']-testDate['시가'] 
    testDate.loc[(testDate['등락률'] >30)|(testDate['등락률'] <-30),'등락률'] = testDate.apply(lambda x: divide(x["변동폭"], x["시가"]), axis=1)

    testDate.reset_index(drop = False,inplace = True)
    testDate=testDate.convert_dtypes()
    testDate = testDate.dropna(axis=0)
    testDate = testDate[['날짜','종목코드','종목명','종가','변동폭','등락률','거래량','거래대금','시가','고가','저가','시가총액','상장주식수']]
    testDate = testDate.sort_values('거래량',ascending=False)

    testDate.to_csv(nowfilepath,sep='^',float_format='%.2f',mode = 'a',header=False,encoding='UTF-8-sig',index = False) #
    rawData = testDate
    print("done")
print()