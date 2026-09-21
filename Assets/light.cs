using UnityEngine;
using UnityEngine.InputSystem;

public class LightSwitch : MonoBehaviour
{
    public Light light;
    public InputActionReference action;
    [SerializeField] private BloodMoonEffects bloodMoonEffects;
    [SerializeField] private SpatialSoundFeedback soundFeedback;

    [SerializeField] private Color bloodMoonColor = new Color(0.55f, 0.015f, 0.025f);
    [SerializeField, Range(0f, 1f)] private float bloodMoonIntensityMultiplier = 0.65f;

    private Color defaultColor;
    private float defaultIntensity;
    private bool bloodMoonActive;
    private bool enabledAction;

    private void Awake()
    {
        if (light == null)
        {
            light = GetComponent<Light>();
        }

        if (light == null)
        {
            Debug.LogError("Light Switch needs a Light.", this);
            enabled = false;
            return;
        }

        // Remember the scene's lighting without changing its initial appearance.
        defaultColor = light.color;
        defaultIntensity = light.intensity;
    }

    private void OnEnable()
    {
        if (action == null)
        {
            Debug.LogError("Light Switch needs an input action.", this);
            return;
        }

        action.action.performed += ToggleBloodMoon;
        if (!action.action.enabled)
        {
            action.action.Enable();
            enabledAction = true;
        }
    }

    private void OnDisable()
    {
        if (action == null)
        {
            return;
        }

        action.action.performed -= ToggleBloodMoon;
        if (enabledAction)
        {
            action.action.Disable();
            enabledAction = false;
        }
    }

    private void ToggleBloodMoon(InputAction.CallbackContext context)
    {
        bloodMoonActive = !bloodMoonActive;
        light.color = bloodMoonActive ? bloodMoonColor : defaultColor;
        light.intensity = bloodMoonActive
            ? defaultIntensity * bloodMoonIntensityMultiplier
            : defaultIntensity;
        if (bloodMoonEffects != null)
        {
            bloodMoonEffects.SetBloodMoonActive(bloodMoonActive);
        }
        if (soundFeedback != null)
        {
            soundFeedback.SetBloodMoonActive(bloodMoonActive);
        }
    }
}
