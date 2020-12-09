using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;
using DataLayer.Database.FunctionalityClasses;

namespace WindowsFormsApp
{
    public partial class Form2 : Form
    {
        private string[] role = {"Player", "Coach", "Manager"};

        public Form2(string b, string p_id = "")
        {
            InitializeComponent();

            if(b == "add")
            {
                //add
                Player_add_edit.Visible = true;
                label2.Visible = false;
                button2.Visible = true;
                button1.Visible = false;
                textBox3.Visible = false;
                button3.Visible = false;
                TransferButton.Visible = false;
                comboBoxTransfer.Visible = false;

                foreach (var r in role)
                {
                    comboBoxRole.Items.Add(r);
                }
                comboBoxRole.SelectedIndex = -1;

               
                var games = GameTable.Select();
                foreach (Game g in games)
                {
                    comboBoxGame.Items.Add(g.Name);
                }
                comboBoxGame.SelectedIndex = -1;


                var teams = TeamTable.Select();
                foreach (Team t in teams)
                {
                    comboBoxTeam.Items.Add(t.Name);
                    comboBoxTransfer.Items.Add(t.Name);
                }
                comboBoxTeam.SelectedIndex = -1;
                comboBoxTransfer.SelectedIndex = -1;


                

            }
            if (b == "detail" )
            {
                //detail
                Player_add_edit.Visible = false;
                label2.Visible = true;
                button1.Visible = true;
                button2.Visible = false;

                Person p = new Person();
                p = PersonTable.SelectOne(Int32.Parse(p_id));

                textBox1.Text = p.First_Name;
                textBox5.Text = p.Last_Name;
                textBox4.Text = p.Birth_Date.ToString();
                
                textBox3.Text = p_id.ToString();
                textBox3.Visible = false;
                setReadonly(true);
                label2.Text = "Player detail";
                button2.Visible = false;
                button1.Visible = false;
                TransferButton.Visible = false;
                comboBoxTransfer.Visible = false;
                
                button3.Visible = true;

                int idx = 0;
                
                foreach (var r in role)
                {
                    comboBoxRole.Items.Add(r);
                    if (p.Role == r)
                    {   
                        comboBoxRole.SelectedIndex = idx;
                    }
                    idx++;
                }

                var games = GameTable.Select();
                foreach (Game g in games)
                {
                    comboBoxGame.Items.Add(g.Name);
                }
                comboBoxGame.SelectedIndex = p.Game_Id.Id-1;


                var teams = TeamTable.Select();
                foreach (Team t in teams)
                {
                    comboBoxTeam.Items.Add(t.Name);
                    comboBoxTransfer.Items.Add(t.Name);
                }
                comboBoxTeam.SelectedIndex = p.Team_Id.Id - 1;

            }
            if (b == "edit")
            {
                Player_add_edit.Visible = false;
                label2.Visible = true;
                button1.Visible = true;
                button2.Visible = false;

                Person p = new Person();
                p = PersonTable.SelectOne(Int32.Parse(p_id));

                textBox1.Text = p.First_Name;
                textBox5.Text = p.Last_Name;
                textBox4.Text = p.Birth_Date.ToString();

                textBox3.Text = p_id.ToString();
                textBox3.Visible = false;
                button3.Visible = true;

                setReadonly(false);
                label2.Text = "Edit player";
                button2.Visible = false;
                button1.Visible = true;
                TransferButton.Visible = true;
                comboBoxTransfer.Visible = true;

                int idx = 0;
                foreach (var r in role)
                {
                    comboBoxRole.Items.Add(r);
                    if (p.Role == r)
                    {
                        comboBoxRole.SelectedIndex = idx;
                    }
                    idx++;
                }

                var games = GameTable.Select();
                foreach (Game g in games)
                {
                    comboBoxGame.Items.Add(g.Name);
                }
                comboBoxGame.SelectedIndex = p.Game_Id.Id - 1;


                var teams = TeamTable.Select();
                foreach (Team t in teams)
                {
                    comboBoxTeam.Items.Add(t.Name);
                    comboBoxTransfer.Items.Add(t.Name);
                }
                comboBoxTeam.SelectedIndex = p.Team_Id.Id - 1;
            }
            
        }

        private void TransferButton_Click(object sender, EventArgs e)
        {
            //transfer
            int p_id = Int32.Parse(textBox3.Text);
            if (comboBoxTransfer.SelectedIndex == -1 )
            {
                MessageBox.Show("TEAM FOR TRANSFER MUST BE CHOSEN!\nClick on the team to chose it\n", "", MessageBoxButtons.OK);
                return;
            }
            int p_team = comboBoxTransfer.SelectedIndex+1;

            PersonTable.Prestup(p_id, p_team);
            this.Close();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Do you really want to edit this player?", "", MessageBoxButtons.YesNo))
            {
                //edit player
                Person p = new Person();
                
                try
                {
                    
                    p.Id = Int32.Parse(textBox3.Text);
                    p.First_Name = textBox1.Text;
                    p.Last_Name = textBox5.Text;
                    p.Birth_Date = Int32.Parse(textBox4.Text);
                    if (comboBoxRole.SelectedIndex == -1 )
                    {
                        MessageBox.Show("ROLE MUST BE CHOSEN!\nClick on the role to chose it\n", "", MessageBoxButtons.OK);
                        return;
                    }
                    p.Role = comboBoxRole.Items[comboBoxRole.SelectedIndex].ToString();
                    if (p.First_Name.Length > 30 || p.Last_Name.Length > 30 || p.Birth_Date > 2300 || p.Birth_Date < 1800)
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("WRONG FORMAT OF INPUT!\nBoth name and last name can be max 30 chars long\nBirth year must be realistic\n", "", MessageBoxButtons.OK);
                    return;
                }
                Team t = new Team();
                t.Id = comboBoxTeam.SelectedIndex + 1;
                if (t.Id == 0)
                {
                    MessageBox.Show("TEAM NOT CHOSEN!\nClick on the team name to chose it\n", "", MessageBoxButtons.OK);
                    return;
                }
                p.Team_Id = t;

                Game g = new Game();
                g.Id = comboBoxGame.SelectedIndex + 1;
                if (g.Id == 0)
                {
                    MessageBox.Show("GAME NOT CHOSEN!\nClick on the game name to chose it\n", "", MessageBoxButtons.OK);
                    return;
                }
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
                    p.Id = tmp.Count + 1;
                    p.First_Name = textBox1.Text;
                    p.Last_Name = textBox5.Text;
                    p.Birth_Date = Int32.Parse(textBox4.Text);
                    if (comboBoxRole.SelectedIndex == -1 )
                    {
                        MessageBox.Show("ROLE MUST BE CHOSEN!\nClick on the role to chose it\nThe role name must be in blue in order to be chosen sucessfully\n", "", MessageBoxButtons.OK);
                        return;
                    }
                    p.Role = comboBoxRole.Items[comboBoxRole.SelectedIndex].ToString();
                   

                    if (p.First_Name.Length>30 || p.Last_Name.Length > 30 || p.Birth_Date > 2300 || p.Birth_Date < 1800)
                    {
                        throw new Exception();
                    }
                }
                catch(Exception)
                {
                    MessageBox.Show("WRONG FORMAT OF INPUT!\nBoth name and last name can be max 30 chars long\nBirth year must be realistic\n", "", MessageBoxButtons.OK);
                    return;
                }
            
                
                
                
                if (comboBoxTeam.SelectedIndex == -1)
                {
                    
                    MessageBox.Show("TEAM NOT CHOSEN!\nClick on the team name to chose it\n", "", MessageBoxButtons.OK);
                    return;
                }
                Team t = new Team();
                t.Id = comboBoxTeam.SelectedIndex+1;
                p.Team_Id = t;

                
                
                if (comboBoxGame.SelectedIndex == -1)
                {
                    
                    MessageBox.Show("GAME NOT CHOSEN!\nClick on the game name to chose it\n", "", MessageBoxButtons.OK);
                    return;
                }
                Game g = new Game();
                g.Id = comboBoxGame.SelectedIndex+1;
                p.Game_Id = g; 

                PersonTable.Insert(p);
                this.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3(Int32.Parse(textBox3.Text));
            f.ShowDialog();


        }

        private void setReadonly(bool b)
        {
            textBox1.ReadOnly = b;
            textBox5.ReadOnly = b;
            textBox4.ReadOnly = b;
            textBox3.ReadOnly = b;
            comboBoxRole.Enabled = !b;
            comboBoxTeam.Enabled = !b;
            comboBoxGame.Enabled = !b;

            
        }

        private void setEdit()
        {
            setReadonly(false);
            label2.Text = "Edit player";
            button2.Visible = false;
            button1.Visible = true;
            
        }



        private void comboBoxRole_SelectedIndexChanged(object sender, EventArgs e)
        {

           

        }
    }
}
