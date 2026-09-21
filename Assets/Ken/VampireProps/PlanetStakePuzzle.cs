using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public enum PlanetSymbol { Moon, Bat, Coffin }

// Recognizes three deliberate presses and releases the original stake after its cage clears.
public class PlanetStakePuzzle : MonoBehaviour
{
    [SerializeField] private Transform rightController;
    [SerializeField] private VampireKey silverStake;
    [SerializeField] private Transform cage;
    [SerializeField] private Collider cageBarrier;
    [SerializeField] private TMP_Text statusLabel;
    [SerializeField] private PlanetSymbolButton[] buttons;
    [SerializeField] private float liftDistance = 1.1f;
    [SerializeField, Min(.1f)] private float openingSeconds = 1.2f;
    private Vector3 cageClosedPosition;
    private Rigidbody stakeBody;
    private float openingTime;
    private int lastPressFrame = -1;
    public int Progress { get; private set; }
    public bool IsSolved { get; private set; }
    public bool CanPress => isActiveAndEnabled && Progress < 3;

    private void Awake()
    {
        if (rightController == null || silverStake == null || cage == null || cageBarrier == null || statusLabel == null)
        {
            Debug.LogError("Assign the right controller, stake, cage, barrier and status text on PlanetStakePuzzle.", this);
            enabled = false;
            return;
        }
        cageClosedPosition = cage.localPosition;
        stakeBody = silverStake.GetComponent<Rigidbody>();
        silverStake.enabled = false;
        stakeBody.isKinematic = true;
        ShowStatus("FIND THE NIGHT'S ORDER  0 / 3", Color.white);
    }

    public bool IsRightHand(IXRInteractor interactor)
        => rightController != null && interactor != null && interactor.transform.IsChildOf(rightController);

    public void Press(PlanetSymbol symbol, IXRSelectInteractor interactor)
    {
        if (!CanPress || !IsRightHand(interactor) || lastPressFrame == Time.frameCount) return;
        // Moving a held grip from one button to another must not enter extra symbols.
        if (!(interactor is XRBaseInputInteractor input) || !input.selectInput.ReadWasPerformedThisFrame()) return;
        lastPressFrame = Time.frameCount;
        if ((int)symbol != Progress)
        {
            Progress = 0;
            foreach (var button in buttons) button.SetAccepted(false);
            ShowStatus("WRONG ORDER - START AGAIN  0 / 3", new Color(1f, .3f, .25f));
            return;
        }
        Progress++;
        foreach (var button in buttons) button.SetAccepted((int)button.Symbol < Progress);
        ShowStatus(Progress == 3 ? "CORRECT - OPENING THE CAGE" : "CORRECT  " + Progress + " / 3", new Color(.3f, 1f, .55f));
    }

    private void Update()
    {
        if (Progress != 3 || IsSolved) return;
        openingTime += Time.deltaTime;
        float t = Mathf.Clamp01(openingTime / openingSeconds);
        cage.localPosition = cageClosedPosition + Vector3.up * (liftDistance * Mathf.SmoothStep(0f, 1f, t));
        if (t < 1f) return;
        cageBarrier.enabled = false;
        stakeBody.isKinematic = false;
        silverStake.enabled = true;
        IsSolved = true;
        ShowStatus("SILVER STAKE RELEASED - TAKE IT", new Color(.3f, 1f, .55f));
    }

    private void ShowStatus(string message, Color color)
    {
        statusLabel.text = message;
        statusLabel.color = color;
    }
}
