using Afrodite.Abstract;

namespace Afrodite.Connection.Abstract
{
    public interface IMachineStateCollection<TJob>
	{
        void Add(IComponentState<TJob> state);
        IComponentState<TJob> Last { get; }
	}
}

