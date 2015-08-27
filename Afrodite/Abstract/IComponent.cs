namespace Afrodite.Abstract
{
	public interface IComponent<TJob>
	{
		/// <summary>
		/// Starts the job.
		/// </summary>
		/// <returns>The job.</returns>
		IJob<TJob> StartJob(TJob job);
		/// <summary>
		/// Pauses the job.
		/// </summary>
		/// <param name="job">Job.</param>
        void PauseJob(IJob<TJob> job);
		/// <summary>
		/// Resumes the job.
		/// </summary>
		/// <param name="job">Job.</param>
        void ResumeJob(IJob<TJob> job);
		/// <summary>
		/// Terminates the job.
		/// </summary>
		/// <returns><c>true</c>, if job was terminated, <c>false</c> otherwise.</returns>
		/// <param name="job">Job.</param>
        bool TerminateJob(IJob<TJob> job);
		/// <summary>
		/// Gets component info.
		/// </summary>
		IComponentProperties Properties {get;}
		/// <summary>
		/// Gets current machine state.
		/// </summary>
		IComponentState<TJob> State { get; }
	}
}