using Afrodite.Abstract;

namespace Afrodite.Connection.Abstract
{
    public interface IMachineStateCollection<T>
	{
        void Add(IComponentState<T> state);
	}
}

