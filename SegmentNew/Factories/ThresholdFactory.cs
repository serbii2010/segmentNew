using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SegmentNew2.Model;
using SegmentNew2.Threshold;

namespace SegmentNew2.Factories
{
    class ThresholdFactory
    {
        public static AThreshold getThreshold(int index, double leftBound, double rightBound)
        {
            switch (index)
            {
                case 0: return new BinThreshold(leftBound, rightBound);
                case 1: return new LinearThreshold(leftBound, rightBound);
                default: return new BinThreshold(leftBound, rightBound);
            }            
        }
    }
}
