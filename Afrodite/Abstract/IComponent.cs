namespace Afrodite.Abstract
{
	public interface IComponent<T>
	{
		/// <summary>
		/// Starts the job.
		/// </summary>
		/// <returns>The job.</returns>
		IJob<T> StartJob(T job);
		/// <summary>
		/// Pauses the job.
		/// </summary>
		/// <param name="job">Job.</param>
        void PauseJob(IJob<T> job);
		/// <summary>
		/// Resumes the job.
		/// </summary>
		/// <param name="job">Job.</param>
        void ResumeJob(IJob<T> job);
		/// <summary>
		/// Terminates the job.
		/// </summary>
		/// <returns><c>true</c>, if job was terminated, <c>false</c> otherwise.</returns>
		/// <param name="job">Job.</param>
        bool TerminateJob(IJob<T> job);
		/// <summary>
		/// Gets component info.
		/// </summary>
		IComponentProperties Properties {get;}
		/// <summary>
		/// Gets current machine state.
		/// </summary>
		IComponentState<T> State { get; }
	}
}