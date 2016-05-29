using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SegmentNew2.Model;
using SegmentNew2.Criterion;
using SegmentNew2.Threshold;
using System.Windows.Forms;

namespace SegmentNew2
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
			threshold.bestP = criterion.bestP;
            
        }

        public void Segmentate()
        {
            for (int i = window; i >= 2; i--)
            {
                int j = 0;
                while (j+i <= chain.Count)
                {
					double freqPract = (1 - k) * chain.frequncyPractic (j, i);
					double intervalPract = ( chain.intervalPractic(j, i, Link.End));
					double freqCalc = (1 - k) * chain.frequncyCalculate (j, i);
					double intervalCalc =  chain.intervalCalculate (j, i, Link.End);


                    //double pract = freqPract;
                    //double calc = freqCalc;


                    double pract = freqPract + k * intervalPract;
                    double calc = freqCalc + k * intervalCalc;




                    if (intervalCalc *k != 0)
                    {
                        j++;
                        continue;
                    }

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
        public void SegmentateOfP()
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

                    if (std > threshold.bestP)
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
