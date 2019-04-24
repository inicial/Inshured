using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using FileFromToFTP_NEW;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;

namespace rep6050
{


    public partial class frmMain : Form
    {
        const string insertPersonalArea = @"insert into Lanta_PersonalArea([pa_TU_Key]
                                                                           ,[pa_DG_Code]
                                                                           ,[pa_ddgID]
                                                                           ,[pa_Number]
                                                                           ,[pa_FileName]
                                                                           ,[pa_UserUpdate]
                                                                           ,[pa_Description])
                                            Values (@Tu_key,
                                                    @dg_code,
                                                    @ddgId,
                                                    @Number,
                                                    @FileName,
                                                    (select top 1 US_FullName from UserList where US_USERID = SUSER_SNAME()),
                                                    ' ')";
        private DataTable _turists= new DataTable(),_servise=new DataTable(),_dogovor = new DataTable(),_ins= new DataTable();
        private Decimal curs;
        private SqlConnection _con=new SqlConnection();
        private Insured ins;
        

        public void bordero(DateTime from, DateTime to)
        {
            DateTime borderoDate = DateTime.Now.Date;
#if DEBUG
            borderoDate = new DateTime(2014,05,04);
#endif
            Microsoft.Office.Interop.Excel.Worksheet ObjWorkSheet;
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                "lanta.sqlconfig.dll.config");
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap,
                ConfigurationUserLevel.None);
            string path = config.AppSettings.Settings["BorderoPath"].Value;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var files =Directory.GetFiles(path);
            foreach (string file in files)
            {
                File.Delete(file);
            }
            DataTable _buffer = new DataTable();
            SqlCommand _com = new SqlCommand(@"SELECT [Id]
                                            ,[INS_Numder]
                                            ,[INS_Holder]
                                            ,[INS_BirthdayHolder]
                                            ,[INS_Person]
                                            ,[INS_BirthdayPerson]
                                            ,[INS_Country]
                                            ,[INS_Date]
                                            ,[INS_DateBegin]
                                            ,[INS_DateEnd]
                                            ,[INS_Duration]
                                            ,[INS_Dop]
                                            ,[INS_Program]
                                            ,[INS_Code]
                                            ,[INS_Sum]
                                            ,[INS_Currency]
                                            ,[INS_Prem]
                                            ,[INS_CurruncyPrem]
                                            ,[INS_PremRb]
                                            ,[INS_Rule]
                                            ,[INS_DateChange]
                                            ,[INS_Status]
                                             FROM [dbo].[URS_Insurance]
                                             where [INS_DateChange] between @p1 AND @p2", _con);
            _com.Parameters.AddWithValue("@p1", from.Date);
            _com.Parameters.AddWithValue("@p2", to.Date);
            SqlDataAdapter adapter = new SqlDataAdapter(_com);
            adapter.Fill(_buffer);
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook bordero = app.Workbooks.Add();
            Microsoft.Office.Interop.Excel.Workbook cancels = app.Workbooks.Add();
            // ObjWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)bordero.Sheets[1];
            //ObjWorkSheet.Cells[1, 1] = ;
            //
            int p = 1;
            bordero.Sheets[1].Cells[p, 1] = "Номер полиса";
            bordero.Sheets[1].Cells[p, 2] = "ФИО страхователя";
            bordero.Sheets[1].Cells[p, 3] = "Дата рождения страхователя";
            bordero.Sheets[1].Cells[p, 4] = "ФИО застрахованного";
            bordero.Sheets[1].Cells[p, 5] = "Дата рождения застрахованного";
            bordero.Sheets[1].Cells[p, 6] = "Страна пребывания";
            bordero.Sheets[1].Cells[p, 7] = "Дата оформления полиса";
            bordero.Sheets[1].Cells[p, 8] = "Дата начала действия полиса";
            bordero.Sheets[1].Cells[p, 9] = "Дата окончания действия полиса";
            bordero.Sheets[1].Cells[p, 10] = "Продолжи-тельность поездки";
            bordero.Sheets[1].Cells[p, 11] = "Дополнительные условия";
            bordero.Sheets[1].Cells[p, 12] = "Программа (вариант) страхования";
            bordero.Sheets[1].Cells[p, 13] = "Коды Застрахованных";
            bordero.Sheets[1].Cells[p, 14] = "Страховая сумма";
            bordero.Sheets[1].Cells[p, 15] = "Валюта страхования";
            bordero.Sheets[1].Cells[p, 16] = "Страховая премия в валюте";
            bordero.Sheets[1].Cells[p, 17] = "Валюта премии";
            bordero.Sheets[1].Cells[p, 18] = "Страховая премия в рублях";
            bordero.Sheets[1].Cells[p, 19] = "Правила";
            p++;
            for (int i = 1; i <= 19; i++)
            {
                bordero.Sheets[1].Cells[p, i] = i.ToString();
            }

            p++;

            
            foreach (DataRow row in _buffer.Rows)
            {
                if (row.Field<bool>("INS_Status"))
                {
                    bordero.Sheets[1].Cells[p, 1] = row.Field<string>("INS_Numder");
                    bordero.Sheets[1].Cells[p, 2] = row.Field<string>("INS_Holder");
                    bordero.Sheets[1].Cells[p, 3] =
                        row.Field<DateTime>("INS_BirthdayHolder").Date.ToString("dd.MM.yyyy");
                    bordero.Sheets[1].Cells[p, 4] = row.Field<string>("INS_Person");
                    bordero.Sheets[1].Cells[p, 5] = row.Field<DateTime>("INS_BirthdayPerson")
                                                       .Date.ToString("dd.MM.yyyy");
                    bordero.Sheets[1].Cells[p, 6] = row.Field<string>("INS_Country");
                    bordero.Sheets[1].Cells[p, 7] = row.Field<DateTime>("INS_Date").Date.ToString("dd.MM.yyyy");
                    bordero.Sheets[1].Cells[p, 8] = row.Field<DateTime>("INS_DateBegin").Date.ToString("dd.MM.yyyy");
                    bordero.Sheets[1].Cells[p, 9] = row.Field<DateTime>("INS_DateEnd").Date.ToString("dd.MM.yyyy");
                    bordero.Sheets[1].Cells[p, 10] = row.Field<int>("INS_Duration").ToString();
                    bordero.Sheets[1].Cells[p, 11] = row.Field<string>("INS_Dop");
                    bordero.Sheets[1].Cells[p, 12] = row.Field<string>("INS_Program");
                    bordero.Sheets[1].Cells[p, 13] = row.Field<string>("INS_Code");
                    bordero.Sheets[1].Cells[p, 14] = row.Field<decimal>("INS_Sum").ToString("N2");
                    bordero.Sheets[1].Cells[p, 15] = row.Field<string>("INS_Currency");
                    bordero.Sheets[1].Cells[p, 16] = row.Field<decimal>("INS_Prem").ToString("N2");
                    bordero.Sheets[1].Cells[p, 17] = row.Field<string>("INS_CurruncyPrem");
                    bordero.Sheets[1].Cells[p, 18] = row.Field<decimal>("INS_PremRb").ToString("N2");
                    bordero.Sheets[1].Cells[p, 19] = row.Field<string>("INS_Rule");
                    ;
                    p++;
                }
            }
            app.Visible = true;
            bordero.SaveAs(path+"Bordero.xls", XlFileFormat.xlWorkbookNormal);
            bordero.Close();
            p = 1;
            cancels.Sheets[1].Cells[p, 1] = "Номер аннулированного полиса";
            cancels.Sheets[1].Cells[p, 2] = "ФИО Страхователя";
            cancels.Sheets[1].Cells[p, 3] = "Дата оформления полиса";
            cancels.Sheets[1].Cells[p, 4] = "Дата начала действия полиса";
            cancels.Sheets[1].Cells[p, 5] = "Дата аннулирования";
            cancels.Sheets[1].Cells[p, 6] = "Страховая сумма по риску отмена поездки (CTI) в аннулированном полисе";
            cancels.Sheets[1].Cells[p, 7] = "Валюта страхования";
            cancels.Sheets[1].Cells[p, 8] = "Номер полиса, оформленного вместо аннулированного";
            cancels.Sheets[1].Cells[p, 9] = "ФИО Страхователя";
            cancels.Sheets[1].Cells[p, 10] = "Дата оформления полиса";
            cancels.Sheets[1].Cells[p, 11] = "Дата начала действия полиса";
            cancels.Sheets[1].Cells[p, 12] = "Страховая сумма по риску отмена поездки (CTI) в новом полисе";
            cancels.Sheets[1].Cells[p, 13] = "Валюта страхования";
            p++;
            cancels.Sheets[1].Cells[p, 1] = "1";
            cancels.Sheets[1].Cells[p, 2] = "2";
            cancels.Sheets[1].Cells[p, 3] = "7";
            cancels.Sheets[1].Cells[p, 4] = "8";
            cancels.Sheets[1].Cells[p, 5] = "10";
            cancels.Sheets[1].Cells[p, 6] = "14";
            cancels.Sheets[1].Cells[p, 7] = "15";
            p ++;
            List<AnnulIns> canc = new List<AnnulIns>();
           
            foreach (DataRow row in _buffer.Rows)
            {
                if ((!row.Field<bool>("INS_Status")))
                {
                    AnnulIns annul = null;
                    foreach (AnnulIns item in canc)
                    {
                        if (item.nomann == row.Field<string>("INS_Numder"))
                        {
                            annul = item;
                            break;
                        }
                       
                    }
                    if (annul == null)
                    {
                        annul = new AnnulIns();
                        canc.Add(annul);
                    }
                    annul.nomann = row.Field<string>("INS_Numder");
                    annul.FiOann = row.Field<string>("INS_Holder");
                    annul.datecrann = row.Field<DateTime>("INS_Date").Date.ToString("dd.MM.yyyy");
                    annul.datenachann = row.Field<DateTime>("INS_DateBegin").Date.ToString("dd.MM.yyyy");
                    annul.dateann = row.Field<DateTime>("INS_DateChange").Date.ToString("dd.MM.yyyy");
                    if (row.Field<string>("INS_Program") == "CTI")
                    {
                        if ((annul.sumann == string.Empty) || (Convert.ToDecimal(annul.sumann) < row.Field<decimal>("INS_Sum")))
                        {annul.sumann = row.Field<decimal>("INS_Sum").ToString("N2");}
                        annul.valann = row.Field<string>("INS_Currency");
                        DataRow[] sel = _buffer.Select("INS_Status=1 and INS_Program ='CTI'");
                        if (sel.Length > 0)
                        {
                            annul.nomnov = sel[0].Field<string>("INS_Numder");
                            annul.datecrnov = sel[0].Field<DateTime>("INS_Date").Date.ToString("dd.MM.yyyy");
                            annul.FiOnov = sel[0].Field<string>("INS_Holder");
                            annul.datenachnov = sel[0].Field<DateTime>("INS_DateBegin").Date.ToString("dd.MM.yyyy");
                            annul.valnov = sel[0].Field<string>("INS_Currency");
                            if ((annul.sumnov == string.Empty) || (Convert.ToDecimal(annul.sumnov) < row.Field<decimal>("INS_Sum")))
                            {annul.sumnov = sel[0].Field<decimal>("INS_Sum").ToString("N2");}

                        }
                    }
                }
            }
            string dateborderotext = string.Empty;
            if (from.Date.AddDays(1) == to.Date)
            {
                dateborderotext = from.Date.ToString().Substring(0, 10);
            }
            else
            {
                dateborderotext = "с " + from.Date.ToString().Substring(0, 10) + " по " +
                                  to.Date.AddDays(-1).ToString().Substring(0, 10);
            }
            
            if (canc.Count < 1)
            {
                
                cancels.Sheets[1].Cells[p, 1] = dateborderotext + " полисы не аннулировались";
            }
            else
            {
                foreach (AnnulIns annulInse in canc)
                {
                    cancels.Sheets[1].Cells[p, 1] = annulInse.nomann;
                    cancels.Sheets[1].Cells[p, 2] = annulInse.FiOann;
                    cancels.Sheets[1].Cells[p, 3] = annulInse.datecrann;
                    cancels.Sheets[1].Cells[p, 4] = annulInse.datenachann;
                    cancels.Sheets[1].Cells[p, 5] = annulInse.dateann;
                    cancels.Sheets[1].Cells[p, 6] = annulInse.sumann;
                    cancels.Sheets[1].Cells[p, 7] = annulInse.valann;
                    cancels.Sheets[1].Cells[p, 8] = annulInse.nomnov;
                    cancels.Sheets[1].Cells[p, 9] = annulInse.FiOnov;
                    cancels.Sheets[1].Cells[p, 10] = annulInse.datecrnov;
                    cancels.Sheets[1].Cells[p, 11] = annulInse.datenachnov;
                    cancels.Sheets[1].Cells[p, 12] = annulInse.sumnov;
                    cancels.Sheets[1].Cells[p, 13] = annulInse.valnov;
                    p++;
                }
            }
            cancels.SaveAs(path+"cancels.xls", XlFileFormat.xlWorkbookNormal);
            cancels.Close();
            app.Quit();
            
            System.IO.FileStream fs = new System.IO.FileStream(path+"Bordero.xls", System.IO.FileMode.Open);
            byte[] hashFile = new byte[fs.Length];
            fs.Read(hashFile, 0, hashFile.Length);
            fs.Close();
            Stream bord = new MemoryStream(hashFile);
            System.IO.File.Delete(path+"Bordero.xls");

            fs = new FileStream(path+"cancels.xls", System.IO.FileMode.Open);
            hashFile = new byte[fs.Length];
            fs.Read(hashFile, 0, hashFile.Length);
            fs.Close();
            System.IO.File.Delete(path+"cancels.xls");
            Stream cancl = new MemoryStream(hashFile);
            

            SmtpClient Smtp = new SmtpClient(config.AppSettings.Settings["BorderoMailServer"].Value, 25);
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress(config.AppSettings.Settings["BorderoMailFrom"].Value);
            foreach (string mail in config.AppSettings.Settings["BorderoMailTo"].Value.Split(';'))
            {
                Message.To.Add(new MailAddress(mail));
            }
            Message.Subject = "Бордеро " + dateborderotext;
           // Message.Body = "Сообщение";
            Message.Attachments.Add(new Attachment(bord, "Bordero_MK_" + dateborderotext.Replace(" ","_") + ".xls"));
            Message.Attachments.Add(new Attachment(cancl, "Annul_MK_" + dateborderotext.Replace(" ","_") + ".xls"));
            Smtp.Send(Message);
        }

        public frmMain(SqlConnection connection,string dgCode)
        {
            InitializeComponent();
            _con = connection;
            tbDgCode.Text = dgCode;
            // GetDate();
            
             
        }
        
        bool CreateInshured(List<int>persons )
        {
            ins = new Insured(_con);
            bool isCancels = false;
            Decimal med = 0,cancel = 0,
                tarifMed =Convert.ToDecimal(0.79);
            ins.DG_code = tbDgCode.Text;
            if (_servise.Rows.Count < 1)
            {
                MessageBox.Show("Не найдена услуга страховка или к ней не привязаны туристы");
                return false;

            }
            
            List<InshuredPers> pers = new List<InshuredPers>();
            foreach (DataRow row in _servise.Rows)
            {

                if (!(persons.IndexOf(row.Field<int>("tu_key")) >= 0)) { continue; }
                string fname = row.Field<string>("TU_NAMELAT"),
                       name = row.Field<string>("TU_FNAMELAT");
                int tu_key = row.Field<int>("Tu_key");
                DateTime birdhday = row.Field<DateTime>("TU_BIRTHDAY");
                InshuredPers per = null;
                if (pers.Count<=0)
                {
                    per = new InshuredPers(fname+" "+name, birdhday,tu_key);
                    pers.Add(per);
                   
                }
                foreach (InshuredPers inshured in pers)
                {
                    if ((inshured.tu_key == tu_key))
                    {
                        per = inshured;
                        break;
                    }
                    
                }
               if (per == null)
               {
                   per = new InshuredPers(fname+" "+name, birdhday,tu_key);
                   pers.Add(per);
                                
                        
               }
               if ((row.Field<int>("DL_CODE") == 777000695) || (row.Field<int>("DL_CODE") == row.Field<int>("AC_slkey")))
                {
                    per.sumMed = tarifMed * Convert.ToInt32(row.Field<Int16>("DL_NDAYS")) *
                               Convert.ToDecimal(row.Field<double>("AC_Coef"));
                    per.sumMedRb = per.sumMed * curs;
                    med = med + per.sumMed;
                }
               if ((row.Field<int>("DL_CODE") == 76636 )||(row.Field<int>("DL_CODE") == 76636 ) )
                {
                    decimal percent ;
                    isCancels = true;
                    per.sumIns = Convert.ToDecimal(row.Field<string>("A1_name"));
                    if (per.sumIns < Convert.ToDecimal(5000))
                    {
                        percent = Convert.ToDecimal(0.02);
                    }
                    else
                    {
                        percent = Convert.ToDecimal(0.035);
                    }
                    
                    per.sumVal = per.sumIns*percent;
                    per.deductible = 0;
                    cancel = cancel + per.sumVal;
                    per.sumRb = per.sumVal*curs;
                }
                
            }
            ins.persons = pers;
            bool flag = false;
            foreach (DataRow row in _turists.Rows)
            {
                if (persons.IndexOf(row.Field<int>("tu_key"))>=0)
                {
                    if (row.Field<int>("age") < 18) continue;
                    flag = true;
                    if (row.Field<string>("TU_PHONE") != null)
                    {
                        ins.tel = row.Field<string>("TU_PHONE");
                    }
                    else
                    { 
                        ins.tel = string.Empty;
                    }

                    ins.passport = row.Field<string>("TU_PASPORTTYPE") + ' ' + row.Field<string>("TU_PASPORTNUM");
                    ins.holder = row.Field<string>("TuristName");
                    ins.holderBirthday = row.Field<DateTime>("TU_BIRTHDAY");
                    break;
                }
            }
            if (!flag)
            {
                MessageBox.Show("Ни один из страхуемых не подходит для держателя полиса!");
                return false;
                
            }
            
            
            int i=0;
            bool flag1 = false;
            for (i = 0; i < _servise.Rows.Count; i++)
            {
                if (((_servise.Rows[i].Field<int>("DL_CODE") == 777000695) || (_servise.Rows[i].Field<int>("DL_CODE") == _servise.Rows[i].Field<int>("AC_slkey"))) && (persons.IndexOf(_servise.Rows[i].Field<int>("TU_key")) >= 0))
                {
                   
                    flag1 = true; 
                    break;
                }
            }
            if (!flag1)
            {
                return false;
            }
           
            ins.dateFrom = _dogovor.Rows[0].Field<DateTime>("DG_TURDATE").Date.AddDays(Convert.ToInt32(_servise.Rows[i].Field<Int16>("DL_DAY"))-1);
            ins.days = Convert.ToInt32(_servise.Rows[i].Field<Int16>("DL_NDAYS"));
            ins.dateTo = ins.dateFrom.Date.AddDays(ins.days-1);
            ins.dateIsue = DateTime.Now.Date;
            ins.terretory = "Все страны мира (кроме России и стран СНГ, стран гражданства)/World wide (except Russia and CIS countries, citizenship countries)";
            ins.medicalsum = 50000;
            ins.tripsum = 0;
            ins.fligsum = 1000;
            ins.bagsum = 0;
            if (isCancels)
            {
                ins.dop =ins.dop + " Начало поездки : " + ins.dateFrom.ToString().Substring(0, 10);
            }
            ins.dop = ins.dop + string.Format(" COVERED ONLY ({0}) DAYS",ins.days);
           
            ins.program = "B";
            ins.nomber = GenNomber();
            ins.medicalprem = med;
            ins.medicalpremRb = ins.medicalprem*curs;
            ins.totalsum = med + cancel;
            ins.totalsumRb = ins.totalsum*curs;
            if (_dogovor.Rows[0].Field<string>("DG_RATE") == "Eu")
            {
                ins.curens = "EUR";
            }
            else
            {
                if (_dogovor.Rows[0].Field<string>("DG_RATE") == "$")
                {
                    ins.curens = "USD";
                }
                else
                {
                    MessageBox.Show("Не подходящая валюта путевки!");
                    return false;
                    //ins.curens = _dogovor.Rows[0].Field<string>("DG_RATE");
                }
                
            }
            foreach (var inshuredPerse in pers)
            {
                if (inshuredPerse.sumMed <= 0)
                {
                    MessageBox.Show("У туриста " + inshuredPerse.Name + " Нет медицинской страховки! Либо выбрана неправельно по возрасту!");
                    return false;
                }
            }
            
            if (
                MessageBox.Show("Отредактировать даты с/по ?", "Редактирование страховки?", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //вызов формы редактирования дат
                frmDateCorrect.DayCorrect(ref ins.dateFrom,ref  ins.dateTo); ;
            }
            return true;

        }
        void updateInsGrid()
        {
            dgvInsured.DataSource = _ins;
            foreach (DataGridViewColumn column in dgvInsured.Columns)
            {
                switch (column.Name.ToLower())
                {
                    case "ins_numder":
                        {
                            column.DisplayIndex = 0;
                            column.HeaderText = "Номер страховки";

                        }
                        break;
                    case "status":
                        {
                            column.DisplayIndex = 1;
                            column.HeaderText = "Статус";
                        }
                        break;
                    default:
                        {
                            column.Visible = false;
                        }
                        break;
                }
            }
        }
       
        void updateTuristGrid()
        {
            dgvTurists.DataSource = _turists;
            
            foreach (DataGridViewColumn column in dgvTurists.Columns)
            {
               
                switch (column.Name.ToLower())
                {
                    case "turistname":
                        {
                            column.DisplayIndex = 0;
                            column.HeaderText = "ФИО туриста";
                            //column.Width = 200;
                        }
                        break;
                    case "tu_birthday":
                        {
                            column.DisplayIndex = 1;
                            column.HeaderText = "Дата рождения";
                        }
                        break;
                    case "age":

                        {
                            column.DisplayIndex = 2;
                            column.HeaderText = "Возраст на момент поездки";
                        }
                        break;
                    case "ins_number":
                        {
                            column.DisplayIndex = 3;
                            column.HeaderText = "Номер страховки";
                           // column.Width = 200;
                        }
                        break;
                    default:
                        {
                            column.Visible = false;
                        }
                        break;
                }
            }
           // dgvTurists.SelectAll();
           
        }

        
        string GenNomber()
        { 
            int nom=1;
            SqlCommand com = new SqlCommand("URS_GetNomber",_con);
            com.CommandType= CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(com);
            DataTable tab = new DataTable();
            adapter.Fill(tab);
            nom = tab.Rows[0].Field<int>("nomer");
            String nomber = "019/" + DateTime.Now.Date.Year.ToString().Substring(2, 2) + "/3MKGV" + string.Format("{0:d8}", nom);
            

            return nomber;
        }
    
        public  void GetDate()
        {
            _dogovor.Clear();
            _servise.Clear();
            _turists.Clear();
            _ins.Clear();
            string selectIns = @"SELECT distinct
                                            [INS_Numder]
                                            ,[INS_Status]
                                             ,case  when INS_Status=1 then 'Выписана' when  INS_Status=0 then 'Аннулирована' end as [status]
                                             FROM [dbo].[URS_Insurance]
                                             where INS_DGCode=@p1";
            SqlCommand comIns = new SqlCommand(selectIns, _con);
            SqlDataAdapter adapterIns = new SqlDataAdapter(comIns);
            comIns.Parameters.AddWithValue("@p1", tbDgCode.Text);
            adapterIns.Fill(_ins);
            string selectTurist = @"select distinct 
                                    TU_KEY,
                                    DG_CODE,
                                    TU_NAMELAT+' '+TU_FNAMELAT as TuristName,
                                    TU_BIRTHDAY,
                                    DG_NDAY,
                                    DG_TURDATE,
                                    TU_PHONE,
                                    TU_PASPORTNUM,
                                    TU_PASPORTTYPE,
                                    isnull(ins_numder,'') as ins_number,
                                    dbo.GetYears(TU_BIRTHDAY,DG_TURDATE + DG_NDAY - 1) as age
                                    from tbl_Turist inner join 
                                    tbl_Dogovor on tbl_Turist.TU_DGCOD =tbl_Dogovor.DG_CODE 
                                    left join URS_Insurance on INS_tukey=TU_KEY and INS_Status = 1
                                    where  DG_code = @p1";
                                    // datediff(day,TU_BIRTHDAY,DG_TURDATE) /365  as age
            SqlCommand comTur = new SqlCommand(selectTurist,_con);
            SqlDataAdapter adapterTur = new SqlDataAdapter(comTur);
            comTur.Parameters.AddWithValue("@p1",tbDgCode.Text );
            adapterTur.Fill(_turists);
            string seldog = @"SELECT [DG_CODE]
                            ,[DG_TURDATE]
                            ,[DG_NMEN]
                            ,[DG_PRICE]
                            ,[DG_NDAY]
                            ,[DG_MAINMEN]
                            ,[DG_MAINMENPHONE]
                            ,[DG_MAINMENPASPORT]
                            ,[DG_RATE] 
                             FROM [dbo].[tbl_Dogovor]
                             where DG_CODE =@p1";
            SqlCommand comDogovor = new SqlCommand(seldog,_con);
            comDogovor.Parameters.AddWithValue("@p1", tbDgCode.Text); 
            SqlDataAdapter adapterDogovor = new SqlDataAdapter(comDogovor);
            adapterDogovor.Fill(_dogovor);
            if(!String.IsNullOrEmpty(_dogovor.Rows[0].Field<string>("DG_RATE")))
            {
                SqlCommand course = new SqlCommand(@"select RC_COURSE_CB from RealCourses
            where RC_RCOD1 = 'рб' and RC_DATEBEG = @p1 and RC_RCOD2 = @p2", _con);
                string p1 = _dogovor.Rows[0].Field<string>("DG_RATE");
                //DateTime p2 = DateTime.Now.Date;
                if (p1 != "рб")
                {
                    course.Parameters.AddWithValue("@p2", _dogovor.Rows[0].Field<string>("DG_RATE"));
                    course.Parameters.AddWithValue("@p1", DateTime.Now.Date);
                    SqlDataAdapter adapterCours = new SqlDataAdapter(course);
                    DataTable _cours = new DataTable();
                    adapterCours.Fill(_cours);
                    curs = _cours.Rows[0].Field<decimal>("RC_COURSE_CB");
                }
                else
                {
                    curs = 1;
                    MessageBox.Show("Страховки по России выписываются через систему страховой компании.");
                    this.Close();
                }

            }
            string selectServis = @"SELECT DL_DGCOD
                              , DL_NAME
                              , DL_DAY
                              , DL_CODE
                              , DL_SUBCODE1
                              , DL_SVKEY
                              , DL_NDAYS
                              , tu_key
                              , TU_TURDATE
                              , TU_NAMELAT
                              , TU_FNAMELAT
                              , TU_BIRTHDAY
                              , TU_SEX
                              , TU_RealSex
                              , DL_COST
                              , A1_NAME
                              , AC_Coef
                              , AC_slkey
                  FROM
                 TuristService
                 INNER JOIN tbl_Turist ON TU_TUKEY = TU_KEY
                 INNER JOIN tbl_DogovorList ON DL_KEY = TU_DLKEY
                 inner join AddDescript1 on DL_SUBCODE1=A1_KEY 
                 inner join INS_AgeCoef on DATEDIFF(day,TU_BIRTHDAY,DL_TURDATE)/365 between AC_AgeFrom and AC_AgeTo  
                 WHERE
                 DL_SVKEY = 6
                 AND DL_DGCOD = @p1 ";
            SqlCommand comSer = new SqlCommand(selectServis,_con);
            comSer.Parameters.AddWithValue("@p1", tbDgCode.Text);
            SqlDataAdapter adapterSer = new SqlDataAdapter(comSer);
            adapterSer.Fill(_servise);
            updateTuristGrid();
            updateInsGrid();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (dgvInsured.SelectedRows.Count > 0)
            {
                
                DateTime date;

                string insNomber = dgvInsured.SelectedRows[0].Cells["INS_Numder"].Value.ToString();
                using (SqlCommand com1 = new SqlCommand("select top 1 ins_date from URS_Insurance where INS_Numder=@number",_con))
                {
                    com1.Parameters.AddWithValue("@number", insNomber);
                    date = (DateTime) com1.ExecuteScalar();
                }
                string message = string.Format("Данная страховка находится в статусе обработано{0}.\n Вы действительно хотите данную страховку аннулировать?",date.ToString("dd.MM.yy"));
                if (MessageBox.Show(message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {


                    SqlCommand com =
                        new SqlCommand(
                            "Update URS_Insurance  set INS_DateChange=@p1 , INS_Status = @p2 where INS_Numder=@p3 and INS_Status=1 ",
                            _con);
                    com.Parameters.AddWithValue("@p1", DateTime.Now);
                    com.Parameters.AddWithValue("@p2", false);
                    com.Parameters.AddWithValue("@p3", insNomber);
                    com.ExecuteNonQuery();
                    InsDelFromFTP(dgvInsured.SelectedRows[0].Cells["INS_Numder"].Value.ToString());
                    GetDate();
                    //updateInsGrid();
                    string insertHistory = @"insert into History(HI_DGCOD,HI_DATE,HI_WHO,HI_TEXT,HI_MOD) Values
                                    (@dg_code,
                                     GetDate(),
                                     (select top 1 isnull(US_FullName,'Администратор') from UserList where US_USERID = SUSER_SNAME()),
                                     @text,
                                     @mod)";
                    using (com = new SqlCommand(insertHistory, _con))
                    {
                        com.Parameters.AddWithValue("@dg_code", tbDgCode.Text);
                        com.Parameters.AddWithValue("@text", "Аннулирована страховка № " + insNomber);
                        com.Parameters.AddWithValue("@mod", "CAN");
                        com.ExecuteNonQuery();

                    }
                }
            }
            else
            {
                MessageBox.Show("Сначало выберете страховку");
            }
        }

        private bool InsToFTP(Insured insured)
        {
            try
            {
                ins.Print(false);
                ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
                configMap.ExeConfigFilename = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                    "lanta.sqlconfig.dll.config");
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap,
                    ConfigurationUserLevel.None);
                string servername =  config.AppSettings.Settings["ftp"].Value ;
                WorkWithFTP ftp = new WorkWithFTP(servername);
                string rError;

                if (ftp.GetFilesOnFTPAndCreateNewDir(insured.DG_code, out rError) != WorkWithFTP.FTP_ERROR.ERROR_NO)
                {
                    return false;
                }
                string newnamefile = "MK_" + DateTime.Now.ToString("yyMMdd") + (new Random()).Next().ToString();
                string filepath =tbDgCode.Text ;
                string filename =  "d:\\Insurense\\"+insured.nomber.Replace('/', ' ') + ".pdf";
                if(ftp.Upload(filepath,filename,newnamefile,out rError)!=WorkWithFTP.FTP_ERROR.ERROR_NO)
                {
                    return false;
                }
                File.Delete(filename);
                //Запись в базу приклепления файлов в личный кабинет
                foreach (InshuredPers c in insured.persons)
                {
                    int tu_key = c.tu_key;
                    using (SqlCommand com = new SqlCommand(insertPersonalArea,_con))
                    {
                        com.Parameters.AddWithValue("@tu_key", tu_key);
                        com.Parameters.AddWithValue("@dg_code", insured.DG_code);
                        com.Parameters.AddWithValue("@ddgid", 100600);
                        com.Parameters.AddWithValue("@Number", 1);
                        com.Parameters.AddWithValue("@FileName", newnamefile+".pdf");
                        com.ExecuteNonQuery();
                    }


                }
                //FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ftpPath);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        private void btnCreate_Click(object sender, EventArgs e)
        {
            //foreach (DataRow row in _ins.Rows)
            //{
            //    if (row.Field<bool>("INS_Status"))
            //    {
            //        MessageBox.Show("Уже есть выписанная страховка!");
            //        return;
            //    }
            //}
            if (dgvTurists.SelectedRows.Count < 1)
            {
                MessageBox.Show("Выбирете сначала туристов для страхования");
                return;
            }

            List<int> persons = new List<int>();
            String mesage = "У вас выбраны туристы для страхования:" + Convert.ToChar(13);
            foreach (DataGridViewRow selectedRow in dgvTurists.SelectedRows)
            {
                if (Convert.ToString(selectedRow.Cells["ins_number"].Value)==string.Empty)
                {
                    persons.Add(Convert.ToInt32(selectedRow.Cells["TU_KEY"].Value));
                    mesage += selectedRow.Cells["TuristName"].Value.ToString() + Convert.ToChar(13);
                }
                else
                {
                    MessageBox.Show("У  " + selectedRow.Cells["TuristName"].Value.ToString() + " уже есть страховка!");
                    return;
                }
            }
            if (persons.Count > 4)
            {
                MessageBox.Show("В страховке более 4 человек!");
                return;
            }
            mesage += "Продолжить выписку?";
            if (MessageBox.Show(mesage, "Проверка списка застрахованных", MessageBoxButtons.OKCancel,MessageBoxIcon.Question) ==
                DialogResult.Cancel)
            {
                return;
            }
          

            if (CreateInshured(persons))
            {

                ins.FixIns();
#if !DEBUG          
                if (!InsToFTP(ins))
                {
                    MessageBox.Show("Страховка создана, но при загрузке в личный кабинет произошла ошибка!");

                }
#endif
#if DEBUG
            ins.Print(false);
#endif          

               }
            GetDate();

        }
        bool InsDelFromFTP(string insure)
        {
            try
            {
                string selTur = @"select top 1 ins_tukey,ins_dgcode from URS_Insurance where INS_numder='" + insure + "'";
                int tukey;
                string dgcod;
                using (SqlCommand com = new SqlCommand(selTur,_con))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(com);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    if (table.Rows.Count < 1)
                    {
                        return false;
                    }
                    tukey = table.Rows[0].Field<int>("ins_tukey");
                    dgcod = table.Rows[0].Field<string>("ins_dgcode");
                }
                string selFile =
                    @"select top 1 pa_FileName from Lanta_PersonalArea where pa_Number=1 and  pa_ddgID =100600 and pa_TU_Key =" +
                    tukey.ToString();
                string filename;
                using (SqlCommand com = new SqlCommand(selFile, _con))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(com);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    if (table.Rows.Count < 1)
                    {
                        return false;
                    }
                    filename = table.Rows[0].Field<string>("pa_FileName");
                    
                }
                ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
                configMap.ExeConfigFilename = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                    "lanta.sqlconfig.dll.config");
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap,
                    ConfigurationUserLevel.None);
                string servername = config.AppSettings.Settings["ftp"].Value;
                WorkWithFTP ftp = new WorkWithFTP(servername);
                string rErrror;

                ftp.Delete(dgcod, filename, out rErrror);
                string delPersonalArea =
                    @"delete from Lanta_PersonalArea where  pa_ddgID = 100600 and  pa_DG_Code ='" + dgcod +
                    "' and pa_FileName='" + filename + "'";
                using (SqlCommand com = new SqlCommand(delPersonalArea, _con))
                {
                    com.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                
            }
        }
        private void btnrecreate_Click(object sender, EventArgs e)
        {
            if (dgvInsured.SelectedRows.Count > 0)
            {
                if (!Convert.ToBoolean(dgvInsured.SelectedRows[0].Cells["INS_Status"].Value))
                {
                    MessageBox.Show("Эта страховку уже аннулирована необходимо создать новую либо выбрать неаннулированую");
                }
                else
                {
                    DataTable dt = new DataTable();
                    SqlCommand com = new SqlCommand(@"SELECT  *
                                             FROM [dbo].[URS_Insurance]
                                             where INS_Numder=@p1",_con);
                    com.Parameters.AddWithValue("@p1", dgvInsured.SelectedRows[0].Cells["INS_Numder"].Value.ToString());
                    SqlDataAdapter adapter = new SqlDataAdapter(com);
                    adapter.Fill(dt);                    
                    ins = new Insured(_con);
                    ins.nomber = dt.Rows[0].Field<string>("INS_Numder");
                    ins.holder = dt.Rows[0].Field<string>("INS_Holder");
                    ins.passport = dt.Rows[0].Field<string>("INS_PassportHolder");
                    ins.tel = dt.Rows[0].Field<string>("INS_PhoneHolder");
                    ins.curens = dt.Rows[0].Field<string>("INS_Currency");
                    ins.holderBirthday = dt.Rows[0].Field<DateTime>("INS_BirthdayHolder");
                    ins.dateIsue = dt.Rows[0].Field<DateTime>("INS_Date");
                    ins.dateFrom = dt.Rows[0].Field<DateTime>("INS_DateBegin");
                    ins.dateTo = dt.Rows[0].Field<DateTime>("INS_DateEnd");
                    ins.days =dt.Rows[0].Field<int>("INS_Duration");
                    ins.terretory = dt.Rows[0].Field<string>("INS_Country");
                    ins.medicalsum = 50000;
                    ins.tripsum = 0;
                    ins.fligsum = 1000;
                    ins.bagsum = 0;
                    
                    
                    List<InshuredPers> pers = new List<InshuredPers>();
                    foreach (DataRow row in dt.Rows)
                    {
                        InshuredPers per = null;
                        foreach (InshuredPers inshuredPerse in pers)
                        {
                            if (inshuredPerse.tu_key == row.Field<int>("INS_tukey"))
                            {
                                per = inshuredPerse;
                            }
      
                        }
                        if (per == null)
                        {
                            per = new InshuredPers(row.Field<string>("INS_Person"),
                                                   row.Field<DateTime>("INS_BirthdayPerson").Date, row.Field<int>("INS_tukey"));
                            pers.Add(per);
                        }
                        if (row.Field<string>("INS_Program") == "B")
                        {
                            ins.program = row.Field<string>("INS_Program");
                            ins.medicalsum  = row.Field<decimal>("INS_Sum");
                           
                            per.sumMed  = row.Field<decimal>("INS_Prem");
                            per.sumMedRb  = row.Field<decimal>("INS_PremRb");
                        }
                        if (row.Field<string>("INS_Program") == "CTI")
                        {


                            per.sumIns = row.Field<decimal>("INS_Sum");
                            per.sumRb = row.Field<decimal>("INS_PremRb");
                            per.sumVal = row.Field<decimal>("INS_Prem");
                        }
                     
                    }
                    ins.persons = pers;
                    decimal med=0, medrb=0, total=0, totalRb=0;
                    foreach (InshuredPers inshuredPerse in pers)
                    {
                        med += inshuredPerse.sumMed;
                        medrb += inshuredPerse.sumMedRb;
                        total += inshuredPerse.sumMed + inshuredPerse.sumVal ;
                        totalRb += inshuredPerse.sumMedRb + inshuredPerse.sumRb;

                    }
                    ins.medicalprem = med;
                    ins.medicalpremRb = medrb;
                    ins.totalsum = total;
                    ins.totalsumRb = totalRb;
                    ins.Print(true);
                }
                
            }
            else
            {
                MessageBox.Show("Сначало выберете страховку");
            }
        }    
    }

    public class AnnulIns
    {
        public string nomann = string.Empty;
        public string FiOann = string.Empty;
        public string datecrann = string.Empty;
        public string datenachann = string.Empty;
        public string dateann = string.Empty;
        public string sumann = string.Empty;
        public string valann = string.Empty;
        public string nomnov = string.Empty;
        public string FiOnov = string.Empty;
        public string datecrnov = string.Empty;
        public string datenachnov = string.Empty;
        public string sumnov = string.Empty;
        public string valnov = string.Empty;

    }
}
