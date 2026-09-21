using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

// One saved key per pedestrian. Collected keys travel on the right-hand key ring.
public class PedestrianCarKey : XRBaseInteractable
{
    [SerializeField] private Transform rightController;
    [SerializeField] private Transform keyModel;
    [SerializeField] private TMP_Text hint;
    [SerializeField] private string carColor;
    [SerializeField] private Vector3 carryPosition;
    [SerializeField] private float collectDistance = 2.5f;
    private bool pendingCollection;
    public bool HasKey { get; private set; }
    public bool IsUsed { get; private set; }
    public string CarColor => carColor;

    private bool CanCollect(IXRInteractor interactor)
        => !HasKey && !IsUsed && !pendingCollection && rightController != null &&
           interactor.transform.IsChildOf(rightController) &&
           Vector3.Distance(rightController.position, transform.position + Vector3.up) <= collectDistance;

    public override bool IsHoverableBy(IXRHoverInteractor interactor)
        => CanCollect(interactor) && base.IsHoverableBy(interactor);
    public override bool IsSelectableBy(IXRSelectInteractor interactor)
        => CanCollect(interactor) && base.IsSelectableBy(interactor);

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (CanCollect(args.interactorObject) && args.interactorObject is XRBaseInputInteractor input &&
            input.selectInput.ReadWasPerformedThisFrame()) pendingCollection = true;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        if (hint != null) hint.color = Color.cyan;
    }
    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        if (hint != null && !isHovered) hint.color = Color.white;
    }

    private void LateUpdate()
    {
        if (!pendingCollection) return;
        pendingCollection = false;
        HasKey = true;
        if (interactionManager != null) interactionManager.CancelInteractableSelection((IXRSelectInteractable)this);
        keyModel.SetParent(rightController, false);
        keyModel.localPosition = carryPosition;
        keyModel.localRotation = Quaternion.identity;
        var sound = keyModel.GetComponent<PickupSound>();
        if (sound != null) sound.PlayPickup();
        if (hint != null) hint.text = carColor + " KEY COLLECTED\nUse Left Secondary at the matching car";
    }

    public bool TryUseKey()
    {
        if (IsUsed) return true;
        if (!HasKey) return false;
        HasKey = false;
        IsUsed = true;
        if (keyModel != null) Destroy(keyModel.gameObject);
        if (hint != null) hint.text = carColor + " CAR UNLOCKED\nKey used";
        return true;
    }

    protected override void OnDestroy()
    {
        if (HasKey && keyModel != null) Destroy(keyModel.gameObject);
        base.OnDestroy();
    }
}
