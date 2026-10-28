using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoXML
{
    public class ZS
    {
        
             //public string ID { set; get; }
             //public string Akronim { set; get; }
             public string Kraj { set; get; }
             public string Waluta { set; get; }
            public string Wn { set; get; }
            public string Ma { set; get; }
            public string Saldo { set; get; }
            public string SaldoPLN { set; get; }
            public string RodzPow { set; get; }
       // public string SaldoPLNStare { get; set; }
          //  public string Kurs { set; get; }


        public ZS( string Kraj, string Waluta, string Wn,string Ma, string Saldo, string SaldoPln, string RodzPow)
        {
           // this.ID = ID;
           // this.Akronim = Akronim;
            this.Kraj = Kraj;
            this.Waluta = Waluta;
            this.Wn = Wn;
            this.Ma = Ma;
            this.Saldo = Saldo;
            this.SaldoPLN = SaldoPln;
            this.RodzPow = RodzPow;
               // this.SaldoPLNStare = SaldoPlnStare;
           // this.Kurs = kurs;
        }

        public string getSaldo(string waluta)
        {
            return "";
        }
    }
}
