using UnityEngine;
using TMPro;

/// <summary>
/// Central game state. Handles credits, RNG, win evaluation, and UI.
///
/// Symbol index mapping:
///   0 = 7 (Jackpot)  |  1 = Cherry  |  2 = Bell  |  3 = BAR
/// </summary>
public class GameManager : MonoBehaviour
{
    // ── Inspector ─────────────────────────────────────────────────────────────

    [Header("Economy")]
    public int credits  = 1000;
    public int spinCost = 10;

    [Header("Payouts — 3 of a kind")]
    public int jackpotPayout = 500;
    public int cherryPayout  = 200;
    public int bellPayout    = 100;
    public int barPayout     = 50;

    [Header("UI")]
    public TMP_Text creditsText;
    public TMP_Text resultText;    // Optional — wire up a TMP label for win messages

    // ── RNG Results (read by SlotMachineController) ───────────────────────────

    [HideInInspector] public int reel1Result;
    [HideInInspector] public int reel2Result;
    [HideInInspector] public int reel3Result;

    // ── Unity Callbacks ───────────────────────────────────────────────────────

    void Start()
    {
        UpdateCreditsUI();
        SetResultText(string.Empty);
    }

    // ── Public API ────────────────────────────────────────────────────────────

    /// <summary>Deducts spin cost. Returns false if player cannot afford.</summary>
    public bool SpendCredits()
    {
        if (credits < spinCost)
        {
            Debug.Log("Not enough credits!");
            SetResultText("Not enough credits!");
            return false;
        }

        credits -= spinCost;
        UpdateCreditsUI();
        return true;
    }

    /// <summary>Rolls RNG for all three reels.</summary>
    public void GenerateSpin()
    {
        reel1Result = Random.Range(0, 4);
        reel2Result = Random.Range(0, 4);
        reel3Result = Random.Range(0, 4);

        Debug.Log($"Spin Result: {reel1Result} | {reel2Result} | {reel3Result}");
        SetResultText("Spinning...");
    }

    /// <summary>
    /// Called after all reels stop. sym1/sym2/sym3 come from ReelController.CurrentSymbol —
    /// the actual snapped middle-payline symbol, not a cached assumption.
    /// </summary>
    public void EvaluateResult(int sym1, int sym2, int sym3)
    {
        Debug.Log($"Evaluating: {sym1} | {sym2} | {sym3}");

        if (sym1 == sym2 && sym2 == sym3)
        {
            int    payout = GetPayout(sym1);
            string name   = GetSymbolName(sym1);

            credits += payout;
            UpdateCreditsUI();

            Debug.Log($"WIN! Three {name}s — +{payout} credits");
            SetResultText($"WIN! {name} {name} {name}  +{payout}!");
        }
        else
        {
            Debug.Log("No win.");
            SetResultText("No match. Try again!");
        }
    }

    // ── Private Helpers ───────────────────────────────────────────────────────

    int GetPayout(int symbolIndex)
    {
        switch (symbolIndex)
        {
            case 0:  return jackpotPayout;
            case 1:  return cherryPayout;
            case 2:  return bellPayout;
            case 3:  return barPayout;
            default: return 0;
        }
    }

    string GetSymbolName(int symbolIndex)
    {
        switch (symbolIndex)
        {
            case 0:  return "7";
            case 1:  return "Cherry";
            case 2:  return "Bell";
            case 3:  return "BAR";
            default: return "?";
        }
    }

    void UpdateCreditsUI()
    {
        if (creditsText != null)
            creditsText.text = "Credits : " + credits;
    }

    void SetResultText(string msg)
    {
        if (resultText != null)
            resultText.text = msg;
    }
}
