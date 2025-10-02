using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace Lab05_01
{
    public partial class Form1 : Form
    {
        private string currentFile = ""; // lưu đường dẫn file hiện tại

        public Form1()
        {
            InitializeComponent();
            InitComboBox();
            SetDefaultFormat();
        }

        private void InitComboBox()
        {
            // Fonts
            foreach (FontFamily font in new InstalledFontCollection().Families)
            {
                cmbFonts.Items.Add(font.Name);
            }
            // Sizes
            int[] sizes = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            foreach (int s in sizes)
                cmbSize.Items.Add(s);
        }

        private void SetDefaultFormat()
        {
            cmbFonts.SelectedItem = "Tahoma";
            cmbSize.SelectedItem = 14;
            richText.Font = new Font("Tahoma", 14);
        }

        private void menuNew_Click(object sender, EventArgs e)
        {
            richText.Clear();
            currentFile = "";
            SetDefaultFormat();
        }

        private void menuOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Rich Text Format|*.rtf|Text File|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(ofd.FileName).ToLower() == ".rtf")
                    richText.LoadFile(ofd.FileName, RichTextBoxStreamType.RichText);
                else
                    richText.LoadFile(ofd.FileName, RichTextBoxStreamType.PlainText);
                currentFile = ofd.FileName;
            }
        }

        private void menuSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFile))
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Rich Text Format|*.rtf";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    richText.SaveFile(sfd.FileName, RichTextBoxStreamType.RichText);
                    currentFile = sfd.FileName;
                }
            }
            else
            {
                richText.SaveFile(currentFile, RichTextBoxStreamType.RichText);
                MessageBox.Show("Lưu thành công!", "Thông báo");
            }
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDlg = new FontDialog();
            fontDlg.ShowColor = true;
            fontDlg.ShowApply = true;
            fontDlg.ShowEffects = true;
            fontDlg.ShowHelp = true;

            if (fontDlg.ShowDialog() != DialogResult.Cancel)
            {
                richText.SelectionFont = fontDlg.Font;
                richText.SelectionColor = fontDlg.Color;
            }
        }

        // ===== ToolStrip xử lý Bold, Italic, Underline =====
        private void btnBold_Click(object sender, EventArgs e)
        {
            ChangeFontStyle(FontStyle.Bold);
        }

        private void btnItalic_Click(object sender, EventArgs e)
        {
            ChangeFontStyle(FontStyle.Italic);
        }

        private void btnUnderline_Click(object sender, EventArgs e)
        {
            ChangeFontStyle(FontStyle.Underline);
        }

        private void ChangeFontStyle(FontStyle style)
        {
            if (richText.SelectionFont != null)
            {
                Font currentFont = richText.SelectionFont;
                FontStyle newStyle;

                // Toggle style
                if (currentFont.Style.HasFlag(style))
                    newStyle = currentFont.Style & ~style; // bỏ
                else
                    newStyle = currentFont.Style | style; // thêm

                richText.SelectionFont = new Font(currentFont, newStyle);
            }
        }

        private void cmbFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFonts.SelectedItem != null && richText.SelectionFont != null)
            {
                string fontName = cmbFonts.SelectedItem.ToString();
                float size = richText.SelectionFont.Size;
                richText.SelectionFont = new Font(fontName, size, richText.SelectionFont.Style);
            }
        }

        private void cmbSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSize.SelectedItem != null && richText.SelectionFont != null)
            {
                float size = float.Parse(cmbSize.SelectedItem.ToString());
                string fontName = richText.SelectionFont.FontFamily.Name;
                richText.SelectionFont = new Font(fontName, size, richText.SelectionFont.Style);
            }
        }

        private void richText_TextChanged(object sender, EventArgs e)
        {
            string text = richText.Text.Trim();
            int wordCount = string.IsNullOrEmpty(text) ? 0 : text.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
            lblStatus.Text = "Tổng số từ: " + wordCount;
        }
    }
}

