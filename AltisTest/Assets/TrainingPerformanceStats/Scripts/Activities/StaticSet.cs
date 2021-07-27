using System.Threading.Tasks;
using AltisTests.TrainingPerformanceStats.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AltisTests.TrainingPerformanceStats.Activities
{
    public class StaticSet : SingleActivity
    {
        private readonly int setTargetTime;

        public StaticSet(int setTargetTime, ActivityInfo activityInfo) : base(activityInfo)
        {
            this.setTargetTime = setTargetTime;
        }

        protected override async Task ActivityProcess()
        {
            var secondsPassed = 0;
            var caloriesBurned = 0;

            while (secondsPassed < setTargetTime)
            {
                await Task.Delay(1000);

                secondsPassed += 1;
                caloriesBurned += ActivityInfo.CaloriesPerWorkUnit;

                UpdateStats(secondsPassed, caloriesBurned);

                OnActivityUpdated();
            }

            OnActivityFinished();
        }

        private void UpdateStats(int secondsPassed, int caloriesBurned)
        {
            var completionPercent = Mathf.Clamp((float) secondsPassed / setTargetTime * 100f, 0, 100f);
            var averageAccuracy = Mathf.Clamp(ActivityStats.AverageAccuracy + Random.Range(-10f, 10f), 0f, 100f);

            ActivityStats.UpdateStats(
                completionPercent,
                averageAccuracy,
                secondsPassed,
                caloriesBurned);
        }
    }
}