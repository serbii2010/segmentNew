using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SegmentNew.Model;
using SegmentNew.Threshold;

namespace SegmentNew.Criterion
{
    class CriterionOrlov : ACriterion
    {
        public bool flag = false;

        public CriterionOrlov(AThreshold threshold, Chain chain, double epsilon) : base(threshold, chain, epsilon)
        {
        }

        public override bool state(Chain chain)
        {
            if ((this.threshold.Distance < this.threshold.precision)
                && (Math.Abs(Distortion(chain)) > epsilon))
            {
                if (flag)
                {
                    flag = !flag;
                    if (this.bestDistortion < 0)
                    {
                        this.threshold.leftBound = this.threshold.bestP;
                        this.threshold.rightBound = 0.5;
                        this.threshold.currentValue = 0.5;
                    }
                    else
                    {
                        this.threshold.leftBound = 0;
                        this.threshold.currentValue = 0;
                        this.threshold.rightBound = this.threshold.bestP;
                    }
                }
                else
                {
                    flag = !flag;
                    if (this.bestDistortion > 0)
                    {
                        this.threshold.leftBound = this.threshold.bestP;
                        this.threshold.rightBound = 0.5;
                        this.threshold.currentValue = 0.5;
                    }
                    else
                    {
                        this.threshold.leftBound = 0;
                        this.threshold.currentValue = 0;
                        this.threshold.rightBound = this.threshold.bestP;
                    }
                }
                
                return true;
            }
            return (this.threshold.Distance > this.threshold.precision)
                   && (Math.Abs(Distortion(chain)) > epsilon);
        }

        public override double Distortion(Chain chain)
        {
            this.dist = TheoryVolume(chain) - chain.getDictionaryCount();
            if (Math.Abs(dist) < Math.Abs(bestDistortion))
            {
                bestDistortion = dist;
				bestP = threshold.currentValue;
            }
            return dist;
        }

        public double TheoryVolume(Chain chain)
        {
            double f = 0;
            foreach (var word in chain.dictionary)
            {
                double freq = word.Value/(double)chain.getCount();
                if (freq > f)
                {
                    f = freq;
                }
            }

            double z = chain.Count;
            double k;
            if (f * z != 1)
            {
                k = 1 / Math.Log(f * z);
            }
            else
            {
                k = 1 / Double.MinValue;
            }
             
            double b = (k / f) - 1;
            double v = (k * z) - b;
            return v;
        }

        
    }
}
