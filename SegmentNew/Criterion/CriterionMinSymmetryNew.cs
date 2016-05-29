using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SegmentNew2.Model;
using SegmentNew2.Threshold;

namespace SegmentNew2.Criterion
{
    /**
     * а этот метод основан на глубине, (глубина однородной цепи + разнородной) / кол-во слов в словаре // почему то
     */

    class CriterionMinSymmetryNew : ACriterion
    {
        public double oldSymmmetry;
        public double minSymmetry = double.MinValue;

        public CriterionMinSymmetryNew(AThreshold threshold, Chain chain, double epsilon) : base(threshold, chain, epsilon)
        {
        }

        public override bool state(Chain chain)
        {
            double sym = getSymmetry(chain);
            oldSymmmetry = minSymmetry;
            if (this.minSymmetry <= sym) // ищем максимум
            {
                this.minSymmetry = sym;
                bestP = threshold.currentValue;
                return true;
            }
            if (this.threshold.id == AThreshold.THRESHOLD_LINEAR && this.threshold.currentValue > threshold.rightBound)
            {
                return false;
            }
            return true;
        }

        public double getSymmetry(Chain chain)
        {
            chain.recalculate();
            double q1 = 0; // глубина для однородной цепи 
            double q2 = 0; // глубина для разноводной цепи

            foreach (var word in chain.dictionary)
            {
                int buf1 = 0;
                int buf2 = 0;
                foreach (var el in chain)
                {
                    if (el.element == word.Key)
                    {
                        buf2++;
                        q1 += Math.Log(buf1 + 1, 2);
                        buf1 = 0;
                    }
                    else
                    {
                        buf1++;
                        q2 += Math.Log(buf2 + 1, 2);
                        buf2 = 0;
                    }
                }
            }

            double res = (q1 + q2) / chain.getCount();
            return res;
        }

        public override double Distortion(Chain chain)
        {
            return Math.Abs(minSymmetry - oldSymmmetry);
        }
    }
}
