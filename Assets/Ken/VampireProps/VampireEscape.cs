using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

// Owns the round outcome. Installed keys stay visible in their three sockets after winning.
public class VampireEscape : MonoBehaviour
{
    [SerializeField] private TMP_Text heading;
    [SerializeField] private TMP_Text progress;
    [SerializeField] private UnityEvent onEscaped = new UnityEvent();
    private VampireLock[] locks;
    private Coroutine completion;
    public bool HasWon { get; private set; }
    public UnityEvent OnEscaped => onEscaped;

    public void SetContinuationPrompt(string message)
    {
        if (HasWon && progress != null) progress.text = message;
    }

    private void Awake()
    {
        locks = GetComponentsInChildren<VampireLock>(true);
        int identities = 0;
        foreach (var socket in locks) identities |= 1 << (int)socket.RequiredRelic;
        if (locks.Length != 3 || identities != 7)
        {
            Debug.LogError("VampireEscape requires one lock for each of the three relics.", this);
            enabled = false;
            return;
        }
        if (heading == null || progress == null)
        {
            Debug.LogError("Assign the escape Canvas heading and progress TMP text.", this);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        foreach (var socket in locks) socket.OnUnlocked.AddListener(Refresh);
        Refresh();
    }

    private void OnDisable()
    {
        if (locks != null)
            foreach (var socket in locks) socket.OnUnlocked.RemoveListener(Refresh);
        if (completion != null) StopCoroutine(completion);
        completion = null;
    }

    private void Refresh()
    {
        if (HasWon || progress == null) return;
        int solved = 0;
        string rows = "";
        foreach (var socket in locks)
        {
            bool installed = socket.Unlocked && socket.HasMatchingKeyInstalled;
            if (installed) solved++;
            string pair = socket.RequiredRelic == VampireRelic.BloodVial ? "BLOOD VIAL  ->  CHALICE"
                : socket.RequiredRelic == VampireRelic.SilverStake ? "SILVER STAKE  ->  COFFIN"
                : "BAT MEDALLION  ->  CRYPT";
            rows += (installed ? "[OPEN]  " : "[SEALED]  ") + pair + "\n";
        }
        heading.text = "ESCAPE THE VAMPIRE'S SEALS  " + solved + "/3";
        progress.text = rows.TrimEnd();
        if (solved == 3 && completion == null) completion = StartCoroutine(CompleteEscape());
    }

    private IEnumerator CompleteEscape()
    {
        // Wait for the final key to settle and the eased mechanisms to finish before declaring victory.
        yield return null;
        while (true)
        {
            bool ready = true;
            foreach (var socket in locks)
                ready &= socket.OpeningComplete && socket.HasMatchingKeyInstalled;
            if (ready) break;
            yield return null;
        }
        HasWon = true;
        heading.text = "YOU ESCAPED!  ALL 3 SEALS BROKEN";
        onEscaped.Invoke();
        Color start = heading.color;
        for (float time = 0f; time < 1f; time += Time.deltaTime)
        {
            heading.color = Color.Lerp(start, new Color(0.3f, 1f, 0.55f), Mathf.SmoothStep(0f, 1f, time));
            yield return null;
        }
        heading.color = new Color(0.3f, 1f, 0.55f);
        completion = null;
    }

}
