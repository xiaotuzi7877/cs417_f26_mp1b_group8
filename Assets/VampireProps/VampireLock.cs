using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

// Matching socket, permanent one-shot unlock, and a visible mechanism on each lock.
public class VampireLock : XRSocketInteractor
{
    [SerializeField] private VampireRelic requiredRelic;
    [SerializeField] private Transform movingPart;
    [SerializeField] private Vector3 openedLocalPosition;
    [SerializeField] private Vector3 openedLocalEulerAngles;
    [SerializeField] private TMP_Text statusLabel;
    [SerializeField] private Renderer statusGem;
    [SerializeField] private Material acceptingMaterial;
    [SerializeField] private Material unlockedMaterial;
    [SerializeField] private UnityEvent onUnlocked = new UnityEvent();
    private Material sealedMaterial;
    private Vector3 closedPosition;
    private Quaternion closedRotation;
    private float openingTime;
    private bool animating;
    private MaterialPropertyBlock indicatorProperties;
    private Color indicatorStart;

    public VampireRelic RequiredRelic => requiredRelic;
    public bool Unlocked { get; private set; }
    public bool OpeningComplete => Unlocked && !animating;
    public bool HasMatchingKeyInstalled
    {
        get
        {
            foreach (var selected in interactablesSelected)
                if (selected is VampireKey key && key.Relic == requiredRelic && key.IsInstalled)
                    return true;
            return false;
        }
    }
    public UnityEvent OnUnlocked => onUnlocked;

    protected override void Awake()
    {
        base.Awake();
        if (movingPart != null)
        {
            closedPosition = movingPart.localPosition;
            closedRotation = movingPart.localRotation;
        }
        if (statusGem != null) sealedMaterial = statusGem.sharedMaterial;
    }

    private bool Matches(IXRInteractable target)
    {
        return target is VampireKey key && key.Relic == requiredRelic;
    }

    public override bool CanHover(IXRHoverInteractable target)
    {
        return !Unlocked && Matches(target) && base.CanHover(target);
    }

    public override bool CanSelect(IXRSelectInteractable target)
    {
        return Matches(target) && (!Unlocked || IsSelecting(target)) && base.CanSelect(target);
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        if (Unlocked) return;
        if (statusLabel != null) statusLabel.text = "RELEASE TO OFFER";
        if (statusGem != null) statusGem.sharedMaterial = acceptingMaterial;
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        if (Unlocked || hasHover) return;
        if (statusLabel != null) statusLabel.text = "SEALED";
        if (statusGem != null) statusGem.sharedMaterial = sealedMaterial;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (Unlocked || !(args.interactableObject is VampireKey key) || key.Relic != requiredRelic) return;
        Unlocked = true;
        key.Install(this);
        if (statusLabel != null) statusLabel.text = "UNLOCKED";
        if (statusGem != null)
        {
            indicatorStart = statusGem.sharedMaterial.color;
            indicatorProperties = new MaterialPropertyBlock();
        }
        openingTime = 0f;
        animating = true;
        onUnlocked.Invoke();
    }

    private void Update()
    {
        if (!animating) return;
        openingTime += Time.deltaTime;
        float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(openingTime / 1.1f));
        if (movingPart != null)
        {
            movingPart.localPosition = Vector3.Lerp(closedPosition, openedLocalPosition, t);
            movingPart.localRotation = Quaternion.Slerp(closedRotation, Quaternion.Euler(openedLocalEulerAngles), t);
        }
        if (statusGem != null && unlockedMaterial != null)
        {
            indicatorProperties.SetColor("_BaseColor", Color.Lerp(indicatorStart, unlockedMaterial.color, t));
            statusGem.SetPropertyBlock(indicatorProperties);
        }
        if (openingTime >= 1.1f)
        {
            animating = false;
            if (statusGem != null && unlockedMaterial != null)
            {
                statusGem.sharedMaterial = unlockedMaterial;
                statusGem.SetPropertyBlock(null);
            }
        }
    }
}
