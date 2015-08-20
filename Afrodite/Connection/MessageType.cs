using System;

namespace Afrodite
{
	public enum MessageType
	{
		ConnectionRequest,
		Affirmation,
		StartNewJob,
		StopJob,
		PauseJob,
		ResumeJob,
		Negation,
		Status,
		Error
	}
}

