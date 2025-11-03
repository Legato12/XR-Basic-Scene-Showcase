// SliderPuzzleOrchestrator.cs
using UnityEngine;
public class SliderPuzzleOrchestrator : MonoBehaviour
{
    public SliderStatusLight[] statuses;
    public Transform drawer;
    public float openDistance = 0.25f, openLerp = 4f;
    public Renderer lampRenderer; public Light lampLight;
    public Color okColor=new Color(0.2f,1f,0.3f), warnColor=new Color(1f,0.9f,0.2f);
    public ParticleSystem[] winBursts; public AudioSource winAudio;
    Vector3 drawerClosedLocal; bool wasAllOk;
    void Start(){ if(drawer) drawerClosedLocal=drawer.localPosition; }
    void Update(){
        if(statuses==null||statuses.Length==0) return;
        bool allOk=true; for(int i=0;i<statuses.Length;i++){ if(!(statuses[i]&&statuses[i].IsCorrect)){ allOk=false; break; } }
        if(allOk && !wasAllOk){ if(winBursts!=null) foreach(var ps in winBursts) if(ps) ps.Play(true); if(winAudio) winAudio.Play(); }
        wasAllOk=allOk;
        SetLamp(allOk?okColor:warnColor);
        if(drawer){ Vector3 tgt=drawerClosedLocal+new Vector3(0,0, allOk?openDistance:0f); drawer.localPosition=Vector3.Lerp(drawer.localPosition,tgt,Time.deltaTime*openLerp); }
    }
    void SetLamp(Color c){
        if(lampRenderer){
            var m=lampRenderer.sharedMaterial; if(m!=null){
                if(m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", c); else if(m.HasProperty("_Color")) m.SetColor("_Color", c);
                if(m.HasProperty("_EmissionColor")){ m.EnableKeyword("_EMISSION"); m.SetColor("_EmissionColor", c*2f); }
            }
        }
        if(lampLight){ lampLight.color=c; lampLight.intensity=2.5f; lampLight.range=1.0f; }
    }
}
