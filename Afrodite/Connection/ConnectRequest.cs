using System;

namespace Afrodite
{
	public class ConnectRequest
	{
		/// <summary>
		/// Gets or sets the hostname.
		/// </summary>
		/// <value>The hostname.</value>
		public string HostName{ get; set;}

		/// <summary>
		/// Gets or sets the name of the machine.
		/// </summary>
		/// <value>The name of the machine.</value>
		public string MachineName { get; set;}

		/// <summary>
		/// Gets or sets the cpu info.
		/// </summary>
		/// <value>The cpu info.</value>
		public string CpuInfo { get; set;}
	}
}

