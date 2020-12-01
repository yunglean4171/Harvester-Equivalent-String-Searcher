using System;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace harvester
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        public bool chosen = false;
        public string paczka;
        public string nword = "TriggerServerEvent";
        public static string filetoload = String.Empty;
        private delegate void FoundInfoSyncHandler(FoundInfoEventArgs e);
        private FoundInfoSyncHandler FoundInfo;

        public void Log(string text)
        {
            richTextBox1.Invoke(new MethodInvoker(delegate () { richTextBox1.AppendText(text + "\r\n"); richTextBox1.ScrollToCaret(); }));
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                richTextBox1.Text = "";
                Log("➤ Folder path has been set to: " + dialog.FileName);
                chosen = true;
                paczka = dialog.FileName;
            }
        }

        private void xuiSuperButton1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCfOC-q8Fe69REYjY7tBNItA?view_as=subscriber");
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if(chosen == false)
            {
                richTextBox1.Text = "";
                Log("⚠️ Select folder with dump before extraction!");
            }
            else
            {
                listBox1.Items.Clear();
                resultsList.Items.Clear();

                DirectoryInfo dinfo = new DirectoryInfo(paczka);

                Regex FileRegex = new Regex(@"\*\.\w\w\w");
                FileInfo[] Files;
                foreach (Match match in FileRegex.Matches("*.lua"))
                {
                    Files = dinfo.GetFiles(match.Value);

                    tabControl1.SelectTab(1);


                    foreach (FileInfo file in Files)
                    {

                        listBox1.Items.Add(file.Name);
                        // listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        filetoload = paczka + "\\" + file.Name;
                        richTextBox2.LoadFile(filetoload, RichTextBoxStreamType.PlainText);
                        int indexTotext = richTextBox2.Find(nword);
                        if (indexTotext >= 0)
                        {

                            Log(@"Found an occurance of the string you are looking for in " + file.Name);
                            richTextBox2.SelectionColor = Color.White;
                            richTextBox2.SelectionBackColor = Color.Black;
                        }
                    }
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            filetoload = paczka + @"\" + listBox1.SelectedItem;
            richTextBox2.LoadFile(filetoload, RichTextBoxStreamType.PlainText);
            int indexTotext = richTextBox2.Find(nword);
            if (indexTotext >= 0)
            {
                richTextBox2.SelectionStart = indexTotext;
                int wordlength = paczka.Length;
                richTextBox2.SelectionColor = Color.White;
                richTextBox2.SelectionBackColor = Color.Black;
            }
        }
    }
}
