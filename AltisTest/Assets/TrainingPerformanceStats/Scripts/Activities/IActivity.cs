using System;
using System.Threading.Tasks;
using AltisTests.TrainingPerformanceStats.Data;

namespace AltisTests.TrainingPerformanceStats.Activities
{
    public interface IActivity
    {
        ActivityStats ActivityStats { get; }

        event Action ActivityUpdated;

        event Action ActivityFinished;

        Task StartActivityProcess();

        void Stop();
    }
}