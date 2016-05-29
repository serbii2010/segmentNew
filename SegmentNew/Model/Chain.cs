using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegmentNew2.Model
{
    class Chain : List<Element>
    {
        public Dictionary<string, int> dictionary = new Dictionary<string, int>();

        public Chain()
        {
        }

        public Chain(string text)
        {
            foreach (var c in text)
            {
                Add(new Element(c.ToString()));
                this.recalculate();
            }
        }
        /**
         * получить блок
         */
        public List<string> getBlock(int start, int length)
        {
            List<string> result = new List<string>();
            for (int i = start; i < start+length; i++)
            {
                if (this.Count > i && !this[i].isWord && this[i].element !="\n")
                {
                    result.Add(this[i].element);
                }
                else
                {
                    return null;
                }
            }
            return result;
        }

        public List<int> findAll(List<string> block)
        {
            List<int> res = new List<int>();
            int index = 0;
            if (block == null)
            {
                return new List<int>();
            }
            while (this.Count >= block.Count + index)
            {
                List<string> nextBlock = this.getBlock(index, block.Count);
                if (nextBlock != null && block.SequenceEqual(nextBlock))
                {
                    res.Add(index);
                    index += block.Count-1;
                }
                index++;
            }
            if (res.Count == 0)
            {
                this.findAll(block);
            }
            return res;
        }

        public double frequncyPractic(int start, int length)//практическая частота
        {
            List<string> block = this.getBlock(start, length);
            if (block == null)
            {
                return 0;
            }
            List<int> positions = findAll(block);
            
            return positions.Count/(double)getCount();
        }

        public double frequncyCalculate(int start, int length)//расчетная частота
        {
            double calc;
            if (length>2)
            {
                calc = (frequncyPractic(start, length - 1) * frequncyPractic(start + 1, length-1)) /
                         frequncyPractic(start + 1, length - 2);
            }
            else
            {
                calc = frequncyPractic(start, 1) * frequncyPractic(start+1, 1);
            }

            
            return calc;
        }

        public double intervalPractic(int start, int length, Link link)//практические интервалы
        {
            double interval = 1;
            List<string> block = this.getBlock(start, length);
            if (block == null)
            {
                return 0;
            }
            List<int> positions = this.findAll(block);
            if (positions.Count == 0)
            {
                return 0;
            }
            if (positions.Count > 1)
            {
                int pred = positions[0];
                for (int i = 1; i < positions.Count; i++)
                {
                    int current = positions[i];
                    interval += Math.Log(current - pred - block.Count + 1, 2);
                    pred = current;
                }
            }
            //todo сделать что то если слова не найдены
            double begin = Math.Log(positions[0] + 1, 2);
            var count = getCount();
            double end = Math.Log(getCount() - positions[positions.Count - 1] - (block.Count - 1) + 1, 2);
            if (end * 0 != 0) {
                var a = 1;
            }
            switch (link)
            {
                case Link.Start:
				return 1.0 / Math.Pow(interval + begin, 1.0 / (double)positions.Count());
                case Link.End:
				return 1.0 / Math.Pow(interval + end, 1.0 / (double)positions.Count());
                case Link.Both:
                    return 0;
                default:
                    return 0;
            }
        }

        public double intervalCalculate(int start, int length, Link link)//расчетная интервалы
        {
            double calc;
            if (length > 2)
            {
                var intPractbegin = intervalPractic(start, length - 1, link);
                var intPraciEnd = intervalPractic(start + 1, length - 1, link);
                var intProctBeginEnd = intervalPractic(start + 1, length - 2, link);
                calc = (intPractbegin + intPraciEnd) - intProctBeginEnd;
                if (calc * 0 != 0)
                {
                    intPraciEnd = intervalPractic(start + 1, length - 1, link);
                    intProctBeginEnd = intervalPractic(start + 1, length - 2, link);
                    return 1;

                }
            }
            else
            {
                calc = intervalPractic(start, 1, link) * intervalPractic(start + 1, 1, link);
            }

            //if(calc == 0){
            //    return 1;
            //}
            
			if(calc.GetType() != typeof(double)){
				return 1;
			}
            return calc;
        }


        /**
         * свернуть цепь
         */
        public void cutDown(int start, int length)
        {
            List<string> sample = this.getBlock(start, length);
            
            for (int i = 0; i < this.Count; i++)
            {
                List<string> block = this.getBlock(i, length);
                if (block != null && sample.SequenceEqual(block))
                {
                    for (int j = i+1; j < i+length; j++)
                    {
                        this[i].element += this[j].element;
                      
                    }
                    this.RemoveRange(i+1, length-1);
                    this[i].isWord = true;
                }    
            }
            recalculate();
        }

        /**
         * пересчет словаря и частот
         */
        public void recalculate()
        {
            this.dictionary = new Dictionary<string, int>();
            foreach (var el in this)
            {
                if (el.element == "\n")
                {
                    continue;
                }
                if (dictionary.ContainsKey(el.element))
                {
                    dictionary[el.element]++;
                }
                else
                {
                    dictionary.Add(el.element, 1);
                }
            }
        }

        public override string ToString()
        {
            string res = "";
            foreach (var el in this)
            {
                if (el.element == "\n")
                {
                    res += "\r" + el.element + "|";
                }
                else
                {
                    res += el.element + "|";
                }
                
            }

            return res;
        }

        public Chain Clone()
        {
            var res = new Chain();
            foreach (var e in this)
            {
                var elem = e.element;
                var isW = e.isWord;
                res.Add(new Element(elem, isW));
            }
            res.recalculate();
            return res;
        }

        public int getCount()
        {
            var count = this.Count;

            foreach (var el in this)
            {
                if (el.element == "\n")
                {
                    //count--;
                }
            }

            return count;
        }

        public int getDictionaryCount()
        {
            var count = this.dictionary.Count;

            foreach (var el in this.dictionary)
            {
                if (el.Key == "\n")
                {
                    count--;
                }
            }

            return count;
        }

        public string getDictionaryToString()
        {
            this.recalculate();
            string result = "";
            foreach (var i in dictionary)
            {
                result += i.Key + " ";
            }
            return result;
        }
    }
}
