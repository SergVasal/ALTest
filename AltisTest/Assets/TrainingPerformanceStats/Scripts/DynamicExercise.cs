using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AltisTests.TrainingPerformanceStats
{
    public class DynamicExercise : IExercise
    {
        private readonly ActivityStats activityStats;

        private Queue<DynamicSet> awaitingSets = new Queue<DynamicSet>();
        private List<DynamicSet> startedSets = new List<DynamicSet>();

        private DynamicSet currentSet;
        private int totalSets;

        public ActivityStats ActivityStats => activityStats;

        public event Action OnActivityUpdated;

        public event Action OnActivityFinished;

        public DynamicExercise()
        {
            var statsState = new ActivityStatsState();
            activityStats = new ActivityStats(statsState);
        }

        public void AddSet(DynamicSet set)
        {
            awaitingSets.Enqueue(set);
            totalSets++;
        }

        public void Start()
        {
            StartNextSet();
        }

        public void Stop()
        {
            currentSet.Stop();

            currentSet.OnActivityUpdated -= ActivityUpdatedHandler;
            currentSet.OnActivityFinished -= ActivityFinishedHandler;
        }

        private void StartNextSet()
        {
            currentSet = awaitingSets.Dequeue();
            currentSet.OnActivityUpdated += ActivityUpdatedHandler;
            currentSet.OnActivityFinished += ActivityFinishedHandler;
            startedSets.Add(currentSet);
            Debug.LogError($"Next set is starting awaitingSets count: {awaitingSets.Count}, startedSets count: {startedSets.Count}");

            currentSet.Start();
        }

        private void ActivityUpdatedHandler()
        {
            UpdateStats();

            OnActivityUpdated?.Invoke();
        }

        private void UpdateStats()
        {
            var completionPercent = startedSets.Sum(x => x.ActivityStats.CompletionPercent) / totalSets;
            var averageAccuracy = startedSets.Sum(x => x.ActivityStats.AverageAccuracy) / startedSets.Count;
            var totalTimeSec = startedSets.Sum(x => x.ActivityStats.TotalTimeSec);
            var totalCaloriesCount = startedSets.Sum(x => x.ActivityStats.TotalCaloriesCount);

            activityStats.UpdateStats(
                completionPercent,
                averageAccuracy,
                totalTimeSec,
                totalCaloriesCount);
        }

        private void ActivityFinishedHandler()
        {
            currentSet.OnActivityFinished -= ActivityFinishedHandler;

            if (awaitingSets.Count > 0)
            {
                StartNextSet();
            }
            else
            {
                OnActivityFinished?.Invoke();
            }
        }
    }
}