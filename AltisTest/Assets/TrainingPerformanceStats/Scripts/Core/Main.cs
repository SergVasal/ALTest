using AltisTests.TrainingPerformanceStats.Activities;
using AltisTests.TrainingPerformanceStats.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace AltisTests.TrainingPerformanceStats.Core
{
    public class Main : MonoBehaviour
    {
        private CompositeActivity<IActivity> trainingSessionActivity;

        private async void Start()
        {
            trainingSessionActivity = new CompositeActivity<IActivity>();
            trainingSessionActivity.ActivityFinished += TrainingActivityFinished;
            AddSquatExercise(trainingSessionActivity);

            var trainingActivity = trainingSessionActivity.StartActivityProcess();
        }

        private void OnDisable()
        {
            trainingSessionActivity.ActivityFinished -= TrainingActivityFinished;
        }

        private void AddSquatExercise(CompositeActivity<IActivity> trainingSession)
        {
            var squatExercise = new CompositeActivity<IActivity>();
            trainingSession.AddChildActivity(squatExercise);

            var squatSetsTarget = new ActivityTarget(2);
            for (var set = 0; set < squatSetsTarget.TargetWorkUnits; set++)
            {
                var squatSet = new CompositeActivity<IActivity>();
                squatExercise.AddChildActivity(squatSet);

                var squatRepsTarget = new ActivityTarget(2);
                for (var rep = 0; rep < squatRepsTarget.TargetWorkUnits; rep++)
                {
                    var squatInfo = new ActivityInfo(100);
                    var squatRep = new DynamicRepetition(squatInfo);
                    squatSet.AddChildActivity(squatRep);
                }
            }
        }

        private void TrainingActivityFinished()
        {
            trainingSessionActivity.ActivityFinished -= TrainingActivityFinished;

            Debug.LogError($"trainingSession.ActivityStats.TotalTimeSec: {trainingSessionActivity.ActivityStats.TotalTimeSec}");
            Debug.LogError($"Completion percent: {trainingSessionActivity.ActivityStats.CompletionPercent}");
            Debug.LogError($"Calories burned: {trainingSessionActivity.ActivityStats.TotalCaloriesCount}");
            Debug.LogError($"Average accuracy: {trainingSessionActivity.ActivityStats.AverageAccuracy}");

            Assert.AreEqual(4, trainingSessionActivity.ActivityStats.TotalTimeSec);
            Assert.AreEqual(100f, trainingSessionActivity.ActivityStats.CompletionPercent);
            Assert.AreEqual(400, trainingSessionActivity.ActivityStats.TotalCaloriesCount);
            Assert.AreEqual(50f, trainingSessionActivity.ActivityStats.AverageAccuracy);
        }
    }
}