using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;

namespace rep6050
{
    class Insured
    {
        public List<InshuredPers>  persons;
        public string nomber;
        public DateTime dateIsue;
        public DateTime dateFrom;
        public DateTime dateTo;
        public string holder;
        public string passport;
        public DateTime holderBirthday;
        public string tel;
        public string terretory;
        public int days;
        public decimal medicalsum;
        public decimal acidentsum;
        public decimal tripsum;
        public decimal bagsum;
        public decimal fligsum;
        public decimal medicalfran;
        public decimal acidentfran;
        public decimal tripfran;
        public decimal bagfran;
        public decimal fligfran;
        public decimal medicalprem;
        public decimal acidentprem;
        public decimal tripprem;
        public decimal bagprem;
        public decimal fligprem;
        public decimal totalsum;
        public decimal totalsumRb;
        public decimal medicalpremRb;
        public decimal acidentpremRb;
        public decimal trippremRb;
        public decimal bagpremRb;
        public decimal fligpremRb;
        public string program;
        public string dop="";
        public string curens;
        public string DG_code;
        private SqlConnection _connection;

        public Insured(SqlConnection con)
        {
            _connection=con;
        }
       
        void ReplaceInDoc(Document oDoc,string find,string replace)
        {
            var range = oDoc.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: find, ReplaceWith: replace, Replace: WdReplace.wdReplaceAll);
        }
        public void Print(bool flag)

        {
            //обьект пустого значения
            Object wMissing = System.Reflection.Missing.Value;
            //обьекты true  и  false
            Object wTrue = true;
            Object wFalse = false;

            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document oDoc = new Microsoft.Office.Interop.Word.Document();
            //app.Visible = true;
            Object docPath =Environment.CurrentDirectory +"\\..\\Shablon\\uralsib.dot";
           // oDoc.Protect(WdProtectionType.OnlyReading);
            oDoc.Password = "yuiyu";
            oDoc = app.Documents.Add(ref docPath, ref wMissing, ref wTrue, ref wFalse);
            
            //Замена переменных
              
           
            ReplaceInDoc(oDoc,"<--number-->",this.nomber);
            ReplaceInDoc(oDoc,"<--cur-->",this.curens);
            ReplaceInDoc(oDoc, "<--date-->", this.dateIsue.Date.ToString().Substring(0,10));
            ReplaceInDoc(oDoc, "<--holder-->", this.holder);
            ReplaceInDoc(oDoc, "<--passport-->", this.passport);
            ReplaceInDoc(oDoc, "<--phone-->", this.tel);
            ReplaceInDoc(oDoc, "<--terretory-->", this.terretory);
            ReplaceInDoc(oDoc, "<--datefrom-->", this.dateFrom.Date.ToString().Substring(0,10));
            ReplaceInDoc(oDoc, "<--dateto-->", this.dateTo.Date.ToString().Substring(0, 10));
            ReplaceInDoc(oDoc, "<--days-->", this.days.ToString());
            ReplaceInDoc(oDoc, "<--dop-->", this.dop);
            int count = this.persons.Count;
            while (this.persons.Count<4)
            {
                this.persons.Add(new InshuredPers("",DateTime.Now,0));
            }
            for (int i = 1; i <= 4; i++)
            {
                ReplaceInDoc(oDoc, "<--person" + i.ToString() + "-->", this.persons[i - 1].Name);
               if (this.persons[i - 1].Name == string.Empty)
               {
                   ReplaceInDoc(oDoc, "<--birth" + i.ToString() + "-->", "");
                   ReplaceInDoc(oDoc, "<--sum" + i.ToString() + "-->", "");
                   ReplaceInDoc(oDoc, "<--prem" + i.ToString() + "-->", "");
                   ReplaceInDoc(oDoc, "<--rb" + i.ToString() + "-->", "");
                   ReplaceInDoc(oDoc, "<--cur" + i.ToString() + "-->", "");
               }
               else
               {
                   ReplaceInDoc(oDoc, "<--birth" + i.ToString() + "-->", this.persons[i - 1].birhtDay.Date.ToString().Substring(0, 10));
                   ReplaceInDoc(oDoc, "<--sum" + i.ToString() + "-->",IsNull( this.persons[i - 1].sumIns));
                   ReplaceInDoc(oDoc, "<--prem" + i.ToString() + "-->",IsNull( this.persons[i - 1].sumVal));
                   ReplaceInDoc(oDoc, "<--rb" + i.ToString() + "-->", IsNull(this.persons[i - 1].sumRb));
                   if (IsNull(this.persons[i - 1].sumIns) == "--")
                   {
                       ReplaceInDoc(oDoc, "<--cur" + i.ToString() + "-->", "");
                   }
                   else
                   {
                       ReplaceInDoc(oDoc, "<--cur" + i.ToString() + "-->", this.curens);
                   }
                   
               }
                
            }
            while (this.persons.Count>count)
            {
                this.persons.Remove(persons.Last());
            }

            ReplaceInDoc(oDoc, "<--prog-->", this.program);
            ReplaceInDoc(oDoc, "<--total-->", this.totalsum.ToString("N2"));
            ReplaceInDoc(oDoc, "<--totalrb-->", this.totalsumRb.ToString("N2"));
            
            ReplaceInDoc(oDoc, "<--med-->", IsNull(this.medicalsum));
            ReplaceInDoc(oDoc, "<--trip-->", IsNull(this.tripsum));
            ReplaceInDoc(oDoc, "<--lug-->", IsNull(this.bagsum));
            ReplaceInDoc(oDoc, "<--lia-->", IsNull(this.fligsum));

            ReplaceInDoc(oDoc, "<--medpr-->", IsNull(this.medicalprem));
            ReplaceInDoc(oDoc, "<--trippr-->", IsNull(this.tripprem));
            ReplaceInDoc(oDoc, "<--lugpr-->", IsNull(this.bagprem));
            ReplaceInDoc(oDoc, "<--liapr-->", IsNull(this.fligprem));

            ReplaceInDoc(oDoc, "<--medrb-->", IsNull(this.medicalpremRb));
            ReplaceInDoc(oDoc, "<--triprb-->", IsNull(this.trippremRb));
            ReplaceInDoc(oDoc, "<--lugrb-->", IsNull(this.bagpremRb));
            ReplaceInDoc(oDoc, "<--liarb-->", IsNull(this.fligpremRb));
            ReplaceInDoc(oDoc,"-- "+this.curens,"--");
           
            //Сохранение в фаил
            SaveFileDialog file = new SaveFileDialog();
            file.Filter = "Pdf|*.pdf";
            file.DefaultExt = ".pdf";
            file.FileName = this.nomber.Replace('/', ' ');
            string fileName;
            if (flag)
            {
                if (file.ShowDialog() == DialogResult.OK)
                {
                    fileName = file.FileName;
                }
                else
                {
                    fileName = "d:\\" + this.nomber.Replace('/', ' ') + ".pdf";
                }
            }
            else
            {
                if (!Directory.Exists("d:\\Insurense\\"))
                {
                    Directory.CreateDirectory("d:\\Insurense\\");
                }
                fileName = "d:\\Insurense\\" + this.nomber.Replace('/', ' ') + ".pdf";
            }


            oDoc.SaveAs(fileName, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);
           
            oDoc.Close(false);
            app.Quit(false);

           

            
        }
        
       public void FixIns()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
              String insetIns = @"INSERT INTO [URS_Insurance]
                ([INS_Numder]
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
                ,INS_DGCode
                ,INS_PassportHolder
                ,INS_PhoneHolder
                ,INS_TUKEY)
             VALUES
                (@INS_Numder
                ,@INS_Holder
                ,@INS_BirthdayHolder
                ,@INS_Person
                ,@INS_BirthdayPerson
                ,@INS_Country
                ,@INS_Date
                ,@INS_DateBegin
                ,@INS_DateEnd
                ,@INS_Duration
                ,@INS_Dop
                ,@INS_Program
                ,@INS_Code
                ,@INS_Sum
                ,@INS_Currency
                ,@INS_Prem
                ,@INS_CurruncyPrem
                ,@INS_PremRb
                ,@INS_Rule
                ,@INS_DateChange
                ,@INS_Status
                ,@INS_DGCode
                ,@INS_PassportHolder
                ,@INS_PhoneHolder
                ,@ins_tukey)";
            SqlCommand com = new SqlCommand(insetIns,_connection);
            com.Parameters.AddWithValue("@INS_Numder",nomber);
            com.Parameters.AddWithValue("@INS_Holder", holder);
            com.Parameters.AddWithValue("@INS_BirthdayHolder",holderBirthday);
            com.Parameters.AddWithValue("@INS_Country", terretory);
            com.Parameters.AddWithValue("@INS_Date", dateIsue);
            com.Parameters.AddWithValue("@INS_DateBegin", dateFrom);
            com.Parameters.AddWithValue("@INS_DateEnd", dateTo);
            com.Parameters.AddWithValue("@INS_Duration", days);
            com.Parameters.AddWithValue("@INS_Dop", dop);
            com.Parameters.AddWithValue("@INS_Code", "ST");
            com.Parameters.AddWithValue("@INS_Currency", curens);
            com.Parameters.AddWithValue("@INS_CurruncyPrem", curens);
            com.Parameters.AddWithValue("@INS_Rule", "019");
            com.Parameters.AddWithValue("@INS_DateChange", DateTime.Now);
            com.Parameters.AddWithValue("@INS_Status", true);
            com.Parameters.AddWithValue("@INS_DGCode", DG_code);
            com.Parameters.AddWithValue("@INS_PassportHolder", passport);
            com.Parameters.AddWithValue("@INS_PhoneHolder",tel);
            com.Parameters.AddWithValue("@INS_Person","");
            com.Parameters.AddWithValue("@INS_BirthdayPerson",DateTime.Now);
            com.Parameters.AddWithValue("@INS_Program","");
            com.Parameters.AddWithValue("@INS_Sum",0);
            com.Parameters.AddWithValue("@ins_tukey", 0);
            com.Parameters.AddWithValue("@INS_Prem",0);
            com.Parameters.AddWithValue("@INS_PremRb",0);
            foreach (InshuredPers pers in persons)
            {
                com.Parameters["@INS_Person"].Value= pers.Name;
                com.Parameters["@INS_BirthdayPerson"].Value= pers.birhtDay;
                com.Parameters["@INS_Program"].Value= program;
                com.Parameters["@ins_tukey"].Value = pers.tu_key;
                com.Parameters["@INS_Sum"].Value= medicalsum;
                com.Parameters["@INS_Prem"].Value=pers.sumMed;
                com.Parameters["@INS_PremRb"].Value = pers.sumMedRb;
                
                com.ExecuteNonQuery();
                com.Parameters["@INS_Program"].Value = "CL";
                com.Parameters["@INS_Sum"].Value = fligsum;
                com.Parameters["@INS_Prem"].Value = 0;
                com.Parameters["@INS_PremRb"].Value = 0;
                com.ExecuteNonQuery();
                if (pers.sumIns > 0)
                {
                    com.Parameters["@INS_Program"].Value=  "CTI";
                    com.Parameters["@INS_Sum"].Value=  pers.sumIns;
                    com.Parameters["@INS_Prem"].Value=pers.sumVal;
                    com.Parameters["@INS_PremRb"].Value =pers.sumRb;
                    com.ExecuteNonQuery();
                }
            }

            string insertHistory = @"insert into History(HI_DGCOD,HI_DATE,HI_WHO,HI_TEXT,HI_MOD) Values
                                    (@dg_code,
                                     GetDate(),
                                     (select top 1 isnull(US_FullName,'Администратор') from UserList where US_USERID = SUSER_SNAME()),
                                     @text,
                                     @mod)";
           using (com= new SqlCommand(insertHistory,_connection))
           {
               com.Parameters.AddWithValue("@dg_code", this.DG_code);
               com.Parameters.AddWithValue("@text", "Выписана страховка № " + this.nomber);
               com.Parameters.AddWithValue("@mod","CRE");
               com.ExecuteNonQuery();
           }
                                     
        }

        string IsNull(decimal value)
            {
                if ((value == 0) ||(value == null))
                {
                    return "--";
                }
                else
                {
                    return value.ToString("N2");
                }
            }
     }
}
