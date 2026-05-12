using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum State { Playing, Won, Lost }
    public State CurrentState { get; private set; } = State.Playing;

    [SerializeField] float defeatDelay = 3f;

    public event System.Action OnGameWon;
    public event System.Action OnGameLost;
    public event System.Action OnDemolitionChanged;

    void Awake() => Instance = this;

    public void OnDemolitionUpdated()
    {
        OnDemolitionChanged?.Invoke();
    }

    public void OnDemolitionComplete()
    {
        if (CurrentState != State.Playing) return;
        CurrentState = State.Won;
        Debug.Log($"VICTORIA {DemolitionTracker.Instance.DemolitionPercent:P0} demolido");
        OnGameWon?.Invoke();
    }

    public void OnDucksExhausted()
    {
        if (CurrentState != State.Playing) return;
        StartCoroutine(DefeatAfterDelay());
    }

    IEnumerator DefeatAfterDelay()
    {
        yield return new WaitForSeconds(defeatDelay);

        if (CurrentState != State.Playing) yield break;

        CurrentState = State.Lost;
        Debug.Log($"HAS PERDIDO {DemolitionTracker.Instance.DemolitionPercent:P0} demolido");
        OnGameLost?.Invoke();
    }

    /*
    public void NextLevel()
    {
        int current = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(current + 1);
    } */

    public void RestartLevel()
    {
        int current = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(current);
    }
}
