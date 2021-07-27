using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AltisTests.TrainingPerformanceStats.Data;

namespace AltisTests.TrainingPerformanceStats.Activities
{
    public sealed class CompositeActivity<T> : IActivity
        where T : IActivity
    {
        private readonly Queue<T> awaitingActivities = new Queue<T>();
        private readonly List<T> startedActivities = new List<T>();

        private T currentActivity;
        private int totalActivities;

        public ActivityStats ActivityStats { get; }

        public event Action ActivityUpdated;

        public event Action ActivityFinished;

        public CompositeActivity()
        {
            var statsState = new ActivityStatsState();
            ActivityStats = new ActivityStats(statsState);
        }

        public void AddChildActivity(T activity)
        {
            awaitingActivities.Enqueue(activity);
            totalActivities++;
        }

        public async Task StartActivityProcess()
        {
            await ActivityProcess();
        }

        public void Stop()
        {
            currentActivity.Stop();

            currentActivity.ActivityUpdated -= OnActivityUpdated;
            currentActivity.ActivityFinished -= OnActivityFinished;
        }

        private void UpdateStats()
        {
            var completionPercent = startedActivities.Sum(x => x.ActivityStats.CompletionPercent) / totalActivities;
            var averageAccuracy = startedActivities.Sum(x => x.ActivityStats.AverageAccuracy) / startedActivities.Count;
            var totalTimeSec = startedActivities.Sum(x => x.ActivityStats.TotalTimeSec);
            var totalCaloriesCount = startedActivities.Sum(x => x.ActivityStats.TotalCaloriesCount);

            ActivityStats.UpdateStats(
                completionPercent,
                averageAccuracy,
                totalTimeSec,
                totalCaloriesCount);
        }

        private async Task ActivityProcess()
        {
            currentActivity = awaitingActivities.Dequeue();
            currentActivity.ActivityUpdated += OnActivityUpdated;
            currentActivity.ActivityFinished += OnActivityFinished;
            startedActivities.Add(currentActivity);

            await currentActivity.StartActivityProcess();
        }

        private void OnActivityUpdated()
        {
            UpdateStats();

            ActivityUpdated?.Invoke();
        }

        private async void OnActivityFinished()
        {
            currentActivity.ActivityUpdated -= OnActivityUpdated;
            currentActivity.ActivityFinished -= OnActivityFinished;

            if (awaitingActivities.Count > 0)
            {
                await ActivityProcess();
            }
            else
            {
                ActivityFinished?.Invoke();
            }
        }
    }
}