using Hydra;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using DoXML;
using System.Collections.Generic;
using static DoXML.ZS;
using System.Text;
using System;
using System.Data.SqlClient;
using System.Data;
using cdn_api;
using System.IO;
using System.Xml.Linq;

//do update: WHERE Okr_id=(SELECT MAX(OKR_ID) FROM[ERPXL_RainbowTours].[CDN].[Okresy] WHERE OKR_ZamknietyDo<>0)

[assembly: CallbackAssemblyDescription(Name: "ONE-030 Do XML",
                                           Description: "Zapisanie raportu w pliku XML",
                                           Author: "OneSupport",
                                           Version: "1.0",
                                           SystemVersion: "2021.0.2",
                                           Created: "01-08-2023")]


namespace DoXML
{
    [SubscribeProcedure(Procedures.KKSLista, "Do XML")]
    public class ZSnaFS_Class : Callback
    {
        
        private ClaWindow RapButton;
        private ClaWindow zakladkaTabWewnetrzne;
        private ClaWindow filtrBtn;
        private ClaWindow listaPoz;
        
        public override void Init()
        {

            AddSubscription(true, 0, Events.JustAfterWindowOpening, new TakeEventDelegate(OnOpenWindow));
            AddSubscription(false, 0, Events.ResizeWindow, new TakeEventDelegate(Resize));
        }



        private bool OnOpenWindow(Procedures ProcId, int ControlId, Events Event)
        {

            ClaWindow Parent = GetWindow();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Rectangle WndBnd = GetWindow().Bounds;
            zakladkaTabWewnetrzne = GetWindow().AllChildren["?List"];
            //listaPoz = GetWindow().AllChildren["?ListaZam"];
            //RapButton = zakladkaTabWewnetrzne.Children.Add(ControlTypes.button);
            RapButton = Parent.Children.Add(ControlTypes.button);
            RapButton.Visible = true;
            RapButton.TextRaw = "XML";
            RapButton.ToolTipRaw = "Zapisanie raportu do pliku XML";
            //RapButton.IconRaw = "C:\\OneSupport\\klodka.ico";

            filtrBtn = GetWindow().AllChildren["?PopupButton"];                            //pobiera współrzędne przycisku zapisz
            RapButton.Bounds = new Rectangle(filtrBtn.Bounds.X - 26, filtrBtn.Bounds.Y, 20, 15);
            //listID = GetWindow().AllChildren["?ListaZam"].Id;      //tu sprawdzic


            //RapButton.OnBeforeAccepted += new TakeEventDelegate(zaczytaj_OnAfterAccepted);
            RapButton.OnAfterMouseDown += new TakeEventDelegate(BtnOnMouseDown);
            //pobrane z pola w zakładce Raporty
            return true;
        }

        public bool Resize(Procedures ProcId, int ControlId, Events Event)
        {
            filtrBtn = GetWindow().AllChildren["?PopupButton"];                            //pobiera współrzędne przycisku zapisz
            RapButton.Bounds = new Rectangle(filtrBtn.Bounds.X - 26, filtrBtn.Bounds.Y, 20, 15);

            return true;
        }

        
        public bool BtnOnMouseDown(Procedures ProcId, int ControlId, Events Event)
        {

            try
            {

                

                string connectionString = "";
                if (Runtime.ConfigurationDictionary.Baza.ToString().Substring(0, 25) == "ERPXL_RainbowTours_TESTRT")
                    connectionString = @"Data Source = XL2; Initial Catalog = ERPXL_RainbowTours_TESTRT; User ID = onesupport; Password=g54q@8w#v$Y.Df_8f4hg";
                else if (Runtime.ConfigurationDictionary.Baza.ToString().Substring(0, 18) == "ERPXL_RainbowTours")
                    connectionString = @"Data Source = XL2; Initial Catalog = ERPXL_RainbowTours; User ID = onesupport; Password=g54q@8w#v$Y.Df_8f4hg";
                else if (Runtime.ConfigurationDictionary.Baza.ToString().Substring(0, 13) == "ERPXL_RT_Test")
                    connectionString = @"Data Source = XL2; Initial Catalog = ERPXL_RT_Test; User ID = onesupport; Password=g54q@8w#v$Y.Df_8f4hg";

                if (connectionString != "")
                {
                    Application.Run(new XML_form(connectionString));
                }
                else
                    MessageBox.Show("Dodatek nie posiada funkcjonalności dla tej bazy", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Information);




            }
            catch(Exception e)
            {
                MessageBox.Show("Błąd otwierania formu: "+e.ToString(), "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            
            return true;
        }


        

       

        public string Update(string sql, string connection_string)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    connection.Open();
                    adapter.UpdateCommand = new SqlCommand(sql, connection);
                    adapter.UpdateCommand.ExecuteNonQuery();
                    //MessageBox.Show("Row updated !! ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return sql;
        }

        


        
        public override void Cleanup()
        {

        }

    }
}
