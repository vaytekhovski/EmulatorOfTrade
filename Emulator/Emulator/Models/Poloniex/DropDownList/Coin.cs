using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Emulator.Models.Poloniex.DropDownList
{
    public class Coin
    {
        public Coins Name { get; set; }
    }

    public enum Coins
    {
        BTC, XRP, ETH
    }
}