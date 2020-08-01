
using System;

// ReSharper disable once CheckNamespace
public interface ITimer
{
    /// <summary>
    /// Pauses the game and shows the menu.
    /// </summary>
    void pauseTimer();

    /// <summary>
    /// Resumes the game.
    /// </summary>
    void resumeTimer();

    /// <summary>
    /// Returns Paused state.  
    /// </summary>
    /// <returns>true, if the timer is paused</returns>
    bool isPaused();

    /// <summary>
    /// Returns the remaining time of this game.
    /// </summary>
    /// <returns>remaining time in float</returns>
    float getRemainingTime();

    /// <summary>
    /// Adds Consumer for all time updates.
    /// </summary>
    /// <param name="timeConsumer"></param>
    void addTimeConsumer(Action<float> timeConsumer);

    /// <summary>
    /// Returns the game-over state of the game. 
    /// </summary>
    /// <returns>true, if the game is over</returns>
    bool isTimeOver();

    /// <summary>
    /// Adds time to the counter.
    /// </summary>
    void addTime(float timeIncrement);
}