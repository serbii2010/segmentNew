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
            
            runAlgoritm();
        }

        public void runAlgoritm()
        {
            string nameFile;
            //string text = "";
            string text = "dabaccbcabad";

            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                nameFile = fileDialog.FileName;
                text = File.ReadAllText(nameFile).ToLower();
                //string pattern = "[-.?!)(,:" + Regex.Escape("[") + "]";
                //var newText = Regex.Replace(text, "[-.?!)(,:\\[\\]\"«»\r]", "");
            }

            //System.Diagnostics.Stopwatch swatch = new System.Diagnostics.Stopwatch(); // создаем объект 
            //swatch.Start();

            Input input = new Input();
            input.chain = new Chain(text);
            input.window = 5;
            input.k = 0;

			//input.threshold = ThresholdFactory.getThreshold(AThreshold.THRESHOLD_LINEAR, 0.19819000000005743, 0.5);
			input.threshold = ThresholdFactory.getThreshold(AThreshold.THRESHOLD_BIN, 0, 0.5);

		
            Algoritm algoritm = new Algoritm(input);

			var criterion = CriterionFactory.GetCriterion(ACriterion.CRITERION_ORLOV, input, 1);

            algoritm.run(criterion);

            algoritm.SegmentateOfP(algoritm.threshold.bestP);

            string result = algoritm.chain.ToString();

            //swatch.Stop();
            //label1.Text = swatch.ElapsedMilliseconds.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            runAlgoritm();
        }

    }

 
}
