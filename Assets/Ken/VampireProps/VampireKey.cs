using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public enum VampireRelic { BloodVial, SilverStake, BatMedallion }

// A physical XR Grab Interactable; the identity is independent of its color/name.
public class VampireKey : XRGrabInteractable
{
    [SerializeField] private VampireRelic relic;
    [SerializeField] private Renderer grabAccent;
    private VampireLock installedIn;
    private MaterialPropertyBlock accentProperties;
    public VampireRelic Relic => relic;
    public bool IsInstalled => installedIn != null;

    // Direct gate travel bypasses locomotion-provider events. Move the held body with the rig
    // and exclude the discontinuity from release velocity until the smoothing window clears.
    private float gateThrowResumeTime;
    private bool restoreGateThrow;
    public void FollowGate(Pose before, Pose after)
    {
        var rotation = after.rotation * Quaternion.Inverse(before.rotation);
        var pose = new Pose(after.position + rotation * (transform.position - before.position), rotation * transform.rotation);
        transform.SetPositionAndRotation(pose.position, pose.rotation);
        SetTargetPose(pose);
        var body = GetComponent<Rigidbody>();
        body.position = pose.position;
        body.rotation = pose.rotation;
        if (!body.isKinematic) { body.linearVelocity = Vector3.zero; body.angularVelocity = Vector3.zero; }
        restoreGateThrow |= throwOnDetach;
        throwOnDetach = false;
        gateThrowResumeTime = Time.time + throwSmoothingDuration + 0.1f;
    }

    private void LateUpdate()
    {
        if (restoreGateThrow && Time.time >= gateThrowResumeTime)
        {
            throwOnDetach = true;
            restoreGateThrow = false;
        }
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        return (!IsInstalled || ReferenceEquals(interactor, installedIn)) && base.IsSelectableBy(interactor);
    }

    public void Install(VampireLock socket)
    {
        if (socket == null || socket.RequiredRelic != relic || !socket.IsSelecting(this)) return;
        installedIn = socket;
        RefreshAffordance();
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        RefreshAffordance();
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        RefreshAffordance();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        RefreshAffordance();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        RefreshAffordance();
    }

    private void RefreshAffordance()
    {
        if (grabAccent == null) return;
        bool targeted = false;
        foreach (var interactor in interactorsHovering)
            if (interactor is XRBaseInputInteractor) targeted = true;
        if (IsInstalled || (!targeted && !isSelected))
        {
            grabAccent.SetPropertyBlock(null);
            return;
        }
        if (accentProperties == null) accentProperties = new MaterialPropertyBlock();
        accentProperties.SetColor("_BaseColor", isSelected
            ? new Color(0.2f, 0.9f, 1f)
            : new Color(1f, 0.9f, 0.55f));
        grabAccent.SetPropertyBlock(accentProperties);
    }
}
