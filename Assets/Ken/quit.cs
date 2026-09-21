using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Quit : MonoBehaviour
{
    public InputActionReference action;
    [SerializeField] private SpatialSoundFeedback soundFeedback;
    private bool quitting;

    void Start()
    {
        action.action.Enable();
        action.action.performed += OnQuit;
    }

    private void OnQuit(InputAction.CallbackContext context)
    {
        if (quitting) return;
        quitting = true;
        StartCoroutine(QuitAfterSound());
    }

    private IEnumerator QuitAfterSound()
    {
        float duration = soundFeedback != null ? soundFeedback.PlayFarewell() : 0f;
        if (duration > 0f) yield return new WaitForSecondsRealtime(duration + 0.05f);
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void OnDestroy()
    {
        if (action != null) action.action.performed -= OnQuit;
    }
}
