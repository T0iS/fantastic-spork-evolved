namespace EsportWiki_EIS0011
{
    partial class Form1
    {
        /// <summary>
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód generovaný Návrhářem Windows Form

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.TablePlayers = new System.Windows.Forms.DataGridView();
            this.playersLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.addPlayerButton = new System.Windows.Forms.Button();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.p_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Surname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BirthYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Role = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Game = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Team = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_edit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.col_del = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.TablePlayers)).BeginInit();
            this.SuspendLayout();
            // 
            // TablePlayers
            // 
            this.TablePlayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TablePlayers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.p_name,
            this.Surname,
            this.BirthYear,
            this.Role,
            this.Game,
            this.Team,
            this.col_edit,
            this.col_del});
            this.TablePlayers.Location = new System.Drawing.Point(82, 77);
            this.TablePlayers.Name = "TablePlayers";
            this.TablePlayers.Size = new System.Drawing.Size(598, 231);
            this.TablePlayers.TabIndex = 0;
            this.TablePlayers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.TablePlayers_CellClick);
            // 
            // playersLabel
            // 
            this.playersLabel.AutoSize = true;
            this.playersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.playersLabel.Location = new System.Drawing.Point(78, 44);
            this.playersLabel.Name = "playersLabel";
            this.playersLabel.Size = new System.Drawing.Size(86, 20);
            this.playersLabel.TabIndex = 1;
            this.playersLabel.Text = "Player list";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(182, 45);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 20);
            this.button1.TabIndex = 2;
            this.button1.Text = "Refresh";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // addPlayerButton
            // 
            this.addPlayerButton.BackColor = System.Drawing.Color.DarkGray;
            this.addPlayerButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.addPlayerButton.FlatAppearance.BorderSize = 3;
            this.addPlayerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addPlayerButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.addPlayerButton.Location = new System.Drawing.Point(101, 314);
            this.addPlayerButton.Name = "addPlayerButton";
            this.addPlayerButton.Size = new System.Drawing.Size(135, 33);
            this.addPlayerButton.TabIndex = 3;
            this.addPlayerButton.Text = "Add player";
            this.addPlayerButton.UseVisualStyleBackColor = false;
            this.addPlayerButton.Click += new System.EventHandler(this.addPlayerButton_Click);
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.Width = 30;
            // 
            // p_name
            // 
            this.p_name.HeaderText = "Name";
            this.p_name.Name = "p_name";
            this.p_name.Width = 60;
            // 
            // Surname
            // 
            this.Surname.HeaderText = "Surname";
            this.Surname.Name = "Surname";
            this.Surname.Width = 60;
            // 
            // BirthYear
            // 
            this.BirthYear.HeaderText = "BirthYear";
            this.BirthYear.Name = "BirthYear";
            this.BirthYear.Width = 45;
            // 
            // Role
            // 
            this.Role.HeaderText = "Role";
            this.Role.Name = "Role";
            this.Role.Width = 80;
            // 
            // Game
            // 
            this.Game.HeaderText = "Game";
            this.Game.Name = "Game";
            this.Game.Width = 85;
            // 
            // Team
            // 
            this.Team.HeaderText = "Team";
            this.Team.Name = "Team";
            this.Team.Width = 90;
            // 
            // col_edit
            // 
            this.col_edit.HeaderText = "Edit";
            this.col_edit.Name = "col_edit";
            this.col_edit.Text = "Edit";
            this.col_edit.UseColumnTextForButtonValue = true;
            this.col_edit.Width = 40;
            // 
            // col_del
            // 
            this.col_del.HeaderText = "Delete";
            this.col_del.Name = "col_del";
            this.col_del.Text = "delete";
            this.col_del.UseColumnTextForButtonValue = true;
            this.col_del.Width = 47;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.addPlayerButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.playersLabel);
            this.Controls.Add(this.TablePlayers);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.TablePlayers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView TablePlayers;
        private System.Windows.Forms.Label playersLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewButtonColumn tab_edit;
        private System.Windows.Forms.Button addPlayerButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn p_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Surname;
        private System.Windows.Forms.DataGridViewTextBoxColumn BirthYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn Role;
        private System.Windows.Forms.DataGridViewTextBoxColumn Game;
        private System.Windows.Forms.DataGridViewTextBoxColumn Team;
        private System.Windows.Forms.DataGridViewButtonColumn col_edit;
        private System.Windows.Forms.DataGridViewButtonColumn col_del;
    }
}

