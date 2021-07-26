namespace AltisTests.TrainingPerformanceStats
{
    public class ActivityStats
    {
        private readonly ActivityStatsState activityStatsState;

        public float CompletionPercent => activityStatsState.CompletionPercent;

        public float AverageAccuracy => activityStatsState.AverageAccuracy;

        public int TotalTimeSec => activityStatsState.TotalTimeSec;

        public int TotalCaloriesCount => activityStatsState.TotalCaloriesCount;

        public ActivityStats(ActivityStatsState activityStatsState)
        {
            this.activityStatsState = activityStatsState;
        }

        public void UpdateStats(
            float completionPercent,
            float averageAccuracy,
            int totalTimeSec,
            int totalCaloriesCount)
        {
            activityStatsState.CompletionPercent = completionPercent;
            activityStatsState.AverageAccuracy = averageAccuracy;
            activityStatsState.TotalTimeSec = totalTimeSec;
            activityStatsState.TotalCaloriesCount = totalCaloriesCount;
        }
    }
}