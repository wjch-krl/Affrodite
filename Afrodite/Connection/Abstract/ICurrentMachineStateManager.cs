using Afrodite.Abstract;

namespace Afrodite.Connection.Abstract
{
	public interface ICurrentMachineStateManager<TJob>
	{
	    IComponentState<TJob> CurrentState();
	}
}

