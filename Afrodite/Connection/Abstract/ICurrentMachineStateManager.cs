using Afrodite.Abstract;
using Afrodite.Concrete;

namespace Afrodite.Connection.Abstract
{
	public interface ICurrentMachineStateManager<T>
	{
	    IComponentState<T> CurrentState();
	}

    class CurrentMachineStateManager<T> : ICurrentMachineStateManager<T>
    {
        public IComponentState<T> CurrentState()
        {
            return new ComponentState<T>();
        }
    }
}

