using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AltisTests.TrainingPerformanceStats
{
    public class Repetition
    {
        private readonly ExerciseInfo exerciseInfo;
        private readonly ActivityStats activityStats;

        private Coroutine repCoroutine;

        public ActivityStats ActivityStats => activityStats;

        public event Action OnActivityUpdated;

        public event Action OnActivityFinished;

        public Repetition(ExerciseInfo exerciseInfo)
        {
            this.exerciseInfo = exerciseInfo;

            var statsState = new ActivityStatsState();
            activityStats = new ActivityStats(statsState);
        }

        public void Start()
        {
            repCoroutine = RepCoroutine().StartCoroutine();
            Debug.LogError($"Rep started!");
        }

        public void Stop()
        {
            repCoroutine?.StopCoroutine();
        }

        private IEnumerator RepCoroutine()
        {
            var repTotalTimeSec = Random.Range(1, 5);

            yield return new WaitForSecondsRealtime(repTotalTimeSec);

            UpdateStats(repTotalTimeSec, exerciseInfo.CaloriesPerWorkUnit);

            Debug.LogError($"Rep finished. caloriesBurned: {exerciseInfo.CaloriesPerWorkUnit}");
            repCoroutine?.StopCoroutine();

            OnActivityUpdated?.Invoke();
            OnActivityFinished?.Invoke();
        }

        private void UpdateStats(int secondsPassed, int caloriesBurned)
        {
            var averageAccuracy = Random.Range(0f, 100f);

            activityStats.UpdateStats(
                100f,
                averageAccuracy,
                secondsPassed,
                caloriesBurned);
        }
    }
}