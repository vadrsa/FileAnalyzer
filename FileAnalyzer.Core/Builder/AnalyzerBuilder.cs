using FileAnalyzer.Core.Builder;
using System.Collections.Generic;

namespace FileAnalyzer.Core
{
    public class AnalyzerBuilder : IBuilder<Analyzer>
    {
        private Dictionary<string, IEnumerable<IFinder>> _finderMap;

        public AnalyzerBuilder()
        {
            _finderMap = new Dictionary<string, IEnumerable<IFinder>>();
        }

        public AnalyzerBuilder Extension(string extension, IFinder finder) 
        {
            if (!_finderMap.ContainsKey(extension))
            {
                _finderMap.Add(extension, new List<IFinder>());
            }
            ((List<IFinder>)_finderMap[extension]).Add(finder);
            return this;
        }

        public Analyzer Build()
        {
            return new Analyzer(_finderMap);
        }
    }
}
