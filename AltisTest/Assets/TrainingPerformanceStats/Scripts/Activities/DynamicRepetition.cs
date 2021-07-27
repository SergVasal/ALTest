using System.Threading.Tasks;
using AltisTests.TrainingPerformanceStats.Data;

namespace AltisTests.TrainingPerformanceStats.Activities
{
    public class DynamicRepetition : SingleActivity
    {
        public DynamicRepetition(ActivityInfo activityInfo) : base(activityInfo)
        {
        }

        protected override async Task ActivityProcess()
        {
            var repetitionTimeS = 1;
            await Task.Delay(repetitionTimeS * 1000);

            UpdateStats(repetitionTimeS, ActivityInfo.CaloriesPerWorkUnit);

            OnActivityUpdated();
            OnActivityFinished();
        }

        private void UpdateStats(int secondsPassed, int caloriesBurned)
        {
            var averageAccuracy = 50f;

            ActivityStats.UpdateStats(
                100f,
                averageAccuracy,
                secondsPassed,
                caloriesBurned);
        }
    }
}