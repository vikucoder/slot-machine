using System.Collections;
using UnityEngine;

public class SlotMachineController : MonoBehaviour
{
    public ReelController reel1;
    public ReelController reel2;
    public ReelController reel3;

    public GameManager gameManager;

    private bool spinning = false;

    public void Spin()
{
    if (spinning)
        return;

    if (!gameManager.SpendCredits())
        return;

    gameManager.GenerateSpin();

    reel1.SetTargetSymbol(gameManager.reel1Result);
    reel2.SetTargetSymbol(gameManager.reel2Result);
    reel3.SetTargetSymbol(gameManager.reel3Result);    

    StartCoroutine(SpinRoutine());
}

    IEnumerator SpinRoutine()
    {
        spinning = true;

        reel1.StartSpin();
        reel2.StartSpin();
        reel3.StartSpin();

        yield return new WaitForSeconds(2f);

        reel1.StopSpin();

        yield return new WaitForSeconds(0.3f);

        reel2.StopSpin();

        yield return new WaitForSeconds(0.3f);

        reel3.StopSpin();
        spinning = false;
    }
}