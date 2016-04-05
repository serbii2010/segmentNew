using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SegmentNew2.Criterion;
using SegmentNew2.Threshold;

namespace SegmentNew2.Model
{
    class BinThreshold : AThreshold
    {
        public BinThreshold(double left, double right) : base(left, right)
        {
            this.currentValue = (rightBound + leftBound) / 2.0;
            id = AThreshold.THRESHOLD_BIN;
        }

        public override void next(Chain chain, ACriterion criterion)
        {

            if (rightBound - leftBound > precision)
            {
                double criterionDistortion = criterion.Distortion(chain);

                if (criterionDistortion < 0)
                {
                    leftBound = currentValue;
                }
                else
                {
                    rightBound = currentValue;
                }

                if (Math.Abs(criterion.bestDistortion) == Math.Abs(criterionDistortion))
                {
                    this.bestP = currentValue;
                }

                currentValue = (rightBound + leftBound) / 2.0;

            }
        }
    }
}
