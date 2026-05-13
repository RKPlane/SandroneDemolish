using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

	DuckLauncher duckLauncher;

	void Start()
	{
		duckLauncher = Object.FindAnyObjectByType<DuckLauncher>();

		GameManager.Instance.OnGameWon += HandleWon;
		GameManager.Instance.OnGameLost += HandleLost;
		GameManager.Instance.OnDemolitionChanged += UpdateHUD;

		wonPanel.SetActive(false);
		lostPanel.SetActive(false);

		retryButton.onClick.AddListener(GameManager.Instance.RestartLevel);
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
	}

	void HandleLost()
	{
		lostPanel.SetActive(true);
		if (lostPercentText != null)
			lostPercentText.text = $"Solo demoliste el {DemolitionTracker.Instance.DemolitionPercent * 100f:F1}%...";
	}
}