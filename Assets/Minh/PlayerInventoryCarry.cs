using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PlayerInventoryCarry : MonoBehaviour
{
    public NearFarInteractor leftInteractor;
    public NearFarInteractor rightInteractor;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ProtectHeldItems()
    {
        ProtectIfHolding(leftInteractor);
        ProtectIfHolding(rightInteractor);
    }

    private void ProtectIfHolding(NearFarInteractor interactor)
    {
        if (interactor.interactablesSelected.Count > 0)
        {
            GameObject held = interactor.interactablesSelected[0].transform.gameObject;
            DontDestroyOnLoad(held);
        }
    }
}