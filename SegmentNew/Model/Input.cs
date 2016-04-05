using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegmentNew2.Model
{
    class Input
    {
        public Chain chain { get; set; } // цепочка
        public Threshold.AThreshold threshold { get; set; } // порог
        public int window { get; set; } // максимальная длина слова
        public double k { get; set; } // коэффициент соотношениия интервальной и частотной характеристик от 0 до 1
    }
}
