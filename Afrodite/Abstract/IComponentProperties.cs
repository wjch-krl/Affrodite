﻿using System;

namespace Afrodite
{
	public interface IComponentProperties
	{
		/// <summary>
		/// Gets or sets the cpu info.
		/// </summary>
		/// <value>The cpu info.</value>
		string CpuInfo{ get; set;}
		/// <summary>
		/// Gets or sets the cpu cores count.
		/// </summary>
		/// <value>The cpu cores count.</value>
		int CpuCoresCount{get; set;}
		/// <summary>
		/// Gets or sets the total machine RAM memory in MB.
		/// </summary>
		/// <value>The total machine RAM memory in MB.</value>
		int TotalMemoryMBytes { get; set;}
		/// <summary>
		/// Gets or sets the info about OS.
		/// </summary>
		/// <value>The OS info.</value>
		string OsInfo {get; set;}
		/// <summary>
		/// Gets or sets the total disk capcity in MB .
		/// </summary>
		/// <value>The total disk capcity in MB.</value>
		int TotalDiskMBytes {get;set;}
		/// <summary>
		/// Gets or sets the name of the coputer.
		/// </summary>
		/// <value>The name of the coputer.</value>
		string CoputerName { get; set;}
		/// <summary>
		/// Gets or sets HostName.
		/// </summary>
		/// <value>HostName.</value>
		string HostName { get; set;}
	}
}

