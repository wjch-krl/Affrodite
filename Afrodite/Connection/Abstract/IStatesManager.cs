namespace Afrodite.Connection.Abstract
{
	public interface IStatesManager<T>
	{
		IMachineStateCollection<T> this [int key]
		{
			get;
			set;
		}
	}

}

