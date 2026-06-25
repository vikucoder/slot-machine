using System.Collections;
using UnityEngine;

public class SlotMachineController : MonoBehaviour
{
    public ReelController reel1;
    public ReelController reel2;
    public ReelController reel3;

    private bool spinning = false;

    public void Spin()
    {
        if (spinning)
            return;

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