using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class ClockLock : MonoBehaviour
{
    public Transform clockHands;
    public float tickDuration = 1f;
    public bool isSolved = false;
    public void OnItemPlaced(SelectEnterEventArgs args)
    {
        isSolved = true;
        if (clockHands != null)
        {
            StartCoroutine(EaseHandsToTime());
        }
    }

    private IEnumerator EaseHandsToTime()
    {
        Quaternion startRot = clockHands.localRotation;
        Quaternion targetRot = Quaternion.Euler(0, 0, -90);
        float elapsed = 0f;

        while (elapsed < tickDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / tickDuration;
            clockHands.localRotation = Quaternion.Lerp(startRot, targetRot, t);
            yield return null;
        }
    }
}