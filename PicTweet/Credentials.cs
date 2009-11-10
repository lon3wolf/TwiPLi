using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PicTweet
{
    public partial class Credentials : Form
    {
        public Credentials()
        {
            InitializeComponent();

            Properties.Settings.Default.Reload();
            TwitterID.Text = Properties.Settings.Default.TwitterID;
            Password.Text = Properties.Settings.Default.Password;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TwitterID = TwitterID.Text;
            Properties.Settings.Default.Password = Password.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void Credentials_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (String.IsNullOrEmpty(Properties.Settings.Default.TwitterID) || string.IsNullOrEmpty(Properties.Settings.Default.Password))
            {
                e.Cancel = true;
            }
        }
    }
}
