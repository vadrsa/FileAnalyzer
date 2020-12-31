using System;
using System.Reactive.Subjects;

namespace FileAnalyzer.Core.Internal
{
	public class ConnectObservableFunction<T> : IFunction<IConnectableObservable<T>, IObservable<T>>
	{
		public IObservable<T> Execute(IConnectableObservable<T> obs)
		{
			obs.Connect();
			return obs;
		}
	}
}
