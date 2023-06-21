namespace RemoteMVPAdmin
{
    partial class AdminView
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listViewUser = new ListView();
            columnName = new ColumnHeader();
            columnPassword = new ColumnHeader();
            btnDelete = new Button();
            SuspendLayout();
            // 
            // listViewUser
            // 
            listViewUser.Columns.AddRange(new ColumnHeader[] { columnName, columnPassword });
            listViewUser.FullRowSelect = true;
            listViewUser.Location = new Point(12, 12);
            listViewUser.MultiSelect = false;
            listViewUser.Name = "listViewUser";
            listViewUser.Size = new Size(967, 439);
            listViewUser.TabIndex = 0;
            listViewUser.UseCompatibleStateImageBehavior = false;
            listViewUser.View = View.Details;
            // 
            // columnName
            // 
            columnName.Text = "Name";
            columnName.Width = 300;
            // 
            // columnPassword
            // 
            columnPassword.Text = "Password";
            columnPassword.Width = 300;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(985, 12);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(273, 439);
            btnDelete.TabIndex = 1;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // AdminView
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1270, 463);
            Controls.Add(btnDelete);
            Controls.Add(listViewUser);
            Name = "AdminView";
            Text = "Admin";
            ResumeLayout(false);
        }

        #endregion

        private ListView listViewUser;
        private ColumnHeader columnName;
        private ColumnHeader columnPassword;
        private Button btnDelete;
    }
}