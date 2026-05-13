using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading;
using System.Collections;

public class GameUI : MonoBehaviour
{
	[Header("HUD en partida")]
	[SerializeField] TextMeshProUGUI ducksText;
	[SerializeField] TextMeshProUGUI demolitionPercentText;
	[SerializeField] Slider demolitionBar;

	[Header("Panel de Victoria")]
	[SerializeField] GameObject wonPanel;
	[SerializeField] TextMeshProUGUI wonPercentText;

	[Header("Panel de Derrota")]
	[SerializeField] GameObject lostPanel;
	[SerializeField] TextMeshProUGUI lostPercentText;
	[SerializeField] Button retryButton;

    [SerializeField] public string SceneLoader;

	DuckLauncher duckLauncher;

	void Start()
	{
		duckLauncher = Object.FindAnyObjectByType<DuckLauncher>();

		GameManager.Instance.OnGameWon += HandleWon;
		GameManager.Instance.OnGameLost += HandleLost;
		GameManager.Instance.OnDemolitionChanged += UpdateHUD;

		wonPanel.SetActive(false);
		lostPanel.SetActive(false);

	}

	void OnDestroy()
	{
		if (GameManager.Instance != null)
		{
			GameManager.Instance.OnGameWon -= HandleWon;
			GameManager.Instance.OnGameLost -= HandleLost;
			GameManager.Instance.OnDemolitionChanged -= UpdateHUD;
		}
	}

	void Update()
	{
		if (GameManager.Instance.CurrentState != GameManager.State.Playing) return;

		//DUCK LAUNCHER
		if (ducksText != null && duckLauncher != null)
			ducksText.text = $"Patos restantes: {duckLauncher.duckCount}";
	}

	void UpdateHUD()
	{
		float percent = DemolitionTracker.Instance.DemolitionPercent * 100f;

		if (demolitionPercentText != null)
			demolitionPercentText.text = $"{percent:F0}%";

		if (demolitionBar != null)
			demolitionBar.value = DemolitionTracker.Instance.DemolitionPercent;
	}

    void HandleWon()
    {
        wonPanel.SetActive(true);
        if (wonPercentText != null)
            wonPercentText.text = $"¡Demolido un {DemolitionTracker.Instance.DemolitionPercent * 100f:F1}%!";
        StartCoroutine(LoadSceneAfterDelay(SceneLoader, 3f));
    }

    void HandleLost()
    {
        lostPanel.SetActive(true);
        if (lostPercentText != null)
            lostPercentText.text = $"Solo demoliste el {DemolitionTracker.Instance.DemolitionPercent * 100f:F1}%...";
        StartCoroutine(LoadSceneAfterDelay(SceneManager.GetActiveScene().buildIndex, 3f));
    }

    IEnumerator LoadSceneAfterDelay(string sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator LoadSceneAfterDelay(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }
}