using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace rep6050
{
    public partial class frmDateCorrect : Form
    {
        static DateTime beginDate=new DateTime(), endDate=new DateTime();
        private frmDateCorrect()
        {
            InitializeComponent();
            dtpBeginDate.Value = beginDate;
            dtpEndDate.Value = endDate;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        static public void DayCorrect(ref DateTime begin, ref DateTime end)
        {
            beginDate = begin;
            endDate = end;
            frmDateCorrect frm = new frmDateCorrect();
            frm.ShowDialog();
            begin = beginDate;
            end = endDate;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (dtpBeginDate.Value.Date < DateTime.Now.Date)
            {
                dtpBeginDate.Value = DateTime.Now.Date;
                MessageBox.Show("Дата начала не может быть раньше чем сегодня!");
                return;
            }
            if (dtpBeginDate.Value>dtpEndDate.Value)
            {
                DateTime bufer;
                bufer = dtpBeginDate.Value;
                dtpBeginDate.Value = dtpEndDate.Value;
                dtpEndDate.Value = bufer;
                MessageBox.Show("Дата начала не может быть позднее даты окончания!");
                return;
            }
            beginDate = dtpBeginDate.Value;
            endDate = dtpEndDate.Value;
            Close();
        }
    }
}
