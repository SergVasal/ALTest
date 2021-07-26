using UnityEngine;

namespace AltisTests.TrainingPerformanceStats
{
    public class Main : MonoBehaviour
    {
        private TrainingSession trainingSession;

        private void Start()
        {
            trainingSession = new TrainingSession();

            AddSquatExercise(trainingSession);

            trainingSession.Start();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                trainingSession.Stop();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                Debug.LogError($"Completion percent: {trainingSession.ActivityStats.CompletionPercent}, total calories: {trainingSession.ActivityStats.TotalCaloriesCount}");
            }
        }

        private void AddSquatExercise(TrainingSession trainingSession)
        {
            var squatExercise = new DynamicExercise();
            trainingSession.AddExercise(squatExercise);

            for (var set = 0; set < 4; set++)
            {
                var squatSet = new DynamicSet();
                squatExercise.AddSet(squatSet);

                for (var rep = 0; rep < 8; rep++)
                {
                    var squatInfo = new ExerciseInfo(100);
                    var squatRep = new Repetition(squatInfo);
                    squatSet.AddRep(squatRep);
                }
            }
        }
    }
}