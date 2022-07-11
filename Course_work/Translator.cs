using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Course_work
{
    public partial class Translator : Form
    {
        string path;
        string[] DB_Path;
        string firstScreen;
        List<int> lexems_int = new List<int>();
        List<string> lexems_string = new List<string>();
        List<int> consts = new List<int>();
        List<string> ids = new List<string>();
        List<string> strings = new List<string>();

        public Translator()
        {
            InitializeComponent();

        }


        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            OpenFileDialog Fd = new OpenFileDialog();
            Fd.Title = "Выберете файл";
            Fd.InitialDirectory = @"C:\";
            Fd.Filter = "текстовые файлы (*.tex *.rtf)|*.txt;*.rtf|Все файлы|*.*";
            if (Fd.ShowDialog() == DialogResult.OK)
            {

                path = Fd.FileName;
                DB_Path = System.IO.File.ReadAllLines(Fd.FileName);
                foreach (string str in DB_Path)
                {
                    richTextBox1.Text += str + "\n";
                }
                foreach (string str in DB_Path)
                {
                    firstScreen += str;
                }
            }    
        }
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = richTextBox2.Text;
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            File.WriteAllText(Path.Combine(docPath, "translated.txt"), text);
            richTextBox3.Text += "\nФайл сохранён в " + docPath + " как \"translated.txt\"\n";
        }
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void обРазработчикеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программу разработал\nстудент группы БВТ201\nМещеряков М.Д.");
        }
        private void TranslateButton_Click(object sender, EventArgs e)
        {
            richTextBox3.Clear();
            LexicalAnalyse lexicalAnalyse = new LexicalAnalyse(richTextBox1.Text);
            lexicalAnalyse.Analyse();
            consts = lexicalAnalyse.GetConsts();
            ids = lexicalAnalyse.GetIds();
            lexems_int = lexicalAnalyse.GetCodeOut();
            strings = lexicalAnalyse.GetStrings();
            if (lexems_int.Count == 0)
            {
                richTextBox3.Text = "Не найдено";
                return;
            }
            //printLexAn(lexems_int, consts, ids,strings);
            SyntaxAnalyse syntaxAnalyse = new SyntaxAnalyse(lexems_int, strings);
            syntaxAnalyse.Analyse();
            richTextBox3.Text+=syntaxAnalyse.GetToOut();
            List<int> forGenerator = syntaxAnalyse.GetCode();
            List<string> newStrings = syntaxAnalyse.GetStrings();
            Generator generator = new Generator(forGenerator, consts, ids, newStrings);
            richTextBox2.Text = generator.GenerateAndGetCodeAsString();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox3.ReadOnly = true;
        }
        private void printLexAn(List<int> codeOut, List<int> consts, List<string> ids, List<string> strings)
        {
            string codeOutStr = "";
            for (int i = 0; i < codeOut.Count; i++)
            {
                codeOutStr += codeOut[i].ToString() + ", ";
                if (codeOut[i] == 5)
                    codeOutStr += "\n";
            }
            string constsStr = "";
            for (int i = 0; i < consts.Count; i++)
            {
                constsStr += consts[i].ToString() + ", ";
            }
            string idsStr = "";
            for (int i = 0; i < ids.Count; i++)
            {
                idsStr += ids[i] + ", ";
            }
            string stringsStr = "";
            for (int i = 0; i < strings.Count; i++)
            {
                stringsStr += strings[i] + ", ";
            }
            richTextBox3.Text = "CODE:\n"+codeOutStr+"\n\n";
            richTextBox3.Text += "CONSTS:\t\t" + constsStr + "\n";
            richTextBox3.Text += "ID:\t\t" + idsStr + "\n";
            richTextBox3.Text += "STRINGS:\t" + stringsStr + "\n";
        }
    }
}