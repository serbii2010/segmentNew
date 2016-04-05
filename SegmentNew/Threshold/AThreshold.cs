using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SegmentNew2.Model;

namespace SegmentNew2.Threshold
{
    abstract class AThreshold
    {
        public const int THRESHOLD_BIN = 0;
        public const int THRESHOLD_LINEAR = 1;

        public double leftBound;
        public double rightBound;

        public int id;

        public double precision = Math.Pow(10, -5);

        public double currentValue;

        public double bestP;

        public double Distance
        {
            get { return rightBound - leftBound; }
        }

        public AThreshold(double left, double right)
        {
            this.leftBound = left;
            this.rightBound = right;
        }

        abstract public void next(Chain chain, Criterion.ACriterion criterion);
    }
}
