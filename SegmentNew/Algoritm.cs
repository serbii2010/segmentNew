using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SegmentNew.Model;
using SegmentNew.Criterion;
using SegmentNew.Threshold;
using System.Windows.Forms;

namespace SegmentNew
{
    class Algoritm
    {
        private readonly Chain originalChain;
        public Chain chain;
        public AThreshold threshold;
        public int window;
        public double k; // коэффициент соотношениия интервальной и частотной характеристик

        public Algoritm(Input input)
        {
            chain = input.chain;
            originalChain = new Chain();

            originalChain = input.chain.Clone();
            threshold = input.threshold;
            window = input.window;
            k = input.k;
        }

        public void run(ACriterion criterion)
        {
            Segmentate();
            while (criterion.state(chain))
            {
                this.threshold.next(chain, criterion);
                this.chain = new Chain();
                this.chain = originalChain.Clone();
                Segmentate();
                
            }
			threshold.bestP = threshold.currentValue;
            
        }

        public void Segmentate()
        {
            for (int i = window; i >= 2; i--)
            {
                int j = 0;
                while (j+i <= chain.Count)
                {
					double freqPract = (1 - k) * (chain.frequncyPractic (j, i));
					double intervalPract = (k * chain.intervalPractic(j, i, Link.End));
					double freqCalc = (1 - k) * chain.frequncyCalculate (j, i);
					double intervalCalc = k * chain.intervalCalculate (j, i, Link.End);
					//double pract = ((1 - k)*(chain.frequncyPractic(j, i)));
					//double calc = ((1 - k) * chain.frequncyCalculate(j, i));

					double pract = freqPract;
					double calc = freqCalc;
                    
					//double pract = ((1 - k)* freqPract) + (k * intervalPract);
					//double calc = ((1 - k) * freqCalc) + (k * intervalCalc);


                    //if (intervalCalc != 0.0 || intervalPract != 0.0) {
                    //    MessageBox.Show ("123");
                    //}

					double std = Math.Abs(pract - calc) / Math.Sqrt(calc);

                    if (std > threshold.currentValue)
                    {
                        chain.cutDown(j, i);
                    }
                    j++;

                }
                
            }

            for (int i = 0; i < chain.Count; i++)
            {
                if (!chain[i].isWord)
                {
                    chain[i].isWord = true;
                }
            }
        }


        /**
         * сегментация для заданного порогового значения
         */
        public void SegmentateOfP(double bestP)
        {
            this.chain = originalChain.Clone();
            for (int i = window; i >= 2; i--)
            {
                int j = 0;
                while (j + i <= chain.Count)
                {
                    double pract = chain.frequncyPractic(j, i);
                    double calc = chain.frequncyCalculate(j, i);

                    //double pract = ((1 - k)*(chain.frequncyPractic(j, i))) + (k * chain.intervalPractic(j, i, Link.End));
                    //double calc = ((1 - k) * chain.frequncyCalculate(j, i)) + (k * chain.intervalCalculate(j, i, Link.End));

                    double std = Math.Abs(pract - calc) / Math.Sqrt(calc);

                    if (std > bestP)
                    {
                        chain.cutDown(j, i);
                    }
                    j++;

                }

            }

            for (int i = 0; i < chain.Count; i++)
            {
                if (!chain[i].isWord)
                {
                    chain[i].isWord = true;
                }
            }
        }
    }
}
