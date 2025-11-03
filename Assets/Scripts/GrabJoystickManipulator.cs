// GrabJoystickManipulator.cs — disable turn so Ray Anchor push/pull works while grabbing
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[DefaultExecutionOrder(50)]
public class GrabJoystickManipulator : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor;
    public MonoBehaviour turnProviderComponent;
    bool wasTurning; object xrRay; bool hasRay; bool savedAllow; bool savedManip;
    void Reset(){ if(!interactor) TryGetComponent(out interactor); }
    void OnEnable(){
        if(!interactor) TryGetComponent(out interactor);
        if(interactor){ interactor.selectEntered.AddListener(OnSel); interactor.selectExited.AddListener(OnDesel); }
        var ray = GetComponent("UnityEngine.XR.Interaction.Toolkit.XRRayInteractor") as Component;
        hasRay = ray!=null; xrRay = ray;
    }
    void OnDisable(){ if(interactor){ interactor.selectEntered.RemoveListener(OnSel); interactor.selectExited.RemoveListener(OnDesel);} }

    void OnSel(SelectEnterEventArgs _){
        if (turnProviderComponent){ wasTurning=turnProviderComponent.enabled; turnProviderComponent.enabled=false; }
        if (hasRay && xrRay!=null){
            var t=xrRay.GetType();
            var pA=t.GetProperty("allowAnchorControl"); if(pA!=null){ savedAllow=(bool)pA.GetValue(xrRay); if(!savedAllow) pA.SetValue(xrRay,true); }
            var pM=t.GetProperty("manipulateAttachTransform"); if(pM!=null){ savedManip=(bool)pM.GetValue(xrRay); if(!savedManip) pM.SetValue(xrRay,true); }
        }
    }
    void OnDesel(SelectExitEventArgs _){
        if (turnProviderComponent) turnProviderComponent.enabled=wasTurning;
        if (hasRay && xrRay!=null){
            var t=xrRay.GetType();
            var pA=t.GetProperty("allowAnchorControl"); if(pA!=null) pA.SetValue(xrRay,savedAllow);
            var pM=t.GetProperty("manipulateAttachTransform"); if(pM!=null) pM.SetValue(xrRay,savedManip);
        }
    }
}
