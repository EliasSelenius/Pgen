using System;
using System.Collections.Generic;
using System.Text;

namespace Pgen {
    public interface IRule {
        bool ParseMatch(TokenReader tr);
    }
}
