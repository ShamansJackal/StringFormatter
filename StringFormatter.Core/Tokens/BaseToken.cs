using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringFormatter.Core.Tokens
{
    internal abstract class BaseToken
    {
        public string TokenText;

        public override string ToString() => TokenText;
    }
}
