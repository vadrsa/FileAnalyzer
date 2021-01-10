using System;

namespace FileAnalyzer.Core.Internal
{
	internal abstract class AbstractFunction<A,B>
	{
		public abstract B Execute(A param);

		public static implicit operator Func<A,B>(AbstractFunction<A,B> b) => b.Execute;
	}
	internal abstract class AbstractFunction<A>
	{
		public abstract A Execute();
		public static implicit operator Func<A>(AbstractFunction<A> b) => b.Execute;
	}
}
