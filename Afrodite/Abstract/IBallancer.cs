using System;
using System.Collections.Generic;
using System.Data;

namespace Afrodite.Abstract
{
	public interface IBallancer<TJob>
	{
        IRegistrationStatus RegisterComponent(IComponent<TJob> component);
        IEnumerable<IComponent<TJob>> GetComponents();
//		IComponentState GetComponentState(IComponent component);
//		event EventHandler<IComponent> ComponentRegistred;
//		event EventHandler<IComponent> ComponentDisconected;
//		event EventHandler<IComponentState> NewStateRecieved;
        IDbConnection DbConnection { get; set; }

        Func<int, TJob> MasterAction { get; set; }

        Func<TJob, bool> StartTaskAction { get; set; }

        Action MasterFailureAction { get; set; }
	    Func<TJob, bool> StopTaskAction { get; set; }
	    Func<TJob, bool> PauseTaskAction { get; set; }
	    Func<TJob, bool> ResumeTaskAction { get; set; }
	}
}