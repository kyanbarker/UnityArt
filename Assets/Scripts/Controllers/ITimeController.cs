using UnityEngine;

/// <summary>
/// A controller that uses global or local time for calculations
/// </summary>
public interface ITimeController
{
    /// <summary>
    /// The value of `Time.realtimeSinceStartup` recorded at the time this controller's `Start` method ran
    /// </summary>
    public float StartTimeSeconds { get; }

    /// <summary>
    /// We say that an `ITimeController` uses global time if `TimeSeconds` is simply `realtimeSinceStartup`.<br/>
    /// We say that an `ITimeController` uses local time if `TimeSeconds` is instead
    /// `realtimeSinceStartup - startTimeSeconds` (i.e., the number of seconds this controller has been active).
    /// </summary>
    public bool UseGlobalTime { get; }

    /// <summary>
    /// `Time.realtimeSinceStartup` if `useGlobalTime` is true, else `startTimeSeconds` less.
    /// </summary>
    public float TimeSeconds => Time.realtimeSinceStartup - (UseGlobalTime ? 0 : StartTimeSeconds);
}
