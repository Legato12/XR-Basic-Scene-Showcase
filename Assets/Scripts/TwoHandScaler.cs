// TwoHandScaler.cs (patch): allow two interactors by forcing Multiple select mode
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(50)]
public class TwoHandScaler : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    public float minScale = 0.2f, maxScale = 3f;
    public InputActionProperty singleHandY;  // optional: use stick Y to scale while one-handed
    public float joystickScalePerSec = 1.0f;

    UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor a,b; float baseDist; Vector3 baseScale;

    void Reset(){ if(!grab) TryGetComponent(out grab); }
    void OnEnable(){
        if(!grab) TryGetComponent(out grab);
        if(grab){
            // key fix: let the interactable be grabbed by two controllers
            grab.selectMode = UnityEngine.XR.Interaction.Toolkit.Interactables.InteractableSelectMode.Multiple;
            grab.selectEntered.AddListener(OnSelect);
            grab.selectExited.AddListener(OnDeselect);
        }
        if(singleHandY.action!=null) singleHandY.action.Enable();
    }
    void OnDisable(){
        if(grab){ grab.selectEntered.RemoveListener(OnSelect); grab.selectExited.RemoveListener(OnDeselect); }
        if(singleHandY.action!=null) singleHandY.action.Disable();
    }

    void Update()
    {
        if (a!=null && b!=null)
        {
            float d = Vector3.Distance(a.transform.position, b.transform.position);
            if (baseDist > 1e-4f)
            {
                float f = d / baseDist;
                float s = Mathf.Clamp(baseScale.x * f, minScale, maxScale);
                transform.localScale = Vector3.one * s;
            }
        }
        else if (a!=null && singleHandY.action!=null)
        {
            float y = singleHandY.action.ReadValue<Vector2>().y;
            if (Mathf.Abs(y) > 0.01f)
            {
                float s = Mathf.Clamp(transform.localScale.x * (1f + y * joystickScalePerSec * Time.deltaTime), minScale, maxScale);
                transform.localScale = Vector3.one * s;
            }
        }
    }

    void OnSelect(SelectEnterEventArgs e)
    {
        if (a==null){ a=e.interactorObject; baseScale=transform.localScale; }
        else if (b==null){ b=e.interactorObject; baseDist=Vector3.Distance(a.transform.position, b.transform.position); baseScale=transform.localScale; }
    }
    void OnDeselect(SelectExitEventArgs e)
    {
        if (e.interactorObject==b) b=null;
        if (e.interactorObject==a) { a=b; b=null; }
        if (a==null) baseDist=0;
    }
}
