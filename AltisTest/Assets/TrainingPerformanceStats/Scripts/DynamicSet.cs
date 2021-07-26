using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AltisTests.TrainingPerformanceStats
{
    public class DynamicSet
    {
        private readonly ActivityStats activityStats;

        private readonly Queue<Repetition> awaitingReps = new Queue<Repetition>();
        private readonly List<Repetition> startedReps = new List<Repetition>();

        private Repetition currentRepetition;
        private int totalReps;

        public ActivityStats ActivityStats => activityStats;

        public event Action OnActivityUpdated;

        public event Action OnActivityFinished;

        public DynamicSet()
        {
            var statsState = new ActivityStatsState();
            activityStats = new ActivityStats(statsState);
        }

        public void AddRep(Repetition repetition)
        {
            awaitingReps.Enqueue(repetition);
            totalReps++;
        }

        public void Start()
        {
            StartNextRep();
        }

        public void Stop()
        {
            currentRepetition.Stop();
            currentRepetition.OnActivityUpdated -= ActivityUpdatedHandler;
            currentRepetition.OnActivityFinished -= ActivityFinishedHandler;
        }

        private void StartNextRep()
        {
            currentRepetition = awaitingReps.Dequeue();
            currentRepetition.OnActivityUpdated += ActivityUpdatedHandler;
            currentRepetition.OnActivityFinished += ActivityFinishedHandler;
            startedReps.Add(currentRepetition);
            Debug.LogError($"Next rep starting awaitingReps count: {awaitingReps.Count}, startedReps count: {startedReps.Count}");

            currentRepetition.Start();
        }

        private void ActivityUpdatedHandler()
        {
            UpdateStats();

            OnActivityUpdated?.Invoke();
        }

        private void UpdateStats()
        {
            var completionPercent = startedReps.Sum(x => x.ActivityStats.CompletionPercent) / totalReps;
            var averageAccuracy = startedReps.Sum(x => x.ActivityStats.AverageAccuracy) / startedReps.Count;
            var totalTimeSec = startedReps.Sum(x => x.ActivityStats.TotalTimeSec);
            var totalCaloriesCount = startedReps.Sum(x => x.ActivityStats.TotalCaloriesCount);

            activityStats.UpdateStats(
                completionPercent,
                averageAccuracy,
                totalTimeSec,
                totalCaloriesCount);
        }

        private void ActivityFinishedHandler()
        {
            currentRepetition.OnActivityFinished -= ActivityFinishedHandler;

            if (awaitingReps.Count > 0)
            {
                StartNextRep();
            }
            else
            {
                OnActivityFinished?.Invoke();
            }
        }
    }
}