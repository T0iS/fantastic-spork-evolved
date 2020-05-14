using EsportWiki_EIS0011.Database;
using EsportWiki_EIS0011.Database.FunctionalityClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsportWiki_EIS0011
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            refreshRows();
        }

        public void refreshRows(Collection<Person> pl = null)
        {

            Collection<Person> players;
            TablePlayers.Rows.Clear();
            if (pl == null)
            {
                players = PersonTable.Select();
            }
            else
            {
                players = pl;
            }
            foreach (Person p in players)
            {
                int n = TablePlayers.Rows.Add();
                TablePlayers.Rows[n].Cells[0].Value = p.Id.ToString();
                TablePlayers.Rows[n].Cells[1].Value = p.First_Name;
                TablePlayers.Rows[n].Cells[2].Value = p.Last_Name;
                TablePlayers.Rows[n].Cells[3].Value = p.Birth_Date.ToString();
                TablePlayers.Rows[n].Cells[4].Value = p.Role;
                TablePlayers.Rows[n].Cells[5].Value = p.Game_Id.Name;
                TablePlayers.Rows[n].Cells[6].Value = p.Team_Id.Name;


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            refreshRows();
        }

        private void TablePlayers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == TablePlayers.Columns["col_edit"].Index)
            {

                //detail!
                try
                {
                    string p_id = TablePlayers.Rows[e.RowIndex].Cells[0].Value.ToString();
                    Form2 f2 = new Form2("detail", p_id);
                    f2.ShowDialog();
                    refreshRows();
                }
                catch (Exception)
                {
                    MessageBox.Show("You need to select a valid row", "", MessageBoxButtons.OK);
                    return;
                }
            }
            else if (e.ColumnIndex == TablePlayers.Columns["col_del"].Index)
            {
                try
                {
                    if (DialogResult.Yes == MessageBox.Show("Do you want to delete the row?", "", MessageBoxButtons.YesNo))
                    {
                        //delete it!     
                        string p_id = TablePlayers.Rows[e.RowIndex].Cells[0].Value.ToString();
                        PersonTable.Delete(Int32.Parse(p_id));
                        refreshRows();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("You need to select a valid row", "", MessageBoxButtons.OK);
                    return;
                }
            
            }
            else if (e.ColumnIndex == TablePlayers.Columns["col_ed"].Index)
            {
                try {    
                    string p_id = TablePlayers.Rows[e.RowIndex].Cells[0].Value.ToString();
                    Form2 f2 = new Form2("edit", p_id);
                    f2.ShowDialog();
                    refreshRows();
                }
                catch (Exception)
                {
                    MessageBox.Show("You need to select a valid row", "", MessageBoxButtons.OK);
                    return;
                }
            }
        }

        private void addPlayerButton_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2("add");
            f.ShowDialog();
            refreshRows();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            var attr = searchBox.Text.ToString();

            var result = PersonTable.SelectByParameter(attr);
            refreshRows(result);

        }
    }
}
