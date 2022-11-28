using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyWordPad
{
    public partial class Form1 : Form
    {
        OpenFileDialog openFile = new OpenFileDialog();
        SaveFileDialog saveFile = new SaveFileDialog() {
            Filter = "All files |*.*",
            Title = "Save file"
        };
        PrintDialog printFile = new PrintDialog();
        PrintDocument file = new PrintDocument();
        
        public Form1()
        {
            InitializeComponent();
            file.PrintPage += new PrintPageEventHandler(document_PrintPage);
            board.AcceptsTab = true;
        }

        void document_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(board.Text, new Font("Arial", 20, FontStyle.Regular), Brushes.Black, 20, 20);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            board.ResetText();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    board.LoadFile(openFile.FileName, RichTextBoxStreamType.PlainText);
                }
                catch (IOException)
                {
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Path.GetExtension(openFile.FileName) == "")
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                board.SaveFile(openFile.FileName, RichTextBoxStreamType.PlainText);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFile.ShowDialog() == DialogResult.OK )
            {
                board.SaveFile(saveFile.FileName, RichTextBoxStreamType.PlainText);
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFile.FileName != "")
            {
                file.DocumentName = openFile.FileName;
                printFile.Document = file;
                if (printFile.ShowDialog() == DialogResult.OK)
                {
                    file.Print();
                }
            }
            else
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
        }

        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PageSetupDialog pageSetup = new PageSetupDialog();
            pageSetup.PrinterSettings = new PrinterSettings();
            pageSetup.PageSettings = new PageSettings();
            pageSetup.ShowNetwork = false;
            if(pageSetup.ShowDialog() == DialogResult.OK)
            {
                printFile.PrinterSettings = pageSetup.PrinterSettings;
                file.DefaultPageSettings = pageSetup.PageSettings;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            board.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            board.Redo();
        }
        private int HighlightString(char[] text)
        {
            int returnValue = -1;

            // Ensure that a search string has been specified and a valid start point.
            if (text.Length > 0)
            {
                // Obtain the location of the first character found in the control
                // that matches any of the characters in the char array.
                int indexToText = board.Find(text);
                // Determine whether the text was found in richTextBox1.
                if (indexToText >= 0)
                {
                    // Return the location of the character.
                    returnValue = indexToText;
                }
            }

            return returnValue;
        }
        private void SearchDialog()
        {
            Form findDialog = new Form { Width = 500, Height = 142, Text = "Find" };
            Label textLabel = new Label() { Left = 10, Top = 20, Text = "text to find:", Width = 100 };
            TextBox inputBox = new TextBox() { Left = 150, Top = 20, Width = 300 };
            Button search = new Button() { Text = "Find", Left = 350, Width = 100, Top = 70 };
            search.Click += (object sender, EventArgs e) => HighlightString(inputBox.Text.ToCharArray());

            findDialog.Controls.Add(search);
            findDialog.Controls.Add(textLabel);
            findDialog.Controls.Add(inputBox);
            findDialog.Show();
        }
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchDialog();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            board.SelectAll();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(board.SelectedText);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            board.Paste();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            board.Cut();
        }
        private void insertImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile.Filter = "Images |*.bmp;*.jpg;*.png;*.gif;*.ico";
            openFile.Multiselect = false;
            if(openFile.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(openFile.FileName);
                Clipboard.SetImage(img);
                board.Paste();
                board.Focus();
            }
            else
            {
                board.Focus();
            }
        }

        private void selectFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fonts = new FontDialog();
            if(fonts.ShowDialog() == DialogResult.OK)
            {
                this.board.Font = fonts.Font;
            }
        }

        private void fontColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog color = new ColorDialog();
            if (color.ShowDialog() == DialogResult.OK)
            {
                this.board.SelectionColor = color.Color;
            }
        }

        private void boldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            board.Font = new Font(board.Font, FontStyle.Bold);
        }

        private void italicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            board.Font = new Font(board.Font, FontStyle.Italic);
        }

        private void underToolStripMenuItem_Click(object sender, EventArgs e)
        {
            board.Font = new Font(board.Font, FontStyle.Underline);
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            board.Font = new Font(board.Font, FontStyle.Regular);
        }

        private void pageColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog color = new ColorDialog();
            if (color.ShowDialog() == DialogResult.OK)
            {
                this.board.SelectionBackColor = color.Color;
            }
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            board.SelectionIndent = 0;
        }

        private void ptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            board.SelectionIndent = 5;
        }

        private void ptsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            board.SelectionIndent = 10;
        }

        private void ptsToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            board.SelectionIndent = 15;
        }

        private void ptsToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            board.SelectionIndent = 20;
        }

        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            board.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void centerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            board.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            board.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void addBulletsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            board.SelectionBullet = true;
        }

        private void removeBulletsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            board.SelectionBullet = false;
        }
    }
}
