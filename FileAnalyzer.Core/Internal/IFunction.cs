using System;

namespace FileAnalyzer.Core.Internal
{
	public interface IFunction<A,B>
	{
		B Execute(A param);
	}
	public interface IFunction<A>
	{
		A Execute();
	}
}
