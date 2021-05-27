using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Parameters
{
    [Serializable]
    public class PeopleCount: Parameter
    {
        public PeopleCount(int initialValue) : base(initialValue)
        {
            lowerBoundary = 0;
            upperBoundary = 999999999;
        }

        public override string FormText()
        {
            return FormatNumber(value);
        }

        private static string FormatNumber(int num)
        {
            return string.Format("{0:#,0}", num);
        }
    }
}
