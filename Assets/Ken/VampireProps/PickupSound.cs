using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

// Set the source clip to yeah for a key or hmm for a decoy.
[RequireComponent(typeof(AudioSource))]
public class PickupSound : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable grab;
    private AudioSource source;

    private void Awake() => source = GetComponent<AudioSource>();
    private void OnEnable()
    {
        if (grab != null) grab.selectEntered.AddListener(OnGrab);
    }
    private void OnDisable()
    {
        if (grab != null) grab.selectEntered.RemoveListener(OnGrab);
    }
    private void OnGrab(SelectEnterEventArgs args)
    {
        // Installing a key in a socket is not another player pickup.
        if (args.interactorObject is XRBaseInputInteractor) PlayPickup();
    }
    public void PlayPickup()
    {
        if (source != null && source.clip != null && isActiveAndEnabled)
            source.Play();
    }
}
