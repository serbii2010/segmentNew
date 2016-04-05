using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SegmentNew2.Criterion;
using SegmentNew2.Model;

namespace SegmentNew2.Factories
{
    class CriterionFactory
    {
        public static ACriterion GetCriterion(int index, Input input, double epsilon)
        {
            switch (index)
            {
                case 0: return new CriterionOrlov(input.threshold, input.chain, epsilon);

                case 2: return new CriterionMinSymmetry(input.threshold, input.chain, epsilon); 
                case 3: return new CriterionMinSymmetryNew(input.threshold, input.chain, epsilon); 
                case 4: return new CriterionMinSymmetryMod(input.threshold, input.chain, epsilon); 
                default: return new CriterionOrlov(input.threshold, input.chain, epsilon);
            }
        }
    }
}
