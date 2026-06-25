using UnityEngine;
using System.Collections;

public class SlotMachineController : MonoBehaviour
{
    public ReelController reel1;
    public ReelController reel2;
    public ReelController reel3;

    public void Spin()
    {
        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        reel1.StartSpin();
        reel2.StartSpin();
        reel3.StartSpin();

        yield return new WaitForSeconds(2f);
        reel1.StopSpin();

        yield return new WaitForSeconds(0.5f);
        reel2.StopSpin();

        yield return new WaitForSeconds(0.5f);
        reel3.StopSpin();
    }
}