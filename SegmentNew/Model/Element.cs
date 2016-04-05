using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegmentNew2.Model
{
    class Element
    {
        public string element;
        public bool isWord = false;

        public Element(string str)
        {
            element = str;
        }

        public Element(string str, bool isW)
        {
            element = str;
            isWord = isW;
        } 
    }
}
