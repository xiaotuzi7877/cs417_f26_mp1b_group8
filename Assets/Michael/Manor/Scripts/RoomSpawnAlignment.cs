using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

namespace MichaelManor
{
    // Hold locomotion until the arriving camera has consumed its tracked pose.
    [DefaultExecutionOrder(-100)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(XROrigin))]
    public sealed class RoomSpawnAlignment : MonoBehaviour
    {
        [SerializeField] private BreakOut returnPointOwner;
        private XROrigin origin;
        private XRBodyTransformer body;
        private Vector3 spawnPosition;
        private bool resumeBody;

        private void Awake()
        {
            origin = GetComponent<XROrigin>();
            if (origin.Camera == null)
            {
                Debug.LogError("Room spawn alignment requires the XR Origin's camera.", this);
                return;
            }

            spawnPosition = origin.Origin.transform.position;
            body = origin.GetComponentInChildren<XRBodyTransformer>();
            resumeBody = body != null && body.enabled;
            if (resumeBody) body.enabled = false;
        }

        private void Start()
        {
            if (origin.Camera == null) return;
            // TrackedPoseDriver subscribes in OnEnable. Subscribing in Start puts
            // this callback after it, so the pose is current instead of the saved
            // camera transform from the scene file.
            InputSystem.onAfterUpdate += AlignAfterInput;
        }

        private void AlignAfterInput()
        {
            if ((InputState.currentUpdateType & (InputUpdateType.Dynamic | InputUpdateType.Fixed)) == 0) return;
            InputSystem.onAfterUpdate -= AlignAfterInput;

            // A persistent simulator (or room-scale headset) can have a nonzero
            // tracking-space offset. Position the player's feet at the authored
            // spawn, rather than assuming the camera is centered on the rig.
            var spawn = origin.Origin.transform;
            var headOffset = origin.Camera.transform.position - spawn.position;
            var targetEyePosition = spawnPosition + Vector3.Project(headOffset, spawn.up);
            var controller = origin.GetComponent<CharacterController>();
            bool restoreController = controller != null && controller.enabled;
            if (restoreController) controller.enabled = false;
            origin.MoveCameraToWorldLocation(targetEyePosition);
            if (restoreController) controller.enabled = true;
            Physics.SyncTransforms();
            if (returnPointOwner != null) returnPointOwner.CaptureInsidePose();
            ResumeLocomotion();
        }

        private void OnDisable()
        {
            InputSystem.onAfterUpdate -= AlignAfterInput;
            ResumeLocomotion();
        }

        private void ResumeLocomotion()
        {
            if (resumeBody && body != null) body.enabled = true;
            resumeBody = false;
        }
    }
}
