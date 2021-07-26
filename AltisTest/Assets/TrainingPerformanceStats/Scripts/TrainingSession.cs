using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AltisTests.TrainingPerformanceStats
{
    public class TrainingSession
    {
        private readonly ActivityStats activityStats;

        private readonly Queue<IExercise> awaitingExercises = new Queue<IExercise>();
        private readonly List<IExercise> startedExercises = new List<IExercise>();

        private IExercise currentExercise;
        private int totalExercises;

        public ActivityStats ActivityStats => activityStats;

        public event Action OnActivityUpdated;

        public event Action OnActivityFinished;

        public TrainingSession()
        {
            var statsState = new ActivityStatsState();
            activityStats = new ActivityStats(statsState);
        }

        public void AddExercise(IExercise exercise)
        {
            awaitingExercises.Enqueue(exercise);
            totalExercises++;
        }

        public void Start()
        {
            StartNextExercise();
        }

        public void Stop()
        {
            currentExercise.Stop();

            currentExercise.OnActivityUpdated -= ActivityUpdatedHandler;
            currentExercise.OnActivityFinished -= ActivityFinishedHandler;
        }

        private void StartNextExercise()
        {
            currentExercise = awaitingExercises.Dequeue();
            currentExercise.OnActivityUpdated += ActivityUpdatedHandler;
            currentExercise.OnActivityFinished += ActivityFinishedHandler;
            startedExercises.Add(currentExercise);
            Debug.LogError($"Next exercise starting, awaitingExercises count: {awaitingExercises.Count}, startedExercises count: {startedExercises.Count}");

            currentExercise.Start();
        }

        private void ActivityUpdatedHandler()
        {
            UpdateStats();

            OnActivityUpdated?.Invoke();
        }

        private void UpdateStats()
        {
            var completionPercent = startedExercises.Sum(x => x.ActivityStats.CompletionPercent) / totalExercises;
            var averageAccuracy = startedExercises.Sum(x => x.ActivityStats.AverageAccuracy) / startedExercises.Count;
            var totalTimeSec = startedExercises.Sum(x => x.ActivityStats.TotalTimeSec);
            var totalCaloriesCount = startedExercises.Sum(x => x.ActivityStats.TotalCaloriesCount);

            activityStats.UpdateStats(
                completionPercent,
                averageAccuracy,
                totalTimeSec,
                totalCaloriesCount);
        }

        private void ActivityFinishedHandler()
        {
            currentExercise.OnActivityUpdated -= ActivityUpdatedHandler;
            currentExercise.OnActivityFinished -= ActivityFinishedHandler;

            if (awaitingExercises.Count > 0)
            {
                StartNextExercise();
            }
            else
            {
                OnActivityFinished?.Invoke();
            }
        }
    }
}