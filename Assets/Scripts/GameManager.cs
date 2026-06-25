using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int credits = 1000;
    public int spinCost = 10;

    public int jackpotPayout = 500;
    public int cherryPayout = 200;
    public int bellPayout = 100;
    public int barPayout = 50;

    public TMP_Text creditsText;

    public int reel1Result;
    public int reel2Result;
    public int reel3Result;

    public bool SpendCredits()
{
    if (credits < spinCost)
    {
        Debug.Log("Not enough credits!");
        return false;
    }

    credits -= spinCost;

    UpdateCreditsUI();

    return true;
}
public void GenerateSpin()
{
    reel1Result = Random.Range(0, 4);
    reel2Result = Random.Range(0, 4);
    reel3Result = Random.Range(0, 4);

    Debug.Log($"Spin Result: {reel1Result} | {reel2Result} | {reel3Result}");
}

void Start()
{
    UpdateCreditsUI();
}

void UpdateCreditsUI()
{
    creditsText.text = "Credits : " + credits;
}
}