using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum State { Playing, Won, Lost }
    public State CurrentState { get; private set; } = State.Playing;

    void Awake() => Instance = this;

    public void OnDemolitionComplete()
    {
        if (CurrentState != State.Playing) return;
        CurrentState = State.Won;
        Debug.Log($"Victory! {DemolitionTracker.Instance.DemolitionPercent:P0} demolished.");
    }

    public void OnDucksExhausted()
    {
        if (CurrentState != State.Playing) return;
        CurrentState = State.Lost;
        Debug.Log($"Out of ducks. {DemolitionTracker.Instance.DemolitionPercent:P0} demolished.");
    }
}
