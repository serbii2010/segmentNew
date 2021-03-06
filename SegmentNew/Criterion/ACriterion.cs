﻿using System;
using SegmentNew2.Model;
using SegmentNew2.Threshold;

namespace SegmentNew2.Criterion
{
    abstract class ACriterion
    {
        public const int CRITERION_ORLOV = 0;

        public const int CRITERION_MIN_SYMMETRY = 1;
        public const int CRITERION_MIN_SYMMETRY_MOD = 2;
        public const int CRITERION_MIN_SYMMETRY_NEW = 3;

        public AThreshold threshold;
        public Chain chainOriginal;
        public double epsilon;

        public double dist;
        public double bestDistortion = Double.MaxValue;

		public double bestP;


        public ACriterion(AThreshold threshold, Chain chain, double epsilon )
        {
            this.threshold = threshold;
            this.chainOriginal = chain;
            this.epsilon = epsilon;
        }

        public double Frequency(Chain chain, string word)
        {
            return chain.dictionary[word]/Convert.ToDouble(chain.dictionary[word]);
        }

        public abstract bool state(Chain chain);

        public abstract double Distortion(Chain chain);

    }
}
