using FileAnalyzer.DataStructures;

namespace FileAnalyzer.Core.Internal
{
	internal static class ReactiveExtensions
	{
		internal static HeapObservable<T> ToHeapObservable<T>(this ConcurrentBinaryMinHeap<T> heap) =>
			new HeapObservable<T>(heap);
	}
}
