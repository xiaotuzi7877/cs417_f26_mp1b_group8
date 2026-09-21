using UnityEngine;

// All particle settings live on scene/prefab children, editable in the Inspector.
public class BloodMoonEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] clouds = new ParticleSystem[0];
    [SerializeField] private ParticleSystem[] bursts = new ParticleSystem[0];
    private bool bloodMoonActive;

    private void Awake()
    {
        SetPlaying(clouds, false);
        SetPlaying(bursts, false);
        if (clouds.Length == 0 && bursts.Length == 0)
        {
            Debug.LogError("Blood Moon Effects has no scene emitters assigned. Reload the updated SampleScene outside Play mode.", this);
        }
    }

    public void SetBloodMoonActive(bool active)
    {
        if ((active && !isActiveAndEnabled) || bloodMoonActive == active) return;
        bloodMoonActive = active;
        SetPlaying(clouds, active);
        SetPlaying(bursts, active);
    }

    private static void SetPlaying(ParticleSystem[] emitters, bool active)
    {
        foreach (ParticleSystem emitter in emitters)
        {
            if (emitter == null) continue;
            emitter.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            if (active) emitter.Play(true);
        }
    }

    private void OnDisable()
    {
        bloodMoonActive = false;
        SetPlaying(clouds, false);
        SetPlaying(bursts, false);
    }
}
