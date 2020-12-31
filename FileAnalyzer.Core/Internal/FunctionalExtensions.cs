using System;
using System.Collections.Generic;
using System.Linq;

namespace FileAnalyzer.Core.Internal
{
	public static class FunctionalExtensions
	{
		public static Func<A,C> Pipe<A, B, C>(this Func<A, B> func1, Func<B, C> func2) => 
			input => func2(func1(input));

		public static Func<A,C> Pipe<A, B, C>(this IFunction<A, B> func1, IFunction<B, C> func2) => 
			input => func2.Execute(func1.Execute(input));

		public static Func<A, C> Pipe<A, B, C>(this Func<A, B> func1, IFunction<B, C> func2) =>
			input => func2.Execute(func1(input));

		public static Func<A, C> Pipe<A, B, C>(this IFunction<A, B> func1, Func<B, C> func2) =>
			input => func2(func1.Execute(input));

		public static Func<A, D> Compose<A, B, C, D>(this Func<A, B> func1, Func<A, C> func2, Func<B,C,D> merge) =>
			input => merge(func1(input), func2(input));

		public static Func<A, D> Compose<A, B, C, D>(this IFunction<A, B> func1, IFunction<A, C> func2, Func<B, C, D> merge) =>
			input => merge(func1.Execute(input), func2.Execute(input));

		public static Func<A, D> Compose<A, B, C, D>(this Func<A, B> func1, IFunction<A, C> func2, Func<B, C, D> merge) =>
			input => merge(func1(input), func2.Execute(input));

		public static Func<A, D> Compose<A, B, C, D>(this IFunction<A, B> func1, Func<A, C> func2, Func<B, C, D> merge) =>
			input => merge(func1.Execute(input), func2(input));


		public static Func<A, D> Select<A, B, C, D, T>(this Func<A, B> func1, Func<T, IFunction<C>> selector, Func<IEnumerable<C>, D> merge)
			where B : IEnumerable<T>
		=>
			input => merge(func1(input).Select(selector).Select(f => f.Execute()));

		public static Func<A, D> Select<A, B, C, D, T>(this IFunction<A, B> func1, Func<T, IFunction<C>> selector, Func<IEnumerable<C>, D> merge)
			where B : IEnumerable<T>
		=>
			input => merge(func1.Execute(input).Select(selector).Select(f => f.Execute()));
	}
}
