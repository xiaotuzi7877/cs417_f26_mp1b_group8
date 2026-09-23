using UnityEngine;

public class EscapeCondition : MonoBehaviour
{
    public ClockLock alarmLock;
    public ClockLock wristLock;
    public ClockLock pocketLock;

    void Update()
    {
        if (alarmLock.isSolved && wristLock.isSolved && pocketLock.isSolved)
        {
            OnAllLocksSolved();
        }
    }

    void OnAllLocksSolved()
    {
        Debug.Log("All clock locks solved — player can escape!");
        enabled = false;
    }
}