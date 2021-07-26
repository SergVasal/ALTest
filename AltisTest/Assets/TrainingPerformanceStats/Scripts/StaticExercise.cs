using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AltisTests.TrainingPerformanceStats
{
    public class StaticExercise : IExercise
    {
        private readonly ActivityStats activityStats;

        private readonly Queue<StaticSet> awaitingSets = new Queue<StaticSet>();
        private readonly List<StaticSet> startedSets = new List<StaticSet>();

        private StaticSet currentSet;
        private int totalSets;

        public ActivityStats ActivityStats => activityStats;

        public event Action OnActivityUpdated;

        public event Action OnActivityFinished;

        public StaticExercise()
        {
            var statsState = new ActivityStatsState();
            activityStats = new ActivityStats(statsState);
        }

        public void AddSet(StaticSet set)
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
            Debug.LogError($"Next set started awaitingSets count: {awaitingSets.Count}, startedSets count: {startedSets.Count}");

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