using TMPro;
using UnityEngine;

public class BetManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject betPanel;
    public TMP_Text currentBetText;

    public LeverController leverController;
    public PlayPopupManager playPopupManager;

    [Header("Bet Settings")]
    public int currentBet = 0;   // No bet selected initially

    void Start()
    {
        betPanel.SetActive(false);
        UpdateBetUI();
    }

    public void OpenBetMenu()
    {
        betPanel.SetActive(true);
    }

    public void CloseBetMenu()
    {
        betPanel.SetActive(false);
    }

    public void SelectBet10()
    {
        currentBet = 10;
        UpdateBetUI();
        CloseBetMenu();
        leverController.PullLever();
    }

    public void SelectBet50()
    {
        currentBet = 50;
        UpdateBetUI();
        CloseBetMenu();
        leverController.PullLever();
    }

    public void SelectBet100()
    {
        currentBet = 100;
        UpdateBetUI();
        CloseBetMenu();
        leverController.PullLever();
    }

    public void ExitGame()
    {
        CloseBetMenu();
        playPopupManager.ReturnToMenu();
    }

    // NEW: Clears the current bet after a spin or failed spin
    public void ClearBet()
    {
        currentBet = 0;
        UpdateBetUI();
    }

    void UpdateBetUI()
    {
        if (currentBet <= 0)
            currentBetText.text = "Bet : --";
        else
            currentBetText.text = $"Bet : {currentBet}G";
    }
}