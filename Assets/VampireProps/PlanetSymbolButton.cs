using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PlanetSymbolButton : XRBaseInteractable
{
    [SerializeField] private PlanetStakePuzzle puzzle;
    [SerializeField] private PlanetSymbol symbol;
    [SerializeField] private Renderer buttonFace;
    private MaterialPropertyBlock tint;
    private bool accepted;
    public PlanetSymbol Symbol => symbol;

    protected override void Awake()
    {
        base.Awake();
        tint = new MaterialPropertyBlock();
    }

    public override bool IsHoverableBy(IXRHoverInteractor interactor)
        => puzzle != null && puzzle.CanPress && puzzle.IsRightHand(interactor) && base.IsHoverableBy(interactor);

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
        => puzzle != null && puzzle.CanPress && puzzle.IsRightHand(interactor) && base.IsSelectableBy(interactor);

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        puzzle.Press(symbol, args.interactorObject);
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args) { base.OnHoverEntered(args); Refresh(); }
    protected override void OnHoverExited(HoverExitEventArgs args) { base.OnHoverExited(args); Refresh(); }

    public void SetAccepted(bool value) { accepted = value; Refresh(); }

    private void Refresh()
    {
        if (buttonFace == null) return;
        if (!accepted && !isHovered) { buttonFace.SetPropertyBlock(null); return; }
        tint.SetColor("_BaseColor", accepted ? new Color(.2f, .9f, .4f) : new Color(1f, .85f, .2f));
        buttonFace.SetPropertyBlock(tint);
    }
}
