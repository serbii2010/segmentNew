using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SegmentNew.Model;
using SegmentNew.Threshold;

namespace SegmentNew.Criterion
{
    class CriterionMinSymmetryNew : ACriterion
    {
        public double oldSymmmetry;
        public double minSymmetry = double.MaxValue;

        public CriterionMinSymmetryNew(AThreshold threshold, Chain chain, double epsilon) : base(threshold, chain, epsilon)
        {
        }

        public override bool state(Chain chain)
        {
            double sym = getSymmetry(chain);
            oldSymmmetry = minSymmetry;
            if (this.minSymmetry >= sym)
            {
                this.minSymmetry = sym;
                return true;
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

            double res = (q1 + q2)/chain.getDictionaryCount();
            return res;
        }

        public override double Distortion(Chain chain)
        {
            return Math.Abs(minSymmetry - oldSymmmetry);
        }
    }
}
