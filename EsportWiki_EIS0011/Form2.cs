using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EsportWiki_EIS0011.Database;
using EsportWiki_EIS0011.Database.FunctionalityClasses;

namespace EsportWiki_EIS0011
{
    public partial class Form2 : Form
    {
        public Form2(bool b, string p_id = "")
        {
            InitializeComponent();

            if(b == true)
            {
                //add
                Player_add_edit.Visible = true;
                label2.Visible = false;
                button2.Visible = true;
                button1.Visible = false;
                textBox3.Visible = false;

                var games = GameTable.Select();
                foreach (Game g in games)
                {
                    listBox1.Items.Add(g.Name);
                }
                

                var teams = TeamTable.Select();
                foreach (Team t in teams)
                {
                    TeamList.Items.Add(t.Name);
                    listBox2.Items.Add(t.Name);
                }


            }
            if (b == false)
            {
                //edit
                Player_add_edit.Visible = false;
                label2.Visible = true;
                button1.Visible = true;
                button2.Visible = false;

                Person p = new Person();
                p = PersonTable.SelectOne(Int32.Parse(p_id));

                textBox1.Text = p.First_Name;
                textBox5.Text = p.Last_Name;
                textBox4.Text = p.Birth_Date.ToString();
                textBox2.Text = p.Role;
                textBox3.Text = p_id.ToString();
                textBox3.Visible = false;
                
                
                var games = GameTable.Select();
                foreach (Game g in games) {
                    listBox1.Items.Add(g.Name);
                }
                listBox1.SelectedIndex = p.Game_Id.Id-1;

                var teams = TeamTable.Select();
                foreach( Team t in teams)
                {
                    TeamList.Items.Add(t.Name);
                    listBox2.Items.Add(t.Name);
                }
                listBox2.SelectedIndex = p.Team_Id.Id - 1;
                
            }
        }

        private void TransferButton_Click(object sender, EventArgs e)
        {
            //transfer
            int p_id = Int32.Parse(textBox3.Text);
            int p_team = TeamList.SelectedIndex+1;
            PersonTable.Prestup(p_id, p_team);
            this.Close();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Do you really want to edit this player?", "", MessageBoxButtons.YesNo))
            {
                //edit player
                Person p = new Person();
                p.Id = Int32.Parse(textBox3.Text);
                p.First_Name = textBox1.Text;
                p.Last_Name = textBox5.Text;
                p.Birth_Date = Int32.Parse(textBox4.Text);
                p.Role = textBox2.Text;

                Team t = new Team();
                t.Id = listBox2.SelectedIndex + 1;
                p.Team_Id = t;

                Game g = new Game();
                g.Id = listBox1.SelectedIndex + 1;
                p.Game_Id = g;

                PersonTable.Update(p);
                this.Close();
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Do you really want to add this player?", "", MessageBoxButtons.YesNo))
            {
                Person p = new Person();
                var tmp = PersonTable.Select();
                try
                {
                    p.Id = tmp[tmp.Count - 1].Id + 1;
                    p.First_Name = textBox1.Text;
                    p.Last_Name = textBox5.Text;
                    p.Birth_Date = Int32.Parse(textBox4.Text);
                    p.Role = textBox2.Text;
                }
                catch(Exception)
                {
                    MessageBox.Show("WRONG FORMAT OF INPUT!", "", MessageBoxButtons.OK);
                    return;
                }
            

                Team t = new Team();
                t.Id = listBox2.SelectedIndex + 1;
                if (t.Id == 0)
                {
                    MessageBox.Show("TEAM NOT CHOSEN!", "", MessageBoxButtons.OK);
                    return;
                }
                p.Team_Id = t;

                Game g = new Game();
                g.Id = listBox1.SelectedIndex + 1;
                if (g.Id == 0)
                {
                    MessageBox.Show("GAME NOT CHOSEN!", "", MessageBoxButtons.OK);
                    return;
                }
                p.Game_Id = g;

                PersonTable.Insert(p);
                this.Close();
            }
        }
    }
}
