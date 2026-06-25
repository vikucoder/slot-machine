using UnityEngine;

public class LeverController : MonoBehaviour
{
    public SlotMachineController slotMachine;

    private void OnMouseDown()
    {
        slotMachine.Spin();
    }
}