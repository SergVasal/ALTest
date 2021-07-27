using System;
using System.Threading;
using System.Threading.Tasks;
using AltisTests.TrainingPerformanceStats.Data;

namespace AltisTests.TrainingPerformanceStats.Activities
{
    public abstract class SingleActivity : IActivity
    {
        private readonly CancellationTokenSource activityCancellationTokenSource = new CancellationTokenSource();

        public ActivityStats ActivityStats { get; }

        protected ActivityInfo ActivityInfo { get; }

        public event Action ActivityUpdated;

        public event Action ActivityFinished;

        protected SingleActivity(ActivityInfo activityInfo)
        {
            ActivityInfo = activityInfo;

            var statsState = new ActivityStatsState();
            ActivityStats = new ActivityStats(statsState);
        }

        public async Task StartActivityProcess()
        {
            await ActivityProcess();
        }

        public void Stop()
        {
            activityCancellationTokenSource.Cancel();
        }

        protected virtual async Task ActivityProcess()
        {
        }

        protected void OnActivityUpdated()
        {
            ActivityUpdated?.Invoke();
        }

        protected void OnActivityFinished()
        {
            ActivityFinished?.Invoke();
        }
    }
}