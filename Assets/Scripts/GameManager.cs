using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //estados y clases
    public enum State { Playing, Won, Lost }
    public State CurrentState { get; private set; } = State.Playing;

   
   private void Awake()
    { 
            Instance = this;
     
    }
    public void OnDemolitionComplete()
    {
        if (CurrentState != State.Playing) return;
        CurrentState = State.Won;
        Debug.Log("VICTORIA");
        Debug.Log(DemolitionTracker.Instance.DemolitionPercent);
    }

    public void OnDucksExhausted()
    {
        if (CurrentState != State.Playing) return;
        CurrentState = State.Lost;
        Debug.Log("HAS PERDIDO");
        Debug.Log(DemolitionTracker.Instance.DemolitionPercent);
    }

    public void NextLevel()
    {

    }
}
