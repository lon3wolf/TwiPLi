using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwiPLi;
namespace PicTweet
{
    public partial class Form1 : Form
    {
        private string FileName;   
        private bool MessageEnabled;
        public Form1()
        {
            InitializeComponent();

            TxtMessage.Text = global::PicTweet.Properties.Resources.MsgBoxText;
            MessageEnabled = false;
        }

        private void Message_CheckedChanged(object sender, EventArgs e)
        {
            if (Message.Checked)
            {
                TxtMessage.Enabled = true;
                TxtMessage.ReadOnly = false;
                TxtMessage.Text = "";
                MessageEnabled = true;
            }
            else
            {
                TxtMessage.Enabled = false;
                TxtMessage.ReadOnly = true;
                TxtMessage.Text = global::PicTweet.Properties.Resources.MsgBoxText;
                MessageEnabled = false;
            }
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.DereferenceLinks = true;

            OFD.CustomPlaces.Add(new Guid("{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}"));
            //Initial direcory is desktop
            OFD.InitialDirectory = (new FileDialogCustomPlace(new Guid("{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}"))).Path;

            OFD.ReadOnlyChecked = true;
            OFD.Filter = "Joint Picture Experts Group (*.jpg)|*.jpg|"
                       + "Joint Picture Experts Group (*.jpeg)|*.jpeg|"
                       + "Bitmap (*.bmp)|(*.bmp) |"
                       + "Graphical Interchange Format (*.gif)|*.gif|"
                       + "All files (*.*)|*.*";
            OFD.ShowDialog(this);
            if (!String.IsNullOrEmpty(OFD.FileName))
            {
                FileName = OFD.FileName;
                Picture.Image = Image.FromFile(FileName);
            }
        }

        private void Picture_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void Picture_DragDrop(object sender, DragEventArgs e)
        {
            string[] str = (string[])e.Data.GetData(DataFormats.FileDrop);
            FileName = str[0];
            Picture.Image = Image.FromFile(FileName);
        }

        private void Post_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                if ((String.IsNullOrEmpty(Properties.Settings.Default.TwitterID)) || string.IsNullOrEmpty(Properties.Settings.Default.Password))
                {
                    new Credentials().ShowDialog(this);
                }
                if (!String.IsNullOrEmpty(Properties.Settings.Default.TwitterID) || !string.IsNullOrEmpty(Properties.Settings.Default.Password))
                {
                    TwitPicLib twipli = new TwitPicLib(Properties.Settings.Default.TwitterID, Properties.Settings.Default.Password);
                    twipli.UserAgent = "PicTweet; (http://thewiredguy.com/CodeGarage); Powered by TwiPLi ";
                    Response response;
                    if (MessageEnabled)
                    {
                        response = twipli.UploadAndPost(FileName, TxtMessage.Text);
                    }
                    else
                    {
                        response = twipli.Upload(FileName);
                    }
                    if (response.Status) //operartion successful
                    {
                        TwitPicURL.Text = response.MediaUrl;
                        ResultPane.Visible = true;
                        TwitPicURL.SelectAll();
                        MessageBox.Show("Image was successfuly uploaded", "done", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //reset the file name
                        FileName = "";
                    }
                }
            }
            else
            {
                MessageBox.Show("Select a picture first");
            }
        }

        private void Open_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(TwitPicURL.Text));
        }

        private void ChangeCredentials_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Credentials().ShowDialog(this);
        }

    }
}
