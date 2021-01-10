using System.Collections.Generic;
using System.Linq;

namespace FileAnalyzer.Core
{
    public class Expression
    {
        public Expression(string expression)
        {
            Words = expression.Split(' ').Select(w => w.Trim());
        }

        public IEnumerable<string> Words { get; }
    }
}
