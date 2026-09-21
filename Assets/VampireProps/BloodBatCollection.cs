using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

// Owns the house scoreboard and the one-time blood-vial reward for this round.
public class BloodBatCollection : MonoBehaviour
{
    [SerializeField] private Transform rightController;
    [SerializeField] private GameObject bloodVial;
    [SerializeField] private TMP_Text countLabel;
    [SerializeField] private TMP_Text rewardLabel;
    [SerializeField, Min(1)] private int requiredBats = 3;
    public int CaughtCount { get; private set; }
    public bool VialReleased { get; private set; }

    private void Awake()
    {
        if (rightController == null || bloodVial == null || countLabel == null || rewardLabel == null)
        {
            Debug.LogError("Assign the right controller, blood vial and house bat scoreboard on BloodBatCollection.", this);
            enabled = false;
            return;
        }
        bloodVial.SetActive(false);
        Refresh();
    }

    public bool IsRightHand(IXRInteractor interactor)
    {
        return isActiveAndEnabled && interactor != null && rightController != null &&
            interactor.transform.IsChildOf(rightController);
    }

    internal void RecordCatch()
    {
        CaughtCount++;
        if (!VialReleased && CaughtCount >= requiredBats)
        {
            VialReleased = true;
            bloodVial.SetActive(true);
        }
        Refresh();
    }

    private void Refresh()
    {
        countLabel.text = "BAT CAUGHT: " + CaughtCount;
        rewardLabel.text = VialReleased ? "BLOOD VIAL READY" : "MORE BLOOD BATS NEEDED\n" + CaughtCount + " / " + requiredBats;
        rewardLabel.color = VialReleased ? new Color(.3f, 1f, .55f) : new Color(1f, .8f, .35f);
    }
}
