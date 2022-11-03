using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringFormatter.Core.Tokens
{
    internal class EscapeToken : BaseToken
    {
        public override string ToString() => TokenText[0].ToString();
    }
}
