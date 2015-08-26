using System.Collections.Generic;

namespace Afrodite.Abstract
{
	public interface IBallancer
	{
		IRegistrationStatus RegisterComponent(IComponent component);
		IEnumerable<IComponent> GetComponents();
		IComponentState GetComponentState(IComponent component);
//		event EventHandler<IComponent> ComponentRegistred;
//		event EventHandler<IComponent> ComponentDisconected;
//		event EventHandler<IComponentState> NewStateRecieved;

	}
}