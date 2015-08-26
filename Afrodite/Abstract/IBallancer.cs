using System;
using System.Collections.Generic;
using System.Data;

namespace Afrodite.Abstract
{
	public interface IBallancer<T>
	{
		IRegistrationStatus RegisterComponent(IComponent component);
		IEnumerable<IComponent> GetComponents();
//		IComponentState GetComponentState(IComponent component);
//		event EventHandler<IComponent> ComponentRegistred;
//		event EventHandler<IComponent> ComponentDisconected;
//		event EventHandler<IComponentState> NewStateRecieved;
        IDbConnection DbConnection { get; set; }

        Func<int, T> MasterAction { get; set; }

        Func<T, bool> SlaveAction { get; set; }

        Action MasterFailureAction { get; set; }
	}
}