using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>Owns the one-way journey from the foyer to the first escape room.</summary>
public sealed class StartScreenController : MonoBehaviour
{
    [SerializeField] private string firstRoomPath = "Assets/Ken/Scenes/Ken's_room.unity";
    [SerializeField] private Button startButton;
    [SerializeField] private TMP_Text statusLabel;

    public bool IsLoading { get; private set; }

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard != null && (keyboard.enterKey.wasPressedThisFrame || keyboard.numpadEnterKey.wasPressedThisFrame))
            StartGame();
    }

    // The scene button's persistent OnClick event also calls this method for XR/mouse input.
    public void StartGame()
    {
        if (!isActiveAndEnabled || IsLoading) return;
        if (string.IsNullOrWhiteSpace(firstRoomPath) || !Application.CanStreamedLevelBeLoaded(firstRoomPath))
        {
            SetStatus("The room is unavailable. Check the enabled build scenes.");
            Debug.LogError("Start screen cannot load scene: " + firstRoomPath, this);
            return;
        }

        IsLoading = true;
        if (startButton != null) startButton.interactable = false;
        SetStatus("Opening Ken's room...");
        try
        {
            // Unload the foyer and its rig before beginning the challenge.
            if (SceneManager.LoadSceneAsync(firstRoomPath, LoadSceneMode.Single) != null) return;
        }
        catch (Exception exception)
        {
            Debug.LogException(exception, this);
        }

        IsLoading = false;
        if (startButton != null) startButton.interactable = true;
        SetStatus("Could not open the room. Select Start Game to retry.");
    }

    private void SetStatus(string message)
    {
        if (statusLabel != null) statusLabel.text = message;
    }
}
