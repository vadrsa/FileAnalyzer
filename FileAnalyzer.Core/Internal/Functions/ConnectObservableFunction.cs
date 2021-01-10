using System;
using System.Reactive.Subjects;

namespace FileAnalyzer.Core.Internal
{
	internal class ConnectObservableFunction<T> : AbstractFunction<IConnectableObservable<T>, IObservable<T>>
	{
		public override IObservable<T> Execute(IConnectableObservable<T> obs)
		{
			obs.Connect();
			return obs;
		}
	}
}
