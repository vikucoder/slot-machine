using UnityEngine;

public class PlayPopupManager : MonoBehaviour
{
    public GameObject playPopup;
    public GameObject betPanel;
    public GameObject currentBetText;
    public GameObject resultText;
    public GameObject creditsText;

    void Start()
    {
        playPopup.SetActive(true);

        betPanel.SetActive(false);
        currentBetText.SetActive(false);
        resultText.SetActive(false);
        creditsText.SetActive(false);
    }

    public void PlayGame()
    {
        playPopup.SetActive(false);

        betPanel.SetActive(true);
        currentBetText.SetActive(true);
        resultText.SetActive(true);
        creditsText.SetActive(true);
    }

    public void ReturnToMenu()
    {
        playPopup.SetActive(true);

        betPanel.SetActive(false);
        currentBetText.SetActive(false);
        resultText.SetActive(false);
        creditsText.SetActive(false);
    }
}