using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SegmentNew2.Model;
using SegmentNew2.Threshold;
using System.Collections.Generic;

namespace SegmentNew2.Criterion
{
    class CriterionMinSymmetryMod : ACriterion
    {
        public double oldSymmmetry;
        public double minSymmetry = double.MinValue;


        public CriterionMinSymmetryMod(AThreshold threshold, Chain chain, double epsilon)
            : base(threshold, chain, epsilon)
        {
        }

        public override bool state(Chain chain)
        {
            double sym = getSymmetry(chain);
            oldSymmmetry = minSymmetry;
            if (this.minSymmetry <= sym)
            {
                this.minSymmetry = sym;
                threshold.bestP = threshold.currentValue;
            }
            if (threshold.id == AThreshold.THRESHOLD_LINEAR)
            {
                if (threshold.currentValue >= threshold.rightBound)
                {
                    return false;
                }
            }

            return true;
        }

        public double getSymmetry(Chain chain)
        {
            chain.recalculate();
            double q1 = 0;
            double q2 = 0;
            //int max = 0;
            List<int> listClass = new List<int>();
            foreach (var el in chain.dictionary)
            {
                if (!listClass.Contains(el.Value))
                {
                    listClass.Add(el.Value);
                }
                q1 += factorialLog(el.Value);
            }
            foreach (var i in listClass)
            {
                int m = 0;
                foreach (var el in chain.dictionary)
                {
                    if (el.Value <= i)
                    {
                        m++;
                    }
                }
                q2 += factorialLog(m);
            }
            //for (int i = max; i > 0; i--)
            //{
            //    int m = 0;
            //    foreach (var el in chain.dictionary)
            //    {
            //        if (el.Value <= i)
            //        {
            //            m++;
            //        }
            //    }
            //    q2 += factorialLog(m);
            //}

            //double Q1 = factorialLog(chain.getCount()) / q1;
            //double Q2 = factorialLog(chain.getDictionaryCount()) / q2;

            double Q1 = q1;
            double Q2 = q2;


            double res = Q1 + Q2;
            return res;
        }

        public override double Distortion(Chain chain)
        {
            return Math.Abs(minSymmetry - oldSymmmetry);
        }

        public double factorialLog(int i)
        {
            double buf = 0;
            for (int j = 1; j <= i; j++)
            {
                buf += Math.Log(j, 2);
            }
            return buf;
        }
    }
}
