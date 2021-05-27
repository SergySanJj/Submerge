using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Parameters
{
    [Serializable]
    public class Parameter
    {
        public int value;
        public int lowerBoundary = 0;
        public int upperBoundary = 100;
        
        public Parameter(int initialValue)
        {
            value = initialValue;
        }

        public int Value()
        {
            return value;
        }

        public float GetNormalizedPercentValue()
        {
            return ((float)(value - lowerBoundary) / (float)(upperBoundary - lowerBoundary));
        }

        public virtual void SetValue(int val)
        {
            value = val;
            ClampValue();
        }

        protected void ClampValue()
        {
            if (value > upperBoundary)
                value = upperBoundary;
            else if (value < lowerBoundary)
                value = lowerBoundary;
        }

        public virtual string FormText()
        {
            string res = value.ToString();

            return res;
        }

        public virtual bool NoResources()
        {
            return value <= lowerBoundary;
        }
    }
}
