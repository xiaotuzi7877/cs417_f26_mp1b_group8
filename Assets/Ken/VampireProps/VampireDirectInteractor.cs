using UnityEngine.XR.Interaction.Toolkit.Interactors;

// Joins the existing hand's interaction group so direct grabbing wins over its ray.
public class VampireDirectInteractor : XRDirectInteractor
{
    protected override void Start()
    {
        base.Start();
        var handGroup = GetComponentInParent<XRInteractionGroup>();
        if (handGroup == null) return;
        handGroup.AddGroupMember(this);
        handGroup.MoveGroupMemberTo(this, 0);
    }
}
