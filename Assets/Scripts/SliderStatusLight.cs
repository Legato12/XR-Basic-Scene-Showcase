// SliderStatusLight.cs (minimal)
using UnityEngine;
[ExecuteAlways]
public class SliderStatusLight : MonoBehaviour
{
    public AxisSlider slider;
    public Renderer bulbRenderer;
    public Light bulbLight;
    [Range(0,1)] public float target = 0.5f;
    [Range(0.001f, 0.5f)] public float tolerance = 0.06f;
    public Color okColor = new Color(0.2f,1f,0.3f), warnColor = new Color(1f,0.9f,0.2f);
    public bool IsCorrect { get; private set; }
    void Reset(){ if(!slider) slider=GetComponentInParent<AxisSlider>(); if(!bulbRenderer) TryGetComponent(out bulbRenderer); if(!bulbLight) TryGetComponent(out bulbLight); }
    void Update(){
        if(!slider) return;
        IsCorrect = Mathf.Abs(slider.normalized-target)<=tolerance;
        var c = IsCorrect ? okColor : warnColor;
        if (bulbRenderer && bulbRenderer.sharedMaterial){
            var m = bulbRenderer.sharedMaterial;
            if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", c); else if (m.HasProperty("_Color")) m.SetColor("_Color", c);
            if (m.HasProperty("_EmissionColor")){ m.EnableKeyword("_EMISSION"); m.SetColor("_EmissionColor", c*2f); }
        }
        if (bulbLight){ bulbLight.color=c; bulbLight.intensity=2.5f; bulbLight.range=0.6f; }
    }
}
