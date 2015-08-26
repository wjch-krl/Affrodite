namespace Afrodite.Abstract
{
	public interface IComponent
	{
		/// <summary>
		/// Starts the job.
		/// </summary>
		/// <returns>The job.</returns>
		void StartJob(IJob job);
		/// <summary>
		/// Pauses the job.
		/// </summary>
		/// <param name="job">Job.</param>
	    void PauseJob(IJob job);
		/// <summary>
		/// Resumes the job.
		/// </summary>
		/// <param name="job">Job.</param>
		void ResumeJob(IJob job);
		/// <summary>
		/// Terminates the job.
		/// </summary>
		/// <returns><c>true</c>, if job was terminated, <c>false</c> otherwise.</returns>
		/// <param name="job">Job.</param>
		bool TerminateJob(IJob job);
		/// <summary>
		/// Gets component info.
		/// </summary>
		IComponentProperties Properties {get;}
		/// <summary>
		/// Gets current machine state.
		/// </summary>
		IComponentState State { get; }
	}
}