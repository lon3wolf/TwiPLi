namespace PicTweet
{
    partial class Form1
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
            this.TxtMessage = new System.Windows.Forms.TextBox();
            this.Message = new System.Windows.Forms.CheckBox();
            this.Post = new System.Windows.Forms.Button();
            this.Browse = new System.Windows.Forms.Button();
            this.Picture = new System.Windows.Forms.PictureBox();
            this.TwitPicURL = new System.Windows.Forms.TextBox();
            this.Open = new System.Windows.Forms.Button();
            this.ResultPane = new System.Windows.Forms.GroupBox();
            this.ChangeCredentials = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).BeginInit();
            this.ResultPane.SuspendLayout();
            this.SuspendLayout();
            // 
            // TxtMessage
            // 
            this.TxtMessage.Enabled = false;
            this.TxtMessage.Location = new System.Drawing.Point(12, 284);
            this.TxtMessage.Multiline = true;
            this.TxtMessage.Name = "TxtMessage";
            this.TxtMessage.ReadOnly = true;
            this.TxtMessage.Size = new System.Drawing.Size(260, 52);
            this.TxtMessage.TabIndex = 2;
            // 
            // Message
            // 
            this.Message.AutoSize = true;
            this.Message.Location = new System.Drawing.Point(12, 261);
            this.Message.Name = "Message";
            this.Message.Size = new System.Drawing.Size(107, 17);
            this.Message.TabIndex = 1;
            this.Message.Text = "Include Message";
            this.Message.UseVisualStyleBackColor = true;
            this.Message.CheckedChanged += new System.EventHandler(this.Message_CheckedChanged);
            // 
            // Post
            // 
            this.Post.Location = new System.Drawing.Point(103, 342);
            this.Post.Name = "Post";
            this.Post.Size = new System.Drawing.Size(75, 23);
            this.Post.TabIndex = 3;
            this.Post.Text = "Post";
            this.Post.UseVisualStyleBackColor = true;
            this.Post.Click += new System.EventHandler(this.Post_Click);
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(197, 220);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(75, 23);
            this.Browse.TabIndex = 0;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // Picture
            // 
            this.Picture.Image = global::PicTweet.Properties.Resources.DragnDrop;
            this.Picture.Location = new System.Drawing.Point(13, 12);
            this.Picture.Name = "Picture";
            this.Picture.Size = new System.Drawing.Size(260, 202);
            this.Picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Picture.TabIndex = 0;
            this.Picture.TabStop = false;
            this.Picture.DragOver += new System.Windows.Forms.DragEventHandler(this.Picture_DragOver);
            this.Picture.DragDrop += new System.Windows.Forms.DragEventHandler(this.Picture_DragDrop);
            // 
            // TwitPicURL
            // 
            this.TwitPicURL.Location = new System.Drawing.Point(6, 11);
            this.TwitPicURL.Name = "TwitPicURL";
            this.TwitPicURL.ReadOnly = true;
            this.TwitPicURL.Size = new System.Drawing.Size(219, 20);
            this.TwitPicURL.TabIndex = 0;
            // 
            // Open
            // 
            this.Open.Location = new System.Drawing.Point(230, 9);
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(46, 23);
            this.Open.TabIndex = 1;
            this.Open.Text = "Open";
            this.Open.UseVisualStyleBackColor = true;
            this.Open.Click += new System.EventHandler(this.Open_Click);
            // 
            // ResultPane
            // 
            this.ResultPane.Controls.Add(this.TwitPicURL);
            this.ResultPane.Controls.Add(this.Open);
            this.ResultPane.Location = new System.Drawing.Point(1, 371);
            this.ResultPane.Name = "ResultPane";
            this.ResultPane.Size = new System.Drawing.Size(282, 35);
            this.ResultPane.TabIndex = 8;
            this.ResultPane.TabStop = false;
            this.ResultPane.Visible = false;
            // 
            // ChangeCredentials
            // 
            this.ChangeCredentials.AutoSize = true;
            this.ChangeCredentials.Location = new System.Drawing.Point(174, 265);
            this.ChangeCredentials.Name = "ChangeCredentials";
            this.ChangeCredentials.Size = new System.Drawing.Size(99, 13);
            this.ChangeCredentials.TabIndex = 9;
            this.ChangeCredentials.TabStop = true;
            this.ChangeCredentials.Text = "Change Credentials";
            this.ChangeCredentials.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ChangeCredentials_LinkClicked);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 414);
            this.Controls.Add(this.ChangeCredentials);
            this.Controls.Add(this.ResultPane);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.Post);
            this.Controls.Add(this.Message);
            this.Controls.Add(this.TxtMessage);
            this.Controls.Add(this.Picture);
            this.MaximumSize = new System.Drawing.Size(300, 450);
            this.MinimumSize = new System.Drawing.Size(300, 450);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PicTweet";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Picture_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.Picture_DragOver);
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).EndInit();
            this.ResultPane.ResumeLayout(false);
            this.ResultPane.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Picture;
        private System.Windows.Forms.TextBox TxtMessage;
        private System.Windows.Forms.CheckBox Message;
        private System.Windows.Forms.Button Post;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.TextBox TwitPicURL;
        private System.Windows.Forms.Button Open;
        private System.Windows.Forms.GroupBox ResultPane;
        private System.Windows.Forms.LinkLabel ChangeCredentials;
    }
}

