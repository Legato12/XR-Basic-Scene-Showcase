// AxisSlider.cs (v2, attach-based, minimal)
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[ExecuteAlways]
public class AxisSlider : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Transform knob;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    public float followLerp = 25f;
    [Range(0,1)] public float normalized;

    UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor;
    Transform attach; bool held;

    void Reset()
    {
        if (!knob) knob = transform;
        if (!startPoint || !endPoint)
        {
            var a = new GameObject("Start").transform; a.SetParent(transform); a.localPosition = new Vector3(-0.25f,0,0);
            var b = new GameObject("End").transform;   b.SetParent(transform); b.localPosition = new Vector3( 0.25f,0,0);
            startPoint = a; endPoint = b;
        }
        if (!grab && knob) grab = knob.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }
    void Awake(){ if (grab){ grab.trackPosition=false; grab.trackRotation=false; var rb=grab.GetComponent<Rigidbody>(); if (rb) rb.isKinematic=true; } }
    void OnEnable(){ if (grab){ grab.selectEntered.AddListener(OnSel); grab.selectExited.AddListener(OnDesel);} }
    void OnDisable(){ if (grab){ grab.selectEntered.RemoveListener(OnSel); grab.selectExited.RemoveListener(OnDesel);} }
    void Update()
    {
        if (held && attach != null)
        {
            Vector3 a = startPoint.position, b = endPoint.position, p = attach.position;
            Vector3 ab = b - a; float len = ab.magnitude; if (len < 1e-5f) return;
            float t = Mathf.Clamp01(Vector3.Dot(p - a, ab.normalized) / len);
            normalized = t;
            Vector3 target = Vector3.Lerp(a, b, t);
            knob.position = Vector3.Lerp(knob.position, target, Time.deltaTime * followLerp);
        }
        else if (!Application.isPlaying && startPoint && endPoint && knob)
        { knob.position = Vector3.Lerp(startPoint.position,endPoint.position,normalized); }
    }
    public bool IsNear(float target, float tol)=> Mathf.Abs(normalized-target)<=tol;
    void OnSel(SelectEnterEventArgs e){ interactor=e.interactorObject; attach=e.interactorObject.GetAttachTransform(e.interactableObject); if(!attach) attach=interactor.transform; held=true; }
    void OnDesel(SelectExitEventArgs e){ held=false; interactor=null; attach=null; }
}
