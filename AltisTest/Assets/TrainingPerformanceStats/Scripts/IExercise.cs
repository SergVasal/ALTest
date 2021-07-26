using System;

namespace AltisTests.TrainingPerformanceStats
{
    public interface IExercise
    {
        ActivityStats ActivityStats { get; }
        
        event Action OnActivityUpdated;

        event Action OnActivityFinished;

        void Start();
        
        void Stop();
    }
}