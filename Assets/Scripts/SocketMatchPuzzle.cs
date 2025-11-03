// SocketMatchPuzzle.cs (robust): door opens when all sockets have the expected TokenId
using UnityEngine;

using System.Linq;

public class SocketMatchPuzzle : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor[] sockets;
    public int[] expectedIds;
    public bool readFromSocketComponents = true; // if true, use SocketExpectId on each socket
    public Transform door; public float openDistance = 0.35f, openLerp = 4f;

    Vector3 doorClosed; bool wasSolved;

    void Start()
    {
        if (door) doorClosed = door.localPosition;
        // auto-fill expectedIds from components if requested
        if (readFromSocketComponents && sockets != null && sockets.Length > 0)
        {
            expectedIds = sockets.Select(s => {
                var c = s ? s.GetComponent<SocketExpectId>() : null;
                return c ? c.expectedId : 0;
            }).ToArray();
        }
    }

    void Update()
    {
        bool solved = AllCorrect();
        if (solved && !wasSolved) OnSolved();
        wasSolved = solved;

        if (door)
        {
            // Move door up by its full height (local Y scale) when solved
            float fullHeight = door.localScale.y; // how tall it is in local units
            Vector3 tgt = doorClosed + new Vector3(0, solved ? fullHeight : 0, 0);
            door.localPosition = Vector3.Lerp(door.localPosition, tgt, Time.deltaTime * openLerp);
        }
    }


    bool AllCorrect()
    {
        if (sockets == null || sockets.Length == 0) return false;
        for (int i = 0; i < sockets.Length; i++)
        {
            var s = sockets[i]; if (!s) return false;
            if (!s.hasSelection) return false;
            var inter = s.firstInteractableSelected;
            var idComp = inter != null ? inter.transform.GetComponentInParent<TokenId>() : null;
            int have = idComp ? idComp.id : int.MinValue;
            int exp = (expectedIds != null && i < expectedIds.Length) ? expectedIds[i] : 0;
            if (have != exp) return false;
        }
        return true;
    }

    void OnSolved() { /* hook SFX here if needed */ }
}
