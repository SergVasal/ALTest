using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AltisTests.TrainingPerformanceStats
{
    public class StaticSet
    {
        private readonly ActivityTarget activityTarget;
        private readonly ExerciseInfo exerciseInfo;
        private readonly ActivityStats activityStats;

        private Coroutine repCoroutine;

        public ActivityStats ActivityStats => activityStats;

        public event Action OnActivityUpdated;

        public event Action OnActivityFinished;

        public StaticSet(ActivityTarget activityTarget, ExerciseInfo exerciseInfo)
        {
            this.activityTarget = activityTarget;
            this.exerciseInfo = exerciseInfo;

            var statsState = new ActivityStatsState();
            activityStats = new ActivityStats(statsState);
        }

        public void Start()
        {
            repCoroutine = StaticSetCoroutine().StartCoroutine();
            Debug.LogError($"StaticSet started! Target seconds: {activityTarget.TargetWorkUnits}");
        }

        public void Stop()
        {
            repCoroutine?.StopCoroutine();
        }

        private IEnumerator StaticSetCoroutine()
        {
            var secondsPassed = 0;
            var caloriesBurned = 0;

            while (secondsPassed < activityTarget.TargetWorkUnits)
            {
                yield return new WaitForSecondsRealtime(1);

                secondsPassed += 1;
                caloriesBurned += exerciseInfo.CaloriesPerWorkUnit;

                UpdateStats(secondsPassed, caloriesBurned);

                OnActivityUpdated?.Invoke();

                Debug.LogError($"Rep secondsPassed: {secondsPassed}, caloriesBurned: {caloriesBurned}");
            }

            repCoroutine?.StopCoroutine();
            Debug.LogError($"Rep finished. averageAccuracy: {activityStats.AverageAccuracy}, caloriesBurned: {caloriesBurned}");

            OnActivityFinished?.Invoke();
        }

        private void UpdateStats(int secondsPassed, int caloriesBurned)
        {
            var completionPercent = Mathf.Clamp((float) secondsPassed / activityTarget.TargetWorkUnits * 100f, 0, 100f);
            var averageAccuracy = Mathf.Clamp(activityStats.AverageAccuracy + Random.Range(-10f, 10f), 0f, 100f);

            activityStats.UpdateStats(
                completionPercent,
                averageAccuracy,
                secondsPassed,
                caloriesBurned);
        }
    }
}