using UnityEngine;

public class TableLabel : MonoBehaviour
{
    [TextArea(2,5)] public string text = "Table";
    public float characterSize = 0.06f;
    TextMesh tm;

    void Awake()
    {
        tm = gameObject.AddComponent<TextMesh>();
        tm.text = text;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.alignment = TextAlignment.Center;
        tm.characterSize = characterSize;
        tm.color = Color.white;
    }

    void LateUpdate()
    {
        var cam = Camera.main;
        if (cam) transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position, Vector3.up);
    }
}
