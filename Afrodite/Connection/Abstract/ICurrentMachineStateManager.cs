using Afrodite.Abstract;

namespace Afrodite.Connection.Abstract
{
	public interface ICurrentMachineStateManager<T>
	{
		 IComponentState<T> CurrentState {get;}
	}

}

