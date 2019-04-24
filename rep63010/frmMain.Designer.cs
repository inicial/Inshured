namespace rep6050
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.dgvTurists = new System.Windows.Forms.DataGridView();
            this.tbDgCode = new System.Windows.Forms.TextBox();
            this.dgvInsured = new System.Windows.Forms.DataGridView();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnrecreate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurists)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInsured)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvTurists
            // 
            this.dgvTurists.AllowUserToAddRows = false;
            this.dgvTurists.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTurists.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTurists.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvTurists.Location = new System.Drawing.Point(12, 38);
            this.dgvTurists.Name = "dgvTurists";
            this.dgvTurists.ReadOnly = true;
            this.dgvTurists.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTurists.Size = new System.Drawing.Size(568, 329);
            this.dgvTurists.TabIndex = 0;
            // 
            // tbDgCode
            // 
            this.tbDgCode.Enabled = false;
            this.tbDgCode.Location = new System.Drawing.Point(12, 12);
            this.tbDgCode.Name = "tbDgCode";
            this.tbDgCode.Size = new System.Drawing.Size(170, 20);
            this.tbDgCode.TabIndex = 2;
            // 
            // dgvInsured
            // 
            this.dgvInsured.AllowUserToAddRows = false;
            this.dgvInsured.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvInsured.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInsured.Location = new System.Drawing.Point(586, 38);
            this.dgvInsured.MultiSelect = false;
            this.dgvInsured.Name = "dgvInsured";
            this.dgvInsured.ReadOnly = true;
            this.dgvInsured.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvInsured.Size = new System.Drawing.Size(265, 329);
            this.dgvInsured.TabIndex = 3;
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(188, 12);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(123, 20);
            this.btnCreate.TabIndex = 4;
            this.btnCreate.Text = "Создать страховку";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(317, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(144, 19);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Аннулировать страховку";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnrecreate
            // 
            this.btnrecreate.Location = new System.Drawing.Point(467, 12);
            this.btnrecreate.Name = "btnrecreate";
            this.btnrecreate.Size = new System.Drawing.Size(176, 19);
            this.btnrecreate.TabIndex = 6;
            this.btnrecreate.Text = "Распечатать страховку";
            this.btnrecreate.UseVisualStyleBackColor = true;
            this.btnrecreate.Click += new System.EventHandler(this.btnrecreate_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 379);
            this.Controls.Add(this.btnrecreate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.dgvInsured);
            this.Controls.Add(this.tbDgCode);
            this.Controls.Add(this.dgvTurists);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "Система выписки страховок";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurists)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInsured)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTurists;
        private System.Windows.Forms.TextBox tbDgCode;
        private System.Windows.Forms.DataGridView dgvInsured;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnrecreate;
    }
}

