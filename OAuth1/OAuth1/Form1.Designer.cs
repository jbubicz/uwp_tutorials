namespace OAuth1
{
    partial class frmSignIn
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
            this.WebFacebook = new System.Windows.Forms.WebBrowser();
            this.btnSignIn = new System.Windows.Forms.Button();
            this.lstFriendList = new System.Windows.Forms.ListBox();
            this.btnGetList = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // WebFacebook
            // 
            this.WebFacebook.Location = new System.Drawing.Point(0, 0);
            this.WebFacebook.MinimumSize = new System.Drawing.Size(20, 20);
            this.WebFacebook.Name = "WebFacebook";
            this.WebFacebook.Size = new System.Drawing.Size(360, 363);
            this.WebFacebook.TabIndex = 0;
            this.WebFacebook.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.WebFacebook_DocumentCompleted);
            // 
            // btnSignIn
            // 
            this.btnSignIn.Location = new System.Drawing.Point(110, 369);
            this.btnSignIn.Name = "btnSignIn";
            this.btnSignIn.Size = new System.Drawing.Size(75, 23);
            this.btnSignIn.TabIndex = 1;
            this.btnSignIn.Text = "Sign In";
            this.btnSignIn.UseVisualStyleBackColor = true;
            this.btnSignIn.Click += new System.EventHandler(this.btnSignIn_Click);
            // 
            // lstFriendList
            // 
            this.lstFriendList.FormattingEnabled = true;
            this.lstFriendList.Location = new System.Drawing.Point(366, 26);
            this.lstFriendList.Name = "lstFriendList";
            this.lstFriendList.Size = new System.Drawing.Size(220, 303);
            this.lstFriendList.TabIndex = 2;
            // 
            // btnGetList
            // 
            this.btnGetList.Location = new System.Drawing.Point(448, 369);
            this.btnGetList.Name = "btnGetList";
            this.btnGetList.Size = new System.Drawing.Size(75, 23);
            this.btnGetList.TabIndex = 3;
            this.btnGetList.Text = "Get";
            this.btnGetList.UseVisualStyleBackColor = true;
            this.btnGetList.Click += new System.EventHandler(this.btnGetList_Click);
            // 
            // frmSignIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 482);
            this.Controls.Add(this.btnGetList);
            this.Controls.Add(this.lstFriendList);
            this.Controls.Add(this.btnSignIn);
            this.Controls.Add(this.WebFacebook);
            this.Name = "frmSignIn";
            this.Text = "Sign In";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser WebFacebook;
        private System.Windows.Forms.Button btnSignIn;
        private System.Windows.Forms.ListBox lstFriendList;
        private System.Windows.Forms.Button btnGetList;
    }
}

