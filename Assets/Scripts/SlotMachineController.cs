using System.Collections;
using UnityEngine;

/// <summary>
/// Orchestrates the full spin cycle:
///   Spend credits → Generate RNG → Spin all reels → Stop sequentially → Evaluate result.
/// </summary>
public class SlotMachineController : MonoBehaviour
{
    // ── Inspector ─────────────────────────────────────────────────────────────

    public ReelController reel1;
    public ReelController reel2;
    public ReelController reel3;

    public GameManager gameManager;

    [Tooltip("How long all reels spin before stopping begins.")]
    public float spinDuration = 2f;

    [Tooltip("Delay between each reel stopping sequentially.")]
    public float reelStopInterval = 0.3f;

    // ── State ─────────────────────────────────────────────────────────────────

    private bool isSpinning = false;

    // ── Public API ────────────────────────────────────────────────────────────

    /// <summary>Entry point called by LeverController / Spin button.</summary>
    public void Spin()
    {
        if (isSpinning) return;
        if (!gameManager.SpendCredits()) return;

        // Generate RNG and push results to reels BEFORE spinning starts
        gameManager.GenerateSpin();

        reel1.SetTargetSymbol(gameManager.reel1Result);
        reel2.SetTargetSymbol(gameManager.reel2Result);
        reel3.SetTargetSymbol(gameManager.reel3Result);

        StartCoroutine(SpinRoutine());
    }

    // ── Coroutine ─────────────────────────────────────────────────────────────

    IEnumerator SpinRoutine()
    {
        isSpinning = true;

        reel1.StartSpin();
        reel2.StartSpin();
        reel3.StartSpin();

        yield return new WaitForSeconds(spinDuration);

        // Stop reels one by one with a short dramatic pause between each
        reel1.StopSpin();
        yield return new WaitForSeconds(reelStopInterval);

        reel2.StopSpin();
        yield return new WaitForSeconds(reelStopInterval);

        reel3.StopSpin();

        // Wait one frame to let the final reel's SnapToTarget() complete
        yield return null;

        // Evaluate using CurrentSymbol — which is set by SnapToTarget(), not assumed
        gameManager.EvaluateResult(
            reel1.CurrentSymbol,
            reel2.CurrentSymbol,
            reel3.CurrentSymbol
        );

        isSpinning = false;
    }
}
