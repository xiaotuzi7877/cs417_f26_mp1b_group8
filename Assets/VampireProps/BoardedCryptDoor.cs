using UnityEngine;
using TMPro;

// Two completed offerings remove the barricade and make the closed teleport door available.
public class BoardedCryptDoor : MonoBehaviour
{
    [SerializeField] private VampireLock bloodTithe;
    [SerializeField] private VampireLock huntersSeal;
    [SerializeField] private Transform[] boards;
    [SerializeField] private Transform doorHinge;
    [SerializeField] private Collider passageBlocker;
    [SerializeField] private Behaviour[] revealedInteractions;
    [SerializeField] private TMP_Text statusLabel;
    private Vector3[] boardPositions;
    private Quaternion[] boardRotations;
    private bool opening;
    private float elapsed;
    public bool IsRevealed { get; private set; }

    private void Awake()
    {
        boardPositions = new Vector3[boards.Length];
        boardRotations = new Quaternion[boards.Length];
        for (int i = 0; i < boards.Length; i++)
        {
            boardPositions[i] = boards[i].localPosition;
            boardRotations[i] = boards[i].localRotation;
        }
        foreach (var interaction in revealedInteractions) interaction.enabled = false;
    }

    private void OnEnable()
    {
        bloodTithe.OnUnlocked.AddListener(CheckLocks);
        huntersSeal.OnUnlocked.AddListener(CheckLocks);
        CheckLocks();
    }

    private void OnDisable()
    {
        bloodTithe.OnUnlocked.RemoveListener(CheckLocks);
        huntersSeal.OnUnlocked.RemoveListener(CheckLocks);
    }

    private void CheckLocks()
    {
        if (IsRevealed || opening) return;
        int solved = (bloodTithe.Unlocked ? 1 : 0) + (huntersSeal.Unlocked ? 1 : 0);
        if (statusLabel != null) statusLabel.text = "CRYPT SEALED  " + solved + "/2";
        if (solved == 2) opening = true;
    }

    private void Update()
    {
        if (!opening || !bloodTithe.OpeningComplete || !huntersSeal.OpeningComplete) return;
        elapsed += Time.deltaTime;
        float boardsT = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / 1.1f));
        for (int i = 0; i < boards.Length; i++)
        {
            float side = i % 2 == 0 ? -1f : 1f;
            boards[i].localPosition = boardPositions[i] + new Vector3(side * 2.6f, -1.6f, -0.3f) * boardsT;
            boards[i].localRotation = boardRotations[i] * Quaternion.Euler(0, 0, side * 65f * boardsT);
            if (elapsed >= 1.1f) boards[i].gameObject.SetActive(false);
        }
        if (elapsed < 1.1f) return;
        foreach (var interaction in revealedInteractions) interaction.enabled = true;
        IsRevealed = true;
        opening = false;
        if (statusLabel != null)
        {
            statusLabel.text = "CRYPT READY - AIM LEFT + SECONDARY";
            statusLabel.color = new Color(0.3f, 1f, 0.55f, 1f);
        }
    }
}
