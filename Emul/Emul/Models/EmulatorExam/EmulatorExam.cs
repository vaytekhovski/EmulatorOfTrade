using Emulator.Models;
using Emulator.Models.DataBase.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Emul.Models.EmulatorExam
{
    public class EmulatorExam
    {
        public EmulatorExam() { }

        double diffFrom, diffTo, diffStep;
        double checkTimeFrom, checkTimeTo, checkTimeStep;
        double buyTimeFrom, buyTimeTo, buyTimeStep;
        double holdTimeFrom, holdTimeTo, holdTimeStep;

        List<BTC_TH> DBBTC = OwnDataBase.database.BTC_TradeHistory.OrderBy(history => history.Date).ToList();
        List<XRP_TH> BDXRP = OwnDataBase.database.XRP_TradeHistory.OrderBy(history => history.Date).ToList();
        List<ETH_TH> BDETH = OwnDataBase.database.ETH_TradeHistory.OrderBy(history => history.Date).ToList();

        public void Settings( double _DiffFrom, double _DiffTo, double _DiffStep, double _CheckTimeFrom, double _CheckTimeTo, double _CheckTimeStep, double _BuyTimeFrom, double _BuyTimeTo, double _BuyTimeStep, double _HoldTimeFrom, double _HoldTimeTo, double _HoldTimeStep, double _balance)
        {
            diffFrom = _DiffFrom;
            diffTo = _DiffTo;
            diffStep = _DiffStep;
            checkTimeFrom = _CheckTimeFrom;
            checkTimeTo = _CheckTimeTo;
            checkTimeStep = _CheckTimeStep;
            buyTimeFrom = _BuyTimeFrom;
            buyTimeTo = _BuyTimeTo;
            buyTimeStep = _BuyTimeStep;
            holdTimeFrom = _HoldTimeFrom;
            holdTimeTo = _HoldTimeTo;
            holdTimeStep = _HoldTimeStep;


        }




    }
}