using System;
using System.Collections;
using System.Collections.Generic;

namespace AffroditeIndepend
{
	public class MyClass
	{
		public MyClass ()
		{
		}

		public static bool IsValidForMachine(int taskId,int machinesCount, int currentMachineId)
		{
			int count = machinesCount + 1;
			return taskId % count == currentMachineId % count;
		}

		public static bool IsValidForMachine(int taskId,int machinesCount, int currentMachineId, HashSet<int> unaviableHosts)
		{
			int count = machinesCount + 1;
			bool isThisMachine = taskId % count == currentMachineId % count;
			if (isThisMachine)
			{
				return true;
			}
			foreach (int unreachableHost in unaviableHosts)
			{
				int newId = taskId;
				do
				{
					int tmpId = newId + unreachableHost + 1;
					newId = GetMachineIdForTask (tmpId, machinesCount);
					if (newId == currentMachineId)
					{
						return true;
					} 
				} while (unaviableHosts.Contains (newId));//TODO check if could become endless loop
			}
			return false;
		}

		public static int GetMachineIdForTask (int taskId, int machinesCount)
		{
			int count = machinesCount + 1;
			return taskId % count;
		}
	}
}

