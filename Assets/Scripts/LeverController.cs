using System.Collections;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    public SlotMachineController slotMachine;

    public SpriteRenderer leverUp;
    public SpriteRenderer leverDown;

    void Start()
    {
        leverUp.enabled = true;
        leverDown.enabled = false;
    }

    public void PullLever()
    {
        StartCoroutine(PullRoutine());
    }

    IEnumerator PullRoutine()
    {
        // Show lever down
        leverUp.enabled = false;
        leverDown.enabled = true;

        yield return new WaitForSeconds(.2f);

        // Show lever up
        leverDown.enabled = false;
        leverUp.enabled = true;

        // Start spinning
        slotMachine.Spin();
    }
}