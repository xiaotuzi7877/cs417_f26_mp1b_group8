using TMPro;
using System;
using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

// Uses the left controller's pointing ray and existing secondary-button action for both doors.
public class CryptDoorTeleport : MonoBehaviour
{
    [Serializable]
    public class Gate
    {
        public Collider target;
        public Transform destination;
        public TMP_Text hint;
        public string label;
        public Transform hinge;
        public bool groundPortal;
        public bool requiresCarKey;
        public PedestrianCarKey carKey;
    }
    [SerializeField] private Gate[] additionalGates = Array.Empty<Gate>();
    [SerializeField] private XROrigin xrOrigin;
    [SerializeField] private Transform leftController;
    [SerializeField] private InputActionReference leftSecondary;
    [SerializeField] private BoardedCryptDoor seal;
    [SerializeField] private Collider entranceDoor;
    [SerializeField] private Collider returnDoor;
    [SerializeField] private Transform roomArrival;
    [SerializeField] private Transform mainArrival;
    [SerializeField] private TMP_Text entranceHint;
    [SerializeField] private TMP_Text returnHint;
    [SerializeField] private float aimDistance = 10f;
    private NearFarInteractor leftInteractor;
    private CharacterController body;
    private bool enabledAction;
    private float nextTeleportTime;
    private Collider lastTarget;
    private bool lastReady;
    private VampireKey[] keys;
    private Transform openingHinge;
    private Quaternion hingeClosedRotation;

    private void Awake()
    {
        if (xrOrigin == null || leftController == null || leftSecondary == null)
        {
            Debug.LogError("Assign the scene XR Origin, left controller and secondary action on CryptDoorTeleport.", this);
            enabled = false;
            return;
        }
        leftInteractor = leftController.GetComponentInChildren<NearFarInteractor>(true);
        body = xrOrigin.Origin.GetComponent<CharacterController>();
        keys = GetComponentInParent<VampireEscape>().GetComponentsInChildren<VampireKey>(true);
    }

    private void OnEnable()
    {
        if (leftSecondary == null) return;
        enabledAction = !leftSecondary.action.enabled;
        if (enabledAction) leftSecondary.action.Enable();
        leftSecondary.action.performed += OnSecondary;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        if (openingHinge != null) openingHinge.localRotation = hingeClosedRotation;
        openingHinge = null;
        nextTeleportTime = 0f;
        if (leftSecondary == null) return;
        leftSecondary.action.performed -= OnSecondary;
        if (enabledAction) leftSecondary.action.Disable();
    }

    private Collider AimedDoor()
    {
        var aim = leftInteractor != null && leftInteractor.farInteractionCaster != null
            ? leftInteractor.farInteractionCaster.effectiveCastOrigin : leftController;
        if (!Physics.Raycast(aim.position, aim.forward, out var hit, aimDistance, ~0, QueryTriggerInteraction.Ignore))
            return null;
        var target = hit.collider;
        Gate gate = FindGate(target);
        if (target != entranceDoor && target != returnDoor && gate == null) return null;
        if (gate != null) target = gate.target;
        // Only the visible front face of each door is a gate; never teleport through its back.
        if (gate == null || !gate.groundPortal)
            if (Vector3.Dot(aim.position - target.bounds.center, -target.transform.forward) <= 0f) return null;
        return target;
    }

    private Gate FindGate(Collider target)
    {
        foreach (var gate in additionalGates)
            if (gate.target != null && target != null && (gate.target == target ||
                (gate.hinge != null && target.transform.IsChildOf(gate.hinge)))) return gate;
        return null;
    }

    private void Update()
    {
        bool ready = seal.IsRevealed;
        var target = AimedDoor();
        // Inventory can change while the player keeps aiming at the same door.
        foreach (var gate in additionalGates)
            if (gate.requiresCarKey && gate.hint != null)
            {
                bool unlocked = gate.carKey != null && gate.carKey.IsUsed;
                bool hasKey = gate.carKey != null && gate.carKey.HasKey;
                gate.hint.text = unlocked || hasKey ? "car unlocked" : "car locked";
                gate.hint.color = target == gate.target ? Color.cyan : unlocked ? new Color(.3f, 1f, .55f) : Color.white;
            }
        if (target == lastTarget && ready == lastReady) return;
        lastTarget = target;
        lastReady = ready;
        foreach (var gate in additionalGates)
            if (gate.hint != null)
                gate.hint.color = target == gate.target ? Color.cyan : Color.white;
        if (ready && entranceHint != null)
        {
            entranceHint.text = target == entranceDoor ? "LEFT SECONDARY - ENTER CRYPT" : "CRYPT READY - AIM LEFT + SECONDARY";
            entranceHint.color = target == entranceDoor ? Color.cyan : new Color(0.3f, 1f, 0.55f);
        }
        if (returnHint != null)
        {
            returnHint.text = target == returnDoor ? "LEFT SECONDARY - RETURN" : "MAIN ROOM - AIM LEFT + SECONDARY";
            returnHint.color = target == returnDoor ? Color.cyan : Color.white;
        }
    }

    private void OnSecondary(InputAction.CallbackContext context)
    {
        if (Time.unscaledTime < nextTeleportTime) return;
        var target = AimedDoor();
        if (target == null) return;
        var gate = FindGate(target);
        if (gate != null)
        {
            if (gate.destination == null) return;
            if (gate.requiresCarKey && (gate.carKey == null || !gate.carKey.TryUseKey())) return;
            nextTeleportTime = float.PositiveInfinity;
            StartCoroutine(EnterGate(gate));
            return;
        }
        if (!seal.IsRevealed) return;
        var destination = target == entranceDoor ? roomArrival : mainArrival;
        Teleport(destination);
    }

    private IEnumerator EnterGate(Gate gate)
    {
        Quaternion closed = gate.hinge != null ? gate.hinge.localRotation : Quaternion.identity;
        openingHinge = gate.hinge;
        hingeClosedRotation = closed;
        if (gate.hinge != null)
        {
            for (float t = 0f; t < 0.45f; t += Time.unscaledDeltaTime)
            {
                gate.hinge.localRotation = closed * Quaternion.Euler(0f, -80f * Mathf.SmoothStep(0f, 1f, t / 0.45f), 0f);
                yield return null;
            }
        }
        Teleport(gate.destination);
        if (gate.hinge != null) gate.hinge.localRotation = closed;
        openingHinge = null;
    }

    private void Teleport(Transform destination)
    {
        var origin = xrOrigin.Origin.transform;
        var before = new Pose(origin.position, origin.rotation);
        float height = xrOrigin.CameraInOriginSpaceHeight;
        bool restoreBody = body != null && body.enabled;
        if (restoreBody) body.enabled = false;
        xrOrigin.MatchOriginUpCameraForward(Vector3.up, destination.forward);
        xrOrigin.MoveCameraToWorldLocation(destination.position + Vector3.up * height);
        var after = new Pose(origin.position, origin.rotation);
        foreach (var key in keys)
            if (key != null && key.isSelected && !key.IsInstalled) key.FollowGate(before, after);
        if (restoreBody) body.enabled = true;
        Physics.SyncTransforms();
        nextTeleportTime = Time.unscaledTime + 0.5f;
    }
}
