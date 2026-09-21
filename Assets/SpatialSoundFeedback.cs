using UnityEngine;

// Sources and clips are authored in the hierarchy; this component only triggers them.
public class SpatialSoundFeedback : MonoBehaviour
{
    [SerializeField] private AudioSource[] normalBodySounds;
    [SerializeField] private AudioSource[] darkBodySounds;
    [SerializeField] private AudioSource[] peopleGasps;
    [SerializeField] private AudioSource teleportDeparture;
    [SerializeField] private AudioSource teleportArrival;
    [SerializeField] private AudioSource welcome;
    [SerializeField] private AudioSource farewell;
    [SerializeField] private Transform listener;

    private bool welcomed;

    public void WelcomeOnce()
    {
        if (welcomed) return;
        welcomed = true;
        PlaceNearListener(welcome);
        Play(welcome);
    }

    public void SetBloodMoonActive(bool active)
    {
        WelcomeOnce();
        Stop(normalBodySounds);
        Stop(darkBodySounds);
        Stop(peopleGasps);
        Play(active ? darkBodySounds : normalBodySounds);
        if (active) Play(peopleGasps);
    }

    public void BeforeTeleport()
    {
        WelcomeOnce();
        PlaceNearListener(teleportDeparture);
        Play(teleportDeparture);
    }

    public void AfterTeleport()
    {
        PlaceNearListener(teleportArrival);
        Play(teleportArrival);
    }

    public float PlayFarewell()
    {
        Stop(normalBodySounds);
        Stop(darkBodySounds);
        Stop(peopleGasps);
        Stop(welcome);
        Stop(teleportDeparture);
        Stop(teleportArrival);
        PlaceNearListener(farewell);
        Play(farewell);
        return farewell != null && farewell.clip != null
            ? farewell.clip.length / Mathf.Max(Mathf.Abs(farewell.pitch), 0.01f)
            : 0f;
    }

    private void PlaceNearListener(AudioSource source)
    {
        if (source != null && listener != null)
            source.transform.position = listener.position + listener.forward;
    }

    private static void Play(AudioSource source)
    {
        if (source == null || source.clip == null) return;
        source.Stop();
        source.Play();
    }

    private static void Play(AudioSource[] sources)
    {
        if (sources == null) return;
        foreach (AudioSource source in sources) Play(source);
    }

    private static void Stop(AudioSource source)
    {
        if (source != null) source.Stop();
    }

    private static void Stop(AudioSource[] sources)
    {
        if (sources == null) return;
        foreach (AudioSource source in sources) Stop(source);
    }

    private void OnDisable()
    {
        Stop(normalBodySounds);
        Stop(darkBodySounds);
        Stop(peopleGasps);
        Stop(teleportDeparture);
        Stop(teleportArrival);
        Stop(welcome);
        Stop(farewell);
    }
}
