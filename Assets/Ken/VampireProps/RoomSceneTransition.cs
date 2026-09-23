using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Replaces this room only after its authoritative puzzle outcome permits it.
[DisallowMultipleComponent]
[RequireComponent(typeof(VampireEscape))]
public sealed class RoomSceneTransition : MonoBehaviour
{
    [SerializeField] private InputActionAsset controllerActions;
    [SerializeField] private string continueActionPath = "LeftHand/Primary2DAxisClick";
    [SerializeField] private string nextScenePath = "Assets/Michael/Scenes/MichaelManorHall.unity";

    private VampireEscape puzzle;
    private InputAction continueAction;
    private bool enabledAction;
    private bool inputReleased;
    private bool loading;

    public bool IsLoading => loading;

    private void Awake()
    {
        puzzle = GetComponent<VampireEscape>();
        if (controllerActions != null)
            continueAction = controllerActions.FindAction(continueActionPath, false);
        if (continueAction == null)
            Debug.LogError("Assign the controller actions and left thumbstick click action to RoomSceneTransition.", this);
    }

    private void OnEnable()
    {
        puzzle.OnEscaped.AddListener(ShowContinuePrompt);
        inputReleased = false;
        if (continueAction != null)
        {
            enabledAction = !continueAction.enabled;
            if (enabledAction) continueAction.Enable();
        }
        if (puzzle.HasWon) ShowContinuePrompt();
    }

    private void OnDisable()
    {
        puzzle.OnEscaped.RemoveListener(ShowContinuePrompt);
        if (enabledAction && continueAction != null) continueAction.Disable();
        enabledAction = false;
    }

    private void ShowContinuePrompt()
    {
        inputReleased = false;
        puzzle.SetContinuationPrompt("NEXT: MICHAEL'S ROOM\nPress LEFT THUMBSTICK (click) or keyboard N");
    }

    private void Update()
    {
        if (!puzzle.HasWon || loading) return;
        var keyboard = Keyboard.current;
        bool held = (continueAction != null && continueAction.IsPressed())
            || (keyboard != null && keyboard.nKey.isPressed);

        // A button held while winning must be released before continuing.
        if (!inputReleased)
        {
            inputReleased = !held;
            return;
        }
        if ((continueAction != null && continueAction.WasPressedThisFrame())
            || (keyboard != null && keyboard.nKey.wasPressedThisFrame))
            TryLoadNextRoom();
    }

    // May also be called by a future UI button; the win gate still applies.
    public bool TryLoadNextRoom()
    {
        if (!isActiveAndEnabled || !puzzle.HasWon || loading) return false;
        if (string.IsNullOrWhiteSpace(nextScenePath) || !Application.CanStreamedLevelBeLoaded(nextScenePath))
        {
            puzzle.SetContinuationPrompt("Michael's room is unavailable. Check the enabled build scenes.");
            Debug.LogError("RoomSceneTransition cannot load scene: " + nextScenePath, this);
            return false;
        }

        loading = true;
        puzzle.SetContinuationPrompt("LOADING MICHAEL'S ROOM...");
        try
        {
            // Single unloads the old room and its XR rig, cameras, audio and colliders.
            // Both rooms can therefore retain their original coordinates at the origin.
            if (SceneManager.LoadSceneAsync(nextScenePath, LoadSceneMode.Single) != null)
                return true;
        }
        catch (Exception exception)
        {
            Debug.LogException(exception, this);
        }
        loading = false;
        puzzle.SetContinuationPrompt("Could not load Michael's room. Press LEFT THUMBSTICK or N to retry.");
        return false;
    }
}
