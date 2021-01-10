using System;
using System.Collections.Generic;
using System.Linq;

namespace FileAnalyzer.Core.Internal
{
	internal static class FunctionalExtensions
	{
		public static Func<A,C> Pipe<A, B, C>(this Func<A, B> func1, Func<B, C> func2) => 
			input => func2(func1(input));

		public static Func<A,C> Pipe<A, B, C>(this AbstractFunction<A, B> func1, AbstractFunction<B, C> func2) => 
			input => func2.Execute(func1.Execute(input));

		public static Func<A, C> Pipe<A, B, C>(this Func<A, B> func1, AbstractFunction<B, C> func2) =>
			input => func2.Execute(func1(input));

		public static Func<A, C> Pipe<A, B, C>(this AbstractFunction<A, B> func1, Func<B, C> func2) =>
			input => func2(func1.Execute(input));

		public static Func<A, D> Compose<A, B, C, D>(this Func<A, B> func1, Func<A, C> func2, Func<B,C,D> merge) =>
			input => merge(func1(input), func2(input));

		public static Func<A, D> Compose<A, B, C, D>(this AbstractFunction<A, B> func1, AbstractFunction<A, C> func2, Func<B, C, D> merge) =>
			input => merge(func1.Execute(input), func2.Execute(input));

		public static Func<A, D> Compose<A, B, C, D>(this Func<A, B> func1, AbstractFunction<A, C> func2, Func<B, C, D> merge) =>
			input => merge(func1(input), func2.Execute(input));

		public static Func<A, D> Compose<A, B, C, D>(this AbstractFunction<A, B> func1, Func<A, C> func2, Func<B, C, D> merge) =>
			input => merge(func1.Execute(input), func2(input));


		public static Func<A, D> Select<A, B, C, D, T>(this Func<A, B> func1, Func<T, AbstractFunction<C>> selector, Func<IEnumerable<C>, D> merge)
			where B : IEnumerable<T>
		=>
			input => merge(func1(input).Select(selector).Select(f => f.Execute()));

		public static Func<A, D> Select<A, B, C, D, T>(this AbstractFunction<A, B> func1, Func<T, AbstractFunction<C>> selector, Func<IEnumerable<C>, D> merge)
			where B : IEnumerable<T>
		=>
			input => merge(func1.Execute(input).Select(selector).Select(f => f.Execute()));

		public static Func<A, G> Branch<A, B, C, D, E, F, G>(this Func<A, B> func, Func<B, C> firstSelector, Func<Func<B, C>, Func<B, D>> func1, Func<B, E> secondSelector, Func<Func<B, E>, Func<B, F>> func2, Func<D, F, G> merge)
		=>
			input =>
			{
				var res = func(input);
				return merge(func1(firstSelector)(res), func2(secondSelector)(res));
			};

	}
}
