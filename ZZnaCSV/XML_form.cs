using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using YamlDotNet.Serialization;

namespace DoXML
{
    public partial class XML_form : Form
    {
        string connection_string;
        public XML_form(string connection_string)
        {
            InitializeComponent();
            int rok = DateTime.Now.Year;
            string[] stringMiesiace = new[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            Miesiace.Items.AddRange(stringMiesiace);
            string[] stringLata = new[] { Convert.ToString(rok - 2), Convert.ToString(rok - 1), Convert.ToString(rok), Convert.ToString(rok + 1), Convert.ToString(rok + 2), Convert.ToString(rok + 3), Convert.ToString(rok + 4), Convert.ToString(rok + 5) };
            Lata.Items.AddRange(stringLata);
            this.connection_string = connection_string;

        }

        private void button_Click(object sender, EventArgs e)
        {
            
            if (Convert.ToString(Lata.SelectedItem) == "" || Convert.ToString(Miesiace.SelectedItem) == "")
            {
                MessageBox.Show("Proszę wybrać rok i miesiąc ", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                TextWriter tw2 = new StreamWriter(@"C:\sprawozdania NBP\test.txt", append: false);
                try
                {
                    int dzien = DateTime.DaysInMonth(Convert.ToInt32(Lata.SelectedItem), Convert.ToInt32(Miesiace.SelectedItem));
                    string date2 = Lata.SelectedItem.ToString() + "-" + Miesiace.SelectedItem.ToString() + "-" + dzien.ToString();
                    string date1 = "1800-12-28";
                    string data = ConnectToSQL("SELECT DATEDIFF(day, '" + date1 + "','" + date2 + "'); ");

                    string dataKurs = ConnectToSQL("SELECT DATEDIFF(s, '1990-01-01', cdn.TSToDate(" + data + "+1,0) )");




                    /*string selectQuery = "select *  from ( select 1 ID, max(knt_akronim) Akronim, max(Knt_Kraj) Kraj, kks_waluta Waluta, sum(case when DT_dc = 1 then DT_Kwota else 0 end) Wn, " 
                                        + " sum(case when DT_dc = 2 then DT_Kwota else 0 end) Ma, sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) Saldo," 
                                        + " (sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) ) (select top(1) WaE_KursL / WaE_KursM from cdn.WalElem" 
                                        + " where WaE_Symbol = KKS_Waluta and WaE_KursTS <= "+data+" order by WaE_KursTS desc) SaldoPLN, (select top(1) WaE_KursL / WaE_KursM from cdn.WalElem" 
                                        + " where WaE_Symbol = KKS_Waluta and WaE_KursTS <= "+data+"  order by WaE_KursTS des ) Kurs  from cdn.KntKarty join cdn.Konta on KKS_KNTTyp = 32 and KKS_KNTNumer = knt_gidnumer" 
                                        + " join cdn.Dekrety on KKS_GIDNumer = DT_KKSNumer where dt_datadekr <= "+data+" and kks_waluta<> 'pln' and not(DT_Dziennik = '*BO*' and DT_Rok != '2014')" 
                                        + " group by Knt_GIDNumer, KKS_Waluta) t where kraj <> 'PL' order by Akronim";*/

                    /*string selectQuery = "select Kraj, Waluta, sum(Wn), sum(Ma), sum(Saldo), sum(SaldoPLN)  from ( select max(Knt_Kraj) Kraj, kks_waluta Waluta, sum(case when DT_dc = 1 then DT_Kwota else 0 end) Wn, "
                                        + " sum(case when DT_dc = 2 then DT_Kwota else 0 end) Ma, sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) Saldo, "
                                        + " (sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) ) * (select top(1) WaE_KursL / WaE_KursM from cdn.WalElem "
                                        + " where WaE_Symbol = KKS_Waluta and WaE_KursTS <= " + data + " order by WaE_KursTS desc) SaldoPLN, (select top(1) WaE_KursL / WaE_KursM from cdn.WalElem where WaE_Symbol = KKS_Waluta "
                                        + " and WaE_KursTS <= " + data + " order by WaE_KursTS desc) Kurs from cdn.KntKarty join cdn.Konta on KKS_KNTTyp = 32 and KKS_KNTNumer = knt_gidnumer join cdn.Dekrety on KKS_GIDNumer = DT_KKSNumer "
                                        + " where dt_datadekr <= " + data + " and kks_waluta<> 'pln' and not(DT_Dziennik = '*BO*' and DT_Rok != '2014') group by Knt_GIDNumer, knt_kraj, KKS_Waluta ) t where kraj <> 'PL' group by Kraj, Waluta";*/

                    /*   TextWriter tw2 = new StreamWriter(@"C:\sprawozdania NBP\test.txt", append: false);
                       string selectQuery = "select *  from ( select 1 ID, max(knt_akronim) Akronim, max(Knt_Kraj) Kraj, kks_waluta Waluta, sum(case when d1.DT_dc = 1 then d1.DT_Kwota else 0 end) Wn, "
                                           + " sum(case when d1.DT_dc = 2 then d1.DT_Kwota else 0 end) Ma, sum(case when d1.DT_dc = 1 then d1.DT_Kwota else 0 end)-sum(case when d1.DT_dc = 2 then d1.DT_Kwota else 0 end) Saldo, "
                                           + " (sum(case when d1.DT_dc = 1 then d1.DT_Kwota else 0 end)-sum(case when d1.DT_dc = 2 then d1.DT_Kwota else 0 end) ) * (select top(1) WaE_KursL / WaE_KursM from cdn.WalElem "
                                           + " where WaE_Symbol = KKS_Waluta and WaE_KursTS <= " + data + " order by WaE_KursTS desc) SaldoPLN, " 
                                           + "(sum(case when d2.DT_dc = 1 then d2.DT_Kwota else 0 end)-sum(case when d2.DT_dc = 2 then d2.DT_Kwota else 0 end) ) *(select top(1) WaE_KursL / WaE_KursM from cdn.WalElem "
                                         + " where WaE_Symbol = KKS_Waluta and WaE_KursTS <= " + data2 + " order by WaE_KursTS desc) SaldoPLNStare ,(select top(1) WaE_KursL / WaE_KursM from cdn.WalElem where WaE_Symbol = KKS_Waluta "
                                         + " and WaE_KursTS <= " + data + " order by WaE_KursTS desc) Kurs from cdn.KntKarty join cdn.Konta on KKS_KNTTyp = 32 and KKS_KNTNumer = knt_gidnumer join cdn.Dekrety as d1 on KKS_GIDNumer = d1.DT_KKSNumer join cdn.Dekrety as d2 on KKS_GIDNumer = d2.DT_KKSNumer "
                                           + " where d1.dt_datadekr <= " + data + " and d2.dt_datadekr <= " + data2 + "and kks_waluta<> 'pln' and not(d1.DT_Dziennik = '*BO*' and d1.DT_Rok != '2014' and d2.DT_Dziennik = '*BO*' and d2.DT_Rok != '2014') group by Knt_GIDNumer, KKS_Waluta ) t where kraj <> 'PL' order by Akronim";

                       tw2.WriteLine(selectQuery);
                       tw2.Close();*/

                    
                    string selectQuery = "select Kraj, Waluta, sum(Wn) , sum(Ma) , sum(Saldo),CAST(ROUND(sum(SaldoPLN), 0) as decimal(14,0)) , 'NP' from ( select max(Knt_Kraj) Kraj, kks_waluta Waluta, "
                                        + " sum(case when DT_dc = 1 then DT_Kwota else 0 end) Wn, sum(case when DT_dc = 2 then DT_Kwota else 0 end) Ma, "
                                        + " sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) Saldo, "
                                        + " (sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) ) *(select top(1) WaE_KursL / WaE_KursM from cdn.WalElem "
                                        + " where WaE_Symbol = KKS_Waluta and WaE_KursTS <=" + dataKurs + " order by WaE_KursTS desc) SaldoPLN from cdn.KntKarty "
                                        + " join cdn.Konta on KKS_KNTTyp = 32 and KKS_KNTNumer = knt_gidnumer join cdn.Dekrety on KKS_GIDNumer = DT_KKSNumer "
                                        + " where dt_datadekr <= " + data + " and kks_waluta<> 'pln' and not(DT_Dziennik = '*BO*' and DT_Rok != '2014') and Knt_ProcTrnJpk != 'TP'  "
                                        + " group by Knt_GIDNumer,knt_kraj, KKS_Waluta ) t where kraj <> 'PL' and SaldoPLN<0 group by Kraj, Waluta ";
                                        //+ " UNION ALL "
                    string selectQueryPIminus= " select Kraj, Waluta, sum(Wn) , sum(Ma) , sum(Saldo), CAST(ROUND(sum(SaldoPLN), 0) as decimal(14,0)),"
                                        + " 'PI' from(select max(Knt_Kraj) Kraj, kks_waluta Waluta, sum(case when DT_dc = 1 then DT_Kwota else 0 end) Wn, "
                                        + " sum(case when DT_dc = 2 then DT_Kwota else 0 end) Ma, sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) Saldo, "
                                        + " (sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) ) *(select top(1) WaE_KursL / WaE_KursM from cdn.WalElem "
                                        + " where WaE_Symbol = KKS_Waluta and WaE_KursTS <= " + dataKurs + " order by WaE_KursTS desc) SaldoPLN from cdn.KntKarty "
                                        + " join cdn.Konta on KKS_KNTTyp = 32 and KKS_KNTNumer = knt_gidnumer join cdn.Dekrety on KKS_GIDNumer = DT_KKSNumer "
                                        + " where dt_datadekr <= " + data + " and kks_waluta<> 'pln' and not(DT_Dziennik = '*BO*' and DT_Rok != '2014') and Knt_ProcTrnJpk = 'TP'  "
                                        + " group by Knt_GIDNumer,knt_kraj, KKS_Waluta ) t where kraj <> 'PL' and SaldoPLN<0 group by Kraj, Waluta";

                    string selectQueryDod = " select Kraj, Waluta, sum(Wn), sum(Ma), sum(Saldo), CAST(ROUND(sum(SaldoPLN), 0) as decimal(14,0)), 'NP' from ( select max(Knt_Kraj) Kraj, kks_waluta Waluta,  "
                                            + " sum(case when DT_dc = 1 then DT_Kwota else 0 end) Wn, sum(case when DT_dc = 2 then DT_Kwota else 0 end) Ma,  sum(case when DT_dc = 1 then DT_Kwota else 0 end) "
                                            + " -sum(case when DT_dc = 2 then DT_Kwota else 0 end) Saldo,  (sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) ) "
                                            + " *(select top(1) WaE_KursL / WaE_KursM from cdn.WalElem  where WaE_Symbol = KKS_Waluta and WaE_KursTS <= "+dataKurs+" order by WaE_KursTS desc) SaldoPLN from cdn.KntKarty "
                                            + " join cdn.Konta on KKS_KNTTyp = 32 and KKS_KNTNumer = knt_gidnumer join cdn.Dekrety on KKS_GIDNumer = DT_KKSNumer  where dt_datadekr <= "+data+" and kks_waluta<> 'pln' "
                                            + " and not(DT_Dziennik = '*BO*' and DT_Rok != '2014') and Knt_ProcTrnJpk != 'TP'   group by Knt_GIDNumer,knt_kraj, KKS_Waluta ) t where kraj <> 'PL' "
                                            + " and SaldoPLN> 0  group by Kraj, Waluta   ";
                                       // + " UNION ALL "
                   string selectQueryPIDod = " select Kraj, Waluta, sum(Wn), sum(Ma), sum(Saldo), CAST(ROUND(sum(SaldoPLN), 0) as decimal(14,0)), 'PI' from(select max(Knt_Kraj) Kraj, "
                                            + " kks_waluta Waluta, sum(case when DT_dc = 1 then DT_Kwota else 0 end) Wn,  sum(case when DT_dc = 2 then DT_Kwota else 0 end) Ma, "
                                            + " sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) Saldo,  (sum(case when DT_dc = 1 then DT_Kwota else 0 end)- "
                                            + " sum(case when DT_dc = 2 then DT_Kwota else 0 end) ) *(select top(1) WaE_KursL / WaE_KursM from cdn.WalElem  where WaE_Symbol = KKS_Waluta and WaE_KursTS <= "+dataKurs
                                            + " order by WaE_KursTS desc) SaldoPLN from cdn.KntKarty join cdn.Konta on KKS_KNTTyp = 32 and KKS_KNTNumer = knt_gidnumer join cdn.Dekrety on KKS_GIDNumer = DT_KKSNumer "
                                            + " where dt_datadekr <= "+data+" and kks_waluta<> 'pln' and not(DT_Dziennik = '*BO*' and DT_Rok != '2014') and Knt_ProcTrnJpk = 'TP'   group by Knt_GIDNumer,knt_kraj, KKS_Waluta ) t "
                                            + " where kraj <> 'PL' and SaldoPLN> 0  group by Kraj, Waluta";
                    

                    // MessageBox.Show(selectQuery);
                    List<ZS> pozycje = SelectListRowColumn(selectQuery, "cdn.WalElem", connection_string);
                    pozycje.AddRange(SelectListRowColumn(selectQueryPIminus, "cdn.WalElem", connection_string));
                    List<ZS> pozycjeDod = SelectListRowColumn(selectQueryDod, "cdn.WalElem", connection_string);
                    pozycjeDod.AddRange(SelectListRowColumn(selectQueryPIDod, "cdn.WalElem", connection_string));

                    tw2.WriteLine(pozycje[0].Waluta);
                    tw2.WriteLine(selectQueryDod);
                    tw2.WriteLine(selectQueryPIDod);
                    tw2.WriteLine(pozycjeDod[0].Waluta);
                    

                    if (pozycje!=null && pozycje.Count>0)
                    {

                        int dzien2 = 0;
                        string date4 = "";
                        if (Miesiace.SelectedItem.ToString() != "01")
                        {
                            dzien2 = DateTime.DaysInMonth(Convert.ToInt32(Lata.SelectedItem), Convert.ToInt32(Miesiace.Items[Miesiace.SelectedIndex - 1]));
                            date4 = Lata.SelectedItem.ToString() + "-" + Miesiace.Items[Miesiace.SelectedIndex - 1].ToString() + "-" + dzien2.ToString();
                        }
                        else
                        {
                            dzien2 = DateTime.DaysInMonth(Convert.ToInt32(Lata.Items[Lata.SelectedIndex - 1]), 12);
                            date4 = Lata.Items[Lata.SelectedIndex - 1].ToString() + "-12-" + dzien2.ToString();
                        }
                        string data2 = ConnectToSQL("SELECT DATEDIFF(day, '" + date1 + "','" + date4 + "'); ");
                        string dataKurs2 = ConnectToSQL("SELECT DATEDIFF(s, '1990-01-01', cdn.TSToDate("+data2+"+1,0) )");
                        tw2.WriteLine(dataKurs2);
                        tw2.Close();


                        string miesiac = Miesiace.SelectedItem.ToString();
                        string rok = Lata.SelectedItem.ToString();




                        selectQuery = "select Kraj, Waluta, sum(Wn) , sum(Ma) , sum(Saldo),CAST(ROUND(sum(SaldoPLN), 0) as decimal(14,0)) , 'NP' from ( select max(Knt_Kraj) Kraj, kks_waluta Waluta, "
                                       + " sum(case when DT_dc = 1 then DT_Kwota else 0 end) Wn, sum(case when DT_dc = 2 then DT_Kwota else 0 end) Ma, "
                                       + " sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) Saldo, "
                                       + " (sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) ) *(select top(1) WaE_KursL / WaE_KursM from cdn.WalElem "
                                       + " where WaE_Symbol = KKS_Waluta and WaE_KursTS <=" + dataKurs2 + " order by WaE_KursTS desc) SaldoPLN from cdn.KntKarty "
                                       + " join cdn.Konta on KKS_KNTTyp = 32 and KKS_KNTNumer = knt_gidnumer join cdn.Dekrety on KKS_GIDNumer = DT_KKSNumer "
                                       + " where dt_datadekr <= " + data2 + " and kks_waluta<> 'pln' and not(DT_Dziennik = '*BO*' and DT_Rok != '2014') and Knt_ProcTrnJpk != 'TP'  "
                                       + " group by Knt_GIDNumer,knt_kraj, KKS_Waluta ) t where kraj <> 'PL' and SaldoPLN<0 group by Kraj, Waluta ";
                        //+ " UNION ALL "
                        selectQueryPIminus = " select Kraj, Waluta, sum(Wn) , sum(Ma) , sum(Saldo), CAST(ROUND(sum(SaldoPLN), 0) as decimal(14,0)),"
                                            + " 'PI' from(select max(Knt_Kraj) Kraj, kks_waluta Waluta, sum(case when DT_dc = 1 then DT_Kwota else 0 end) Wn, "
                                            + " sum(case when DT_dc = 2 then DT_Kwota else 0 end) Ma, sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) Saldo, "
                                            + " (sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) ) *(select top(1) WaE_KursL / WaE_KursM from cdn.WalElem "
                                            + " where WaE_Symbol = KKS_Waluta and WaE_KursTS <= " + dataKurs2 + " order by WaE_KursTS desc) SaldoPLN from cdn.KntKarty "
                                            + " join cdn.Konta on KKS_KNTTyp = 32 and KKS_KNTNumer = knt_gidnumer join cdn.Dekrety on KKS_GIDNumer = DT_KKSNumer "
                                            + " where dt_datadekr <= " + data2 + " and kks_waluta<> 'pln' and not(DT_Dziennik = '*BO*' and DT_Rok != '2014') and Knt_ProcTrnJpk = 'TP'  "
                                            + " group by Knt_GIDNumer,knt_kraj, KKS_Waluta ) t where kraj <> 'PL' and SaldoPLN<0 group by Kraj, Waluta";

                        selectQueryDod = " select Kraj, Waluta, sum(Wn), sum(Ma), sum(Saldo), CAST(ROUND(sum(SaldoPLN), 0) as decimal(14,0)), 'NP' from ( select max(Knt_Kraj) Kraj, kks_waluta Waluta,  "
                                                + " sum(case when DT_dc = 1 then DT_Kwota else 0 end) Wn, sum(case when DT_dc = 2 then DT_Kwota else 0 end) Ma,  sum(case when DT_dc = 1 then DT_Kwota else 0 end) "
                                                + " -sum(case when DT_dc = 2 then DT_Kwota else 0 end) Saldo,  (sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) ) "
                                                + " *(select top(1) WaE_KursL / WaE_KursM from cdn.WalElem  where WaE_Symbol = KKS_Waluta and WaE_KursTS <= " + dataKurs2 + " order by WaE_KursTS desc) SaldoPLN from cdn.KntKarty "
                                                + " join cdn.Konta on KKS_KNTTyp = 32 and KKS_KNTNumer = knt_gidnumer join cdn.Dekrety on KKS_GIDNumer = DT_KKSNumer  where dt_datadekr <= " + data2 + " and kks_waluta<> 'pln' "
                                                + " and not(DT_Dziennik = '*BO*' and DT_Rok != '2014') and Knt_ProcTrnJpk != 'TP'   group by Knt_GIDNumer,knt_kraj, KKS_Waluta ) t where kraj <> 'PL' "
                                                + " and SaldoPLN> 0  group by Kraj, Waluta   ";
                        // + " UNION ALL "
                        selectQueryPIDod = " select Kraj, Waluta, sum(Wn), sum(Ma), sum(Saldo), CAST(ROUND(sum(SaldoPLN), 0) as decimal(14,0)), 'PI' from(select max(Knt_Kraj) Kraj, "
                                                 + " kks_waluta Waluta, sum(case when DT_dc = 1 then DT_Kwota else 0 end) Wn,  sum(case when DT_dc = 2 then DT_Kwota else 0 end) Ma, "
                                                 + " sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) Saldo,  (sum(case when DT_dc = 1 then DT_Kwota else 0 end)- "
                                                 + " sum(case when DT_dc = 2 then DT_Kwota else 0 end) ) *(select top(1) WaE_KursL / WaE_KursM from cdn.WalElem  where WaE_Symbol = KKS_Waluta and WaE_KursTS <= " + dataKurs2
                                                 + " order by WaE_KursTS desc) SaldoPLN from cdn.KntKarty join cdn.Konta on KKS_KNTTyp = 32 and KKS_KNTNumer = knt_gidnumer join cdn.Dekrety on KKS_GIDNumer = DT_KKSNumer "
                                                 + " where dt_datadekr <= " + data2 + " and kks_waluta<> 'pln' and not(DT_Dziennik = '*BO*' and DT_Rok != '2014') and Knt_ProcTrnJpk = 'TP'   group by Knt_GIDNumer,knt_kraj, KKS_Waluta ) t "
                                                 + " where kraj <> 'PL' and SaldoPLN> 0  group by Kraj, Waluta";


                        // MessageBox.Show(selectQuery);
                        List<ZS> pozycjeStare = SelectListRowColumn(selectQuery, "cdn.WalElem", connection_string);
                        pozycjeStare.AddRange(SelectListRowColumn(selectQueryPIminus, "cdn.WalElem", connection_string));
                        List<ZS> pozycjeDodStare = SelectListRowColumn(selectQueryDod, "cdn.WalElem", connection_string);
                        pozycjeDodStare.AddRange(SelectListRowColumn(selectQueryPIDod, "cdn.WalElem", connection_string));




                        //XML_form.ActiveForm.Visible = false;

                        Pasek p = new Pasek();
                        p.Visible = true;
                        p.pBar1.Visible = true;
                        // Set Minimum to 1 to represent the first file being copied.
                        p.pBar1.Minimum = 1;
                        // Set Maximum to the total number of files to copy.
                        p.pBar1.Maximum = pozycje.Count() + pozycjeDod.Count() + pozycjeStare.Count()+pozycjeDodStare.Count();
                        // Set the initial value of the ProgressBar.
                        p.pBar1.Value = 1;
                        // Set the Step property to a value of 1 to represent each file being copied.
                        p.pBar1.Step = 1;
                        dodajDokument(pozycje, "PZ", data2, miesiac, rok, p, dataKurs2, pozycjeStare);
                        dodajDokument(pozycjeDod, "AZ", data2, miesiac, rok, p, dataKurs2, pozycjeDodStare);
                        p.Visible = false;
                        MessageBox.Show("Utworzono pliki: NBP MPZ-KRH " +Miesiace.SelectedItem + "/" + Lata.SelectedItem + ".xml oraz  NBP MAZ-KRH " +Miesiace.SelectedItem + "/" + Lata.SelectedItem + ".xml", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                        MessageBox.Show("Dla " +Miesiace.SelectedItem + "/" + Lata.SelectedItem + " nie istnieją pozycje do pobrania", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    
                    Application.Exit();



                }
                catch (Exception en)
                {
                    tw2.Close();
                    MessageBox.Show("Błąd generowania dokumentu XML: " + en.ToString(), "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                }
            }
        }

        public void dodajDokument(List<ZS> pozycje, string typ, string data, string miesiac, string rok, Pasek p, string dataKurs, List<ZS> pozycjeStare)
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Async = true;
                using (XmlWriter PZ = XmlWriter.Create(@"C:\sprawozdania NBP\NBP M" + typ + "-KRH " +Miesiace.SelectedItem + "_" + Lata.SelectedItem + ".xml", settings))
                {

                    string okres_spr = "";
                    if(Miesiace.SelectedItem.ToString()=="12")
                    {
                        okres_spr = Convert.ToString(Convert.ToInt32(Lata.SelectedItem.ToString())+1) + "_01_01";
                    }
                    else
                    {
                        string miesiac_okr = Convert.ToString(Convert.ToInt32(Miesiace.SelectedItem.ToString()) + 1);
                        if (miesiac_okr.Length == 1)
                            miesiac_okr = "0" + miesiac_okr;
                        okres_spr =Lata.SelectedItem.ToString() + "_" + miesiac_okr +"_01";
                    }
                    // Opens the document
                    PZ.WriteStartDocument();


                    #region przesylka
                    /*
                    PZ.WriteStartElement("prz", "przesylka", @"http://sprawozdawczosc.nbp.pl/schema/2023_02_01/przesylka");
                    PZ.WriteAttributeString("xmlns", "ts", null, @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan");
                    //PZ.WriteAttributeString("xmlns:xsi", @"http://www.w3.org/2001/XMLSchema-instance");
                    PZ.WriteAttributeString("xmlns", "td", null, @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typyDanych");
                    PZ.WriteAttributeString("okres_sprawozdawczy", okres_spr);
                    PZ.WriteAttributeString("xsi", "schemaLocation", @"http://www.w3.org/2001/XMLSchema-instance", @"http://sprawozdawczosc.nbp.pl/schema/2023_02_01/przesylka http://sprawozdawczosc.nbp.pl/schema/2023_02_01/przesylka.xsd");
                    PZ.Flush();*/

                    PZ.WriteStartElement("prz", "przesylka", @"http://sprawozdawczosc.nbp.pl/schema/2024_10_01/przesylka");
                    PZ.WriteAttributeString("xmlns", "ts", null, @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan");
                    //PZ.WriteAttributeString("xmlns:xsi", @"http://www.w3.org/2001/XMLSchema-instance");
                    PZ.WriteAttributeString("xmlns", "td", null, @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typyDanych");
                    PZ.WriteAttributeString("okres_sprawozdawczy", okres_spr);
                    PZ.WriteAttributeString("xsi", "schemaLocation", @"http://www.w3.org/2001/XMLSchema-instance", @"http://sprawozdawczosc.nbp.pl/schema/2024_10_01/przesylka http://sprawozdawczosc.nbp.pl/schema/2024_10_01/przesylka.xsd");
                    PZ.Flush();

                    #region sprawozdanie
                    PZ.WriteStartElement("prz", "sprawozdanie", @"http://sprawozdawczosc.nbp.pl/schema/2024_10_01/przesylka");
                    PZ.WriteAttributeString("dataWypelnienia", DateTime.Now.ToString("yyyy-MM-dd"));
                    PZ.WriteAttributeString("type", @"http://www.w3.org/2001/XMLSchema-instance", @"ts:SprM" + typ + "-KRHType");
                    PZ.Flush();

                    #region naglowek
                    PZ.WriteStartElement("ts", "naglowek", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan");

                    PZ.WriteStartElement("td", "regon", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typyDanych");
                    PZ.WriteValue("47319001400000");
                    PZ.WriteEndElement();

                    PZ.WriteStartElement("ts", "nazwa", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan");
                    PZ.WriteValue("RAINBOW TOURS S.A.");
                    PZ.WriteEndElement();


                    PZ.WriteEndElement();
                    PZ.Flush();

                    #endregion

                    #region dane
                    PZ.WriteStartElement("ts", "dane", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan");

                    PZ.WriteStartElement("ts", "wiersze", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan");
                    PZ.Flush();


                    int lp = 1;
                    foreach (ZS pozycja in pozycje)
                    {
                        p.pBar1.PerformStep();
                        //if ((typ == "PZ" && pozycja.SaldoPLN.Contains("-")) || (typ == "AZ" && !pozycja.SaldoPLN.Contains("-")))
                        //{
                        try
                        {
                            if (pozycja.Kraj != "MC")
                            {

                                string stanPoczQuery = "";
                                if (pozycja.RodzPow == "NP")
                                {
                                    if (typ == "PZ")
                                    {
                                        stanPoczQuery = "select CAST(ROUND(sum(SaldoPLN), 0) as decimal(14,0))  from ( select  max(Knt_Kraj) Kraj, kks_waluta Waluta,  "
                                                            + " (sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) ) * (select top(1) WaE_KursL / WaE_KursM from cdn.WalElem "
                                                            + " where WaE_Symbol = KKS_Waluta and WaE_KursTS <= " + dataKurs + " order by WaE_KursTS desc) SaldoPLN"
                                                            + "  from cdn.KntKarty join cdn.Konta on KKS_KNTTyp = 32 and KKS_KNTNumer = knt_gidnumer join cdn.Dekrety on KKS_GIDNumer = DT_KKSNumer "
                                                            + " where dt_datadekr <= " + data + " and kks_waluta<> 'pln' and not(DT_Dziennik = '*BO*' and DT_Rok != '2014') and Knt_ProcTrnJpk != 'TP'  group by Knt_GIDNumer, knt_kraj,KKS_Waluta ) t where kraj <> 'PL' "
                                                            + " and kraj='" + pozycja.Kraj + "' and Waluta='" + pozycja.Waluta + "' and SaldoPLN<0 group by Kraj, Waluta";
                                    }
                                    else
                                    {
                                        stanPoczQuery = "select CAST(ROUND(sum(SaldoPLN), 0) as decimal(14,0)) from ( select  max(Knt_Kraj) Kraj, kks_waluta Waluta,  "
                                                            + " (sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) ) * (select top(1) WaE_KursL / WaE_KursM from cdn.WalElem "
                                                            + " where WaE_Symbol = KKS_Waluta and WaE_KursTS <= " + dataKurs + " order by WaE_KursTS desc) SaldoPLN"
                                                            + "  from cdn.KntKarty join cdn.Konta on KKS_KNTTyp = 32 and KKS_KNTNumer = knt_gidnumer join cdn.Dekrety on KKS_GIDNumer = DT_KKSNumer "
                                                            + " where dt_datadekr <= " + data + " and kks_waluta<> 'pln' and not(DT_Dziennik = '*BO*' and DT_Rok != '2014') and Knt_ProcTrnJpk != 'TP'  group by Knt_GIDNumer, knt_kraj,KKS_Waluta ) t where kraj <> 'PL' "
                                                            + " and kraj='" + pozycja.Kraj + "' and Waluta='" + pozycja.Waluta + "' and SaldoPLN>0 group by Kraj, Waluta";
                                    }

                                }
                                else
                                {
                                    if (typ == "PZ")
                                    {
                                        stanPoczQuery = "select CAST(ROUND(sum(SaldoPLN), 0) as decimal(14,0))  from ( select  max(Knt_Kraj) Kraj, kks_waluta Waluta,  "
                                                           + " (sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) ) * (select top(1) WaE_KursL / WaE_KursM from cdn.WalElem "
                                                           + " where WaE_Symbol = KKS_Waluta and WaE_KursTS <= " + dataKurs + " order by WaE_KursTS desc) SaldoPLN"
                                                           + "  from cdn.KntKarty join cdn.Konta on KKS_KNTTyp = 32 and KKS_KNTNumer = knt_gidnumer join cdn.Dekrety on KKS_GIDNumer = DT_KKSNumer "
                                                           + " where dt_datadekr <= " + data + " and kks_waluta<> 'pln' and not(DT_Dziennik = '*BO*' and DT_Rok != '2014') and Knt_ProcTrnJpk = 'TP'  group by Knt_GIDNumer, knt_kraj,KKS_Waluta ) t where kraj <> 'PL' "
                                                           + " and kraj='" + pozycja.Kraj + "' and Waluta='" + pozycja.Waluta + "' and SaldoPLN<0 group by Kraj, Waluta";
                                    }
                                    else
                                    {
                                        stanPoczQuery = "select CAST(ROUND(sum(SaldoPLN), 0) as decimal(14,0)) from ( select  max(Knt_Kraj) Kraj, kks_waluta Waluta,  "
                                                           + " (sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) ) * (select top(1) WaE_KursL / WaE_KursM from cdn.WalElem "
                                                           + " where WaE_Symbol = KKS_Waluta and WaE_KursTS <= " + dataKurs + " order by WaE_KursTS desc) SaldoPLN"
                                                           + "  from cdn.KntKarty join cdn.Konta on KKS_KNTTyp = 32 and KKS_KNTNumer = knt_gidnumer join cdn.Dekrety on KKS_GIDNumer = DT_KKSNumer "
                                                           + " where dt_datadekr <= " + data + " and kks_waluta<> 'pln' and not(DT_Dziennik = '*BO*' and DT_Rok != '2014') and Knt_ProcTrnJpk = 'TP'  group by Knt_GIDNumer, knt_kraj,KKS_Waluta ) t where kraj <> 'PL' "
                                                           + " and kraj='" + pozycja.Kraj + "' and Waluta='" + pozycja.Waluta + "' and SaldoPLN>0 group by Kraj, Waluta";

                                    }

                                }

                                string stanPoczSelect = ConnectToSQL(stanPoczQuery);
                                string stanPocz = "0";
                                if (stanPoczSelect != "")
                                    stanPocz = stanPoczSelect.Replace("-","");//Math.Round(Math.Abs(Decimal.Parse(stanPoczSelect)), 0, MidpointRounding.AwayFromZero).ToString();


                                string stanKon = pozycja.SaldoPLN.Replace("-","");//Math.Round(Math.Abs(Decimal.Parse(pozycja.SaldoPLN)), 0, MidpointRounding.AwayFromZero).ToString();

                                if (stanPocz != "0" || stanKon != "0")
                                {
                                    if(pozycja.Kraj=="FR")
                                    {
                                        if (typ == "PZ")
                                        {
                                            stanPoczQuery = "select CAST(ROUND(sum(SaldoPLN), 0) as decimal(14,0))  from ( select  max(Knt_Kraj) Kraj, kks_waluta Waluta,  "
                                                                + " (sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) ) * (select top(1) WaE_KursL / WaE_KursM from cdn.WalElem "
                                                                + " where WaE_Symbol = KKS_Waluta and WaE_KursTS <= " + dataKurs + " order by WaE_KursTS desc) SaldoPLN"
                                                                + "  from cdn.KntKarty join cdn.Konta on KKS_KNTTyp = 32 and KKS_KNTNumer = knt_gidnumer join cdn.Dekrety on KKS_GIDNumer = DT_KKSNumer "
                                                                + " where dt_datadekr <= " + data + " and kks_waluta<> 'pln' and not(DT_Dziennik = '*BO*' and DT_Rok != '2014') and Knt_ProcTrnJpk != 'TP'  group by Knt_GIDNumer, knt_kraj,KKS_Waluta ) t where kraj <> 'PL' "
                                                                + " and kraj='MC' and Waluta='" + pozycja.Waluta + "' and SaldoPLN<0 group by Kraj, Waluta";
                                        }
                                        else
                                        {
                                            stanPoczQuery = "select CAST(ROUND(sum(SaldoPLN), 0) as decimal(14,0)) from ( select  max(Knt_Kraj) Kraj, kks_waluta Waluta,  "
                                                                + " (sum(case when DT_dc = 1 then DT_Kwota else 0 end)-sum(case when DT_dc = 2 then DT_Kwota else 0 end) ) * (select top(1) WaE_KursL / WaE_KursM from cdn.WalElem "
                                                                + " where WaE_Symbol = KKS_Waluta and WaE_KursTS <= " + dataKurs + " order by WaE_KursTS desc) SaldoPLN"
                                                                + "  from cdn.KntKarty join cdn.Konta on KKS_KNTTyp = 32 and KKS_KNTNumer = knt_gidnumer join cdn.Dekrety on KKS_GIDNumer = DT_KKSNumer "
                                                                + " where dt_datadekr <= " + data + " and kks_waluta<> 'pln' and not(DT_Dziennik = '*BO*' and DT_Rok != '2014') and Knt_ProcTrnJpk != 'TP'  group by Knt_GIDNumer, knt_kraj,KKS_Waluta ) t where kraj <> 'PL' "
                                                                + " and kraj='MC' and Waluta='" + pozycja.Waluta + "' and SaldoPLN>0 group by Kraj, Waluta";
                                        }


                                        string stanPoczSelect2 = ConnectToSQL(stanPoczQuery);

                                        if (stanPoczSelect2 != "")
                                            stanPocz = Convert.ToString(Convert.ToInt32(stanPocz) + Convert.ToInt32(stanPoczSelect2)).Replace("-","");//Math.Round(Math.Abs(Decimal.Parse(stanPoczSelect2)), 0, MidpointRounding.AwayFromZero));

                                        if (pozycje.FindAll(poz => poz.Kraj == "MC" && poz.Waluta == pozycja.Waluta).Count() > 0)
                                            stanKon = Convert.ToString(Convert.ToInt32(stanKon) + Convert.ToInt32(pozycje.FindAll(poz => poz.Kraj == "MC" && poz.Waluta == pozycja.Waluta).First().SaldoPLN)).Replace("-","");//Math.Round(Math.Abs(Decimal.Parse(pozycje.FindAll(poz => poz.Kraj == "MC" && poz.Waluta==pozycja.Waluta).First().SaldoPLN)), 0, MidpointRounding.AwayFromZero)) ;

                                    }



                                    PZ.WriteStartElement("ts", "wiersz", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan");
                                    PZ.WriteAttributeString("nrWiersza", lp.ToString());
                                    PZ.Flush();




                                    //string rodzPow = "NP";
                                    /* string rodzPowQuery = "if exists(SELECT 1 FROM [CDN].[KntKarty] WHERE Knt_ProcTrnJpk = 'TP' and Knt_Akronim = '" + pozycja.Akronim + "') SELECT 'TAK' else select 'NIE'";
                                     if (ConnectToSQL(rodzPowQuery) == "TAK")   //(pozycja.Akronim == "WHITE OL" || pozycja.Akronim == "RAINBOW HELLAS" || pozycja.Akronim == "RAINBOW TR" || pozycja.Akronim == "RT SUCURSAL")
                                         rodzPow = "PI";*/

                                    // PZ.WriteAttributeString("td", "rodzPowKap", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typyDanych", rodzPow);
                                    //PZ.FlushAsync();

                                    PZ.WriteStartElement("td", "rodzPowKap", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typyDanych");
                                    PZ.WriteString(pozycja.RodzPow);
                                    PZ.WriteEndElement();


                                    // PZ.WriteAttributeString("td", "sekIns1", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typyDanych", "N");

                                    PZ.WriteStartElement("td", "sekIns1", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typyDanych");
                                    PZ.WriteString("N");
                                    PZ.WriteEndElement();

                                    //PZ.WriteAttributeString("td", "kraj", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typyDanych", pozycja.Kraj);


                                    PZ.WriteStartElement("td", "kraj", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typyDanych");
                                    PZ.WriteString(pozycja.Kraj);
                                    PZ.WriteEndElement();

                                    //PZ.WriteAttributeString("td", "waluta", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typyDanych", pozycja.Waluta);
                                    //            PZ.FlushAsync();


                                    PZ.WriteStartElement("td", "waluta", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typyDanych");
                                    PZ.WriteString(pozycja.Waluta);
                                    PZ.WriteEndElement();



                                    //MessageBox.Show(stanPocz + " " + lp.ToString() + " " + pozycja.Akronim);
                                    // string  stanPocz = Math.Round(Math.Abs(Convert.ToDouble(pozycja.SaldoPLNStare)), 0, MidpointRounding.AwayFromZero).ToString(); 
                                    PZ.WriteStartElement("ts", "stanNaPoczOkr", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan");
                                    PZ.WriteString(stanPocz);
                                    PZ.WriteEndElement();
                                    // PZ.WriteAttributeString("ts", "stanNaPoczOkr", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan", stanPocz);
                                    PZ.Flush();


                                    PZ.WriteStartElement("ts", "pozZmiany", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan");
                                    PZ.WriteString("0");
                                    PZ.WriteEndElement();
                                    // PZ.WriteAttributeString("ts", "pozZmiany", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan", "0" );
                                    // PZ.FlushAsync();


                                    PZ.WriteStartElement("ts", "stanNaKonOkr", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan");
                                    PZ.WriteString(stanKon);
                                    PZ.WriteEndElement();
                                    //PZ.WriteAttributeString("ts", "stanNaKonOkr", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan", stanKon);
                                    //PZ.FlushAsync();



                                    PZ.WriteEndElement();
                                    PZ.Flush();

                                    lp++;
                                }

                            }
                        }
                        catch (System.FormatException ew)
                        {
                            MessageBox.Show("Błąd dodawania wiersza " + pozycja.Kraj + " o walucie " + pozycja.Waluta + ": " + ew.ToString(), "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        //}
                    }



                    #region Poprzedni miesiac

                    foreach (ZS pozycja in pozycjeStare)
                    {
                        p.pBar1.PerformStep();
                        //if ((typ == "PZ" && pozycja.SaldoPLN.Contains("-")) || (typ == "AZ" && !pozycja.SaldoPLN.Contains("-")))
                        //{
                        try
                        {
                            if (pozycja.Kraj != "MC" && pozycje.FindAll(poz => poz.Kraj == pozycja.Kraj && poz.Waluta == pozycja.Waluta && poz.RodzPow==pozycja.RodzPow).Count() == 0)
                            {


                                string stanPocz = pozycja.SaldoPLN.Replace("-","");

                                string stanKon = "0";//Math.Round(Math.Abs(Decimal.Parse(pozycja.SaldoPLN)), 0, MidpointRounding.AwayFromZero).ToString();

                                if (stanPocz != "0" || stanKon != "0")
                                {
                                   

                                    PZ.WriteStartElement("ts", "wiersz", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan");
                                    PZ.WriteAttributeString("nrWiersza", lp.ToString());
                                    PZ.Flush();




                                    //string rodzPow = "NP";
                                    /* string rodzPowQuery = "if exists(SELECT 1 FROM [CDN].[KntKarty] WHERE Knt_ProcTrnJpk = 'TP' and Knt_Akronim = '" + pozycja.Akronim + "') SELECT 'TAK' else select 'NIE'";
                                     if (ConnectToSQL(rodzPowQuery) == "TAK")   //(pozycja.Akronim == "WHITE OL" || pozycja.Akronim == "RAINBOW HELLAS" || pozycja.Akronim == "RAINBOW TR" || pozycja.Akronim == "RT SUCURSAL")
                                         rodzPow = "PI";*/

                                    // PZ.WriteAttributeString("td", "rodzPowKap", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typyDanych", rodzPow);
                                    //PZ.FlushAsync();

                                    PZ.WriteStartElement("td", "rodzPowKap", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typyDanych");
                                    PZ.WriteString(pozycja.RodzPow);
                                    PZ.WriteEndElement();


                                    // PZ.WriteAttributeString("td", "sekIns1", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typyDanych", "N");

                                    PZ.WriteStartElement("td", "sekIns1", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typyDanych");
                                    PZ.WriteString("N");
                                    PZ.WriteEndElement();

                                    //PZ.WriteAttributeString("td", "kraj", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typyDanych", pozycja.Kraj);


                                    PZ.WriteStartElement("td", "kraj", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typyDanych");
                                    PZ.WriteString(pozycja.Kraj);
                                    PZ.WriteEndElement();

                                    //PZ.WriteAttributeString("td", "waluta", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typyDanych", pozycja.Waluta);
                                    //            PZ.FlushAsync();


                                    PZ.WriteStartElement("td", "waluta", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typyDanych");
                                    PZ.WriteString(pozycja.Waluta);
                                    PZ.WriteEndElement();



                                    //MessageBox.Show(stanPocz + " " + lp.ToString() + " " + pozycja.Akronim);
                                    // string  stanPocz = Math.Round(Math.Abs(Convert.ToDouble(pozycja.SaldoPLNStare)), 0, MidpointRounding.AwayFromZero).ToString(); 
                                    PZ.WriteStartElement("ts", "stanNaPoczOkr", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan");
                                    PZ.WriteString(stanPocz);
                                    PZ.WriteEndElement();
                                    // PZ.WriteAttributeString("ts", "stanNaPoczOkr", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan", stanPocz);
                                    PZ.Flush();


                                    PZ.WriteStartElement("ts", "pozZmiany", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan");
                                    PZ.WriteString("0");
                                    PZ.WriteEndElement();
                                    // PZ.WriteAttributeString("ts", "pozZmiany", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan", "0" );
                                    // PZ.FlushAsync();


                                    PZ.WriteStartElement("ts", "stanNaKonOkr", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan");
                                    PZ.WriteString(stanKon);
                                    PZ.WriteEndElement();
                                    //PZ.WriteAttributeString("ts", "stanNaKonOkr", @"http://sprawozdawczosc.nbp.pl/schema/wspolne/typySprawozdan", stanKon);
                                    //PZ.FlushAsync();



                                    PZ.WriteEndElement();
                                    PZ.Flush();

                                    lp++;
                                }

                            }
                        }
                        catch (System.FormatException ew)
                        {
                            MessageBox.Show("Błąd dodawania wiersza z poprzedniego miesiąca " + pozycja.Kraj + " o walucie " + pozycja.Waluta + ": " + ew.ToString(), "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        //}
                    }
                    #endregion



                    PZ.WriteEndElement();

                    PZ.WriteEndElement();
                    #endregion


                    PZ.WriteEndElement();
                    #endregion


                    PZ.WriteEndElement();

                    #endregion

                    PZ.WriteEndDocument();
                    PZ.Flush();

                    PZ.Close();
                }





            }
            catch(Exception ex)
            {
                MessageBox.Show("Błąd dodawania dokumentu " +typ, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



}

public string ConnectToSQL(string query)
{
string str = "";
try
{
    using (SqlConnection connection = new SqlConnection(connection_string))
    {
        SqlDataAdapter adapter = new SqlDataAdapter();
        adapter.SelectCommand = new SqlCommand(query, connection);
        DataSet dataSet = new DataSet();
        adapter.Fill(dataSet, "Query");
        if (dataSet.Tables.Count == 0)
        {
            return "";
        }
        foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
        {
            str = row[0].ToString();
        }
    }
}
catch (Exception ex)
{
    str = str + "Bład SQL:" + ex.ToString() + "\n";
}
return str;
}

        public static List<ZS> SelectListRowColumn(string query, string srcTableName, string connection_string)
        {
        List<string> str = new List<string>();
        List<ZS> myList = new List<ZS>();
        string sttr = "";
        try
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(query, connection);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, srcTableName);
                foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                {
                    foreach (DataColumn column in (InternalDataCollectionBase)dataSet.Tables[0].Columns)
                    {
                        str.Add(row[column].ToString());

                        //MessageBox.Show(column + ": " + row[column].ToString());
                    }
                    myList.Add(new ZS(str[0], str[1], str[2], str[3], str[4], str[5], str[6]));
                    str.Clear();

                }
            }

        }
        catch (Exception ex)
        {
            MessageBox.Show(sttr = sttr + "Bład SQL:" + ex.ToString() + "\n");
        }
        return myList;
        }
      /*  public static List<ZS> SelectListRowColumn(string query, string srcTableName, string connection_string)
        {
            List<ZS> myList = new List<ZS>();
            string sttr = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(connection_string))
                {


                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                myList.Add(new ZS(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6)));
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(sttr = sttr + "Bład SQL:" + ex.ToString() + "\n");
            }
            return myList;
        
        }*/
    }
}
