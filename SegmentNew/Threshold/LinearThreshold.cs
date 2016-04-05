using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SegmentNew2.Threshold;

namespace SegmentNew2.Model
{
    class LinearThreshold : AThreshold
    {
        public LinearThreshold(double left, double right) : base(left, right)
        {
            this.currentValue = left;
            id = AThreshold.THRESHOLD_LINEAR;
        }

        public override void next(Chain chain, Criterion.ACriterion criterion)
        {
            currentValue += 0.01;
        }
    }
}
