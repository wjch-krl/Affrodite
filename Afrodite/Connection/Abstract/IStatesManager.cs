namespace Afrodite.Connection.Abstract
{
	public interface IStatesManager
	{
		IMachineStateCollection this [int key]
		{
			get;
			set;
		}
	}

}

