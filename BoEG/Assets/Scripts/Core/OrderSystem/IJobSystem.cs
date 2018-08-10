namespace Core.OrderSystem
{
    public interface IJobSystem
    {
        void StopJobs();
        void SetJob(IJob job);
        void AddJob(IJob job);
    }
}