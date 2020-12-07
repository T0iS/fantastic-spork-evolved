using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataLayer.Database;
using DataLayer.Database.FunctionalityClasses;

namespace WindowsFormsApp
{
    public partial class Form3 : Form
    {
        public Form3(int p_id)
        {
            InitializeComponent();

            var players = PersonTable.SelectTeammates(p_id);
            foreach (Person p in players)
            {
                int n = TablePlayers.Rows.Add();
                TablePlayers.Rows[n].Cells[0].Value = p.Id.ToString();
                TablePlayers.Rows[n].Cells[1].Value = p.First_Name;
                TablePlayers.Rows[n].Cells[2].Value = p.Last_Name;
                TablePlayers.Rows[n].Cells[3].Value = p.Birth_Date.ToString();
                TablePlayers.Rows[n].Cells[4].Value = p.Role;


            }
        }

        private void TablePlayers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
