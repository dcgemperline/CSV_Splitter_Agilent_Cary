using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSV_Spectra_Splitter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        public List<String> filestoparse;
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter ="CSV Files | *.csv";
            openFileDialog1.Multiselect = true;
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                List<String> filelist = new List<string>();
                foreach(String file in openFileDialog1.FileNames)
                {
                    textBox1.AppendText(file + "\n");
                    filelist.Add(file);
                }
                filestoparse = filelist;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (filestoparse == null)
            {
                MessageBox.Show("Please Select a CSV File to SPlit");
            }
            if (textBox2.Modified != true)
            {
                MessageBox.Show("Please Specify the number of samples to extract from each csv file on a line by line basis.");
            }
            if(filestoparse.Count() == textBox2.Lines.Count())
            {
                SplitCSVs(filestoparse, textBox2.Lines);
            }
            else if (textBox2.Lines.Count() > filestoparse.Count())
            {
                MessageBox.Show("Too many arguments passed for the number of files given");
            }
            //test



        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void SplitCSVs(List<String> parsethesefiles, String[] SampleNumberList)
        {
            int counter = parsethesefiles.Count();
            progressBar1.Minimum = 0;
            progressBar1.Maximum = parsethesefiles.Count();
            for (int i = 0 ; i < counter; i++)
            {
                progressBar1.Value = i;
                SpectralExperiment se = new SpectralExperiment(parsethesefiles.ElementAt<String>(i), Int32.Parse(SampleNumberList.ElementAt<String>(i)));
                se.splitCSV();
            }
            MessageBox.Show("CSVs Should Now be Split");

        }
    }
}
