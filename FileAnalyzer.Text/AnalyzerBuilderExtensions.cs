using FileAnalyzer.Core;

namespace FileAnalyzer.Core
{
    public static class AnalyzerBuilderExtensions
    {
        public static AnalyzerBuilder Text(this AnalyzerBuilder builder, string extension)
            => builder.Extension(extension, new TxtFinder());
        public static AnalyzerBuilder Text(this AnalyzerBuilder builder)
            => builder.Extension(".txt", new TxtFinder());
    }
}
