using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGate : MonoBehaviour
{
    public string targetSceneName;
    public PlayerInventoryCarry inventoryCarry;
    public void TeleportToNextScene()
    {
        inventoryCarry.ProtectHeldItems();
        SceneManager.LoadScene(targetSceneName);
    }
}