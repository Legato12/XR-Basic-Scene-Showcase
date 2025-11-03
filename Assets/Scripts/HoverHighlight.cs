using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable))]
public class HoverHighlight : MonoBehaviour
{
    public Color highlight = new Color(1f,0.9f,0.3f,1f);
    Color original; Material mat; UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable it;
    void Awake(){ var r = GetComponent<Renderer>(); mat = r.material; original = mat.color; it = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>(); it.hoverEntered.AddListener(_=>mat.color=highlight); it.hoverExited.AddListener(_=>mat.color=original); }
}
