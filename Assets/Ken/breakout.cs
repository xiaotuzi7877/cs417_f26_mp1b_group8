using UnityEngine;
using UnityEngine.InputSystem;

public class BreakOut : MonoBehaviour
{
    public InputActionReference action;
    public Transform externalViewingPoint;
    [SerializeField] private ParticleSystem viewChangeParticle;
    [SerializeField] private SpatialSoundFeedback soundFeedback;

    private Vector3 roomPosition;
    private bool isOutside = false;

    void Start()
    {
        // The player starts inside the room.
        roomPosition = transform.position;

        action.action.Enable();
        action.action.performed += (ctx) =>
        {
            if (soundFeedback != null) soundFeedback.BeforeTeleport();
            isOutside = !isOutside;

            if (isOutside)
            {
                transform.position = externalViewingPoint.position;
            }
            else
            {
                transform.position = roomPosition;
            }

            if (soundFeedback != null) soundFeedback.AfterTeleport();

            if (viewChangeParticle != null)
            {
                viewChangeParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                viewChangeParticle.Play(true);
            }
        };
    }
}
