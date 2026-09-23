using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnPoint;
    public InputActionReference action;
    public float shootSpeed = 5f;

    void Start()
    {
        if (prefabToSpawn == null)
            Debug.LogError("OBJECT SPAWNER: prefabToSpawn is EMPTY!");

        if (spawnPoint == null)
            Debug.LogError("OBJECT SPAWNER: spawnPoint is EMPTY!");

        if (action == null)
            Debug.LogError("OBJECT SPAWNER: action is EMPTY!");

        if (action != null)
        {
            action.action.Enable();

            action.action.performed += OnSpawn;
        }
    }

    void OnSpawn(InputAction.CallbackContext ctx)
    {
        Debug.Log("SPAWN BUTTON PRESSED");

        if (prefabToSpawn == null)
        {
            Debug.LogError("Cannot spawn: prefabToSpawn is EMPTY!");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("Cannot spawn: spawnPoint is EMPTY!");
            return;
        }

        GameObject spawned = Instantiate(
            prefabToSpawn,
            spawnPoint.position,
            spawnPoint.rotation
        );

        ProjectileMotion motion = spawned.GetComponent<ProjectileMotion>();

        if (motion != null)
        {
            motion.velocity = spawnPoint.forward * shootSpeed;
        }
    }
}