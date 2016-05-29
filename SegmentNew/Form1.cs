using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using SegmentNew2.Criterion;
using SegmentNew2.Factories;
using SegmentNew2.Model;
using SegmentNew2.Threshold;

namespace SegmentNew2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            comboCriterion.SelectedIndex = 0;
            comboThreshold.SelectedIndex = 1;
            //runAlgoritm();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            var alg = runAlgoritm();

            alg.SegmentateOfP();

            string text = alg.chain.ToString();

            var dict = alg.chain.getDictionaryToString();

            textBox1.Text = dict + "\r\n\r\n" + text;
        }

        private Algoritm runAlgoritm()
        {
            string nameFile;
            //string text = "";
            string text = "dabaccbcabad";

            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                nameFile = fileDialog.FileName;
                text = File.ReadAllText(nameFile);
                //text = File.ReadAllText("C:\\Users\\александр\\Documents\\Visual Studio 2012\\Projects\\SegmentNew\\blake.txt").ToLower();
                string pattern = "[-.?!)(,:" + Regex.Escape("[") + "]";
                text = Regex.Replace(text, "[-.?!)(,:;\\[\\]\"«»\r]", "");
                text = text.ToLower();

                text = text.Replace(" ", "");
                text = text.Replace("\n\n", "\n");
            }

            //System.Diagnostics.Stopwatch swatch = new System.Diagnostics.Stopwatch(); // создаем объект 
            //swatch.Start();

            Input input = new Input();
            input.chain = new Chain(text);
            input.window = 10;
            input.k = 0;


            //input.threshold = ThresholdFactory.getThreshold(AThreshold.THRESHOLD_LINEAR, 0.19819000000005743, 0.5);
            input.threshold = ThresholdFactory.getThreshold(comboThreshold.SelectedIndex, 0, 0.6);


            Algoritm algoritm = new Algoritm(input);

            var criterion = CriterionFactory.GetCriterion(comboCriterion.SelectedIndex, input, 1);

            algoritm.run(criterion);

            //algoritm.SegmentateOfP();

            return algoritm;

            string result = algoritm.chain.ToString();

            //swatch.Stop();
            //label1.Text = swatch.ElapsedMilliseconds.ToString();
        }

        

    }

 
}
