using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Parameters
{
    [Serializable]
    public class Army: Parameter
    {
        public Army(int initialValue): base(initialValue)
        {
            lowerBoundary = 0;
            upperBoundary = 100;
        }

        public override string FormText()
        {
            string res = value.ToString() + "%";
            return res;
        }
    }
}
