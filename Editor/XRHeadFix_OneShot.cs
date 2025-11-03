// XRHeadFix_OneShot.cs
// Menu: XR Starter / Fix Head Tracking & Height (One‑Shot)
// - Ensures XR_Showcase is first in Build Settings, removes XR_Calibration if exists
// - Ensures XR Origin has hierarchy: XROrigin -> "Camera Offset" -> "Main Camera"
// - Adds TrackedPoseDriver (Input System) to Main Camera (Rotation+Position, Update+BeforeRender), tags it MainCamera
// - Removes TrackedPoseDriver from everything else (prevents "world sticks to head")
// - Removes XRRayInteractor from camera (fix "ray from face")
// - Adds SimpleMoveNoActions & SimpleSnapTurnNoActions to XROrigin (movement without actions)
// - Adds/updates CharacterController sane defaults (radius/height/center/skin)
// - Adds FixedHeightBootstrap (sets constant head height at runtime)
// - Saves scene and deletes itself

using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public static class XRHeadFix_OneShot
{
    const string ScenesFolder = "Assets/XRStarter/Scenes";
    const string ShowcasePath = ScenesFolder + "/XR_Showcase.unity";
    const string CalibPath    = ScenesFolder + "/XR_Calibration.unity";

    [MenuItem("XR Starter/Fix Head Tracking & Height (One‑Shot)")]
    public static void Run()
    {
        // Build: keep showcase first, drop calibration
        var list = EditorBuildSettings.scenes.ToList();
        list.RemoveAll(s => s.path == ShowcasePath || s.path == CalibPath);
        if (System.IO.File.Exists(ShowcasePath))
            list.Insert(0, new EditorBuildSettingsScene(ShowcasePath, true));
        EditorBuildSettings.scenes = list.ToArray();
        if (System.IO.File.Exists(CalibPath)) AssetDatabase.DeleteAsset(CalibPath);

        if (System.IO.File.Exists(ShowcasePath))
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            EditorSceneManager.OpenScene(ShowcasePath, OpenSceneMode.Single);
        }

        var origin = Object.FindFirstObjectByType<XROrigin>();
        if (origin == null)
        {
            EditorUtility.DisplayDialog("XR Starter", "XROrigin not found in scene. Add XR Origin (Action‑based) and rerun.", "OK");
            return;
        }

        // Ensure "Camera Offset" child
        var camOffset = origin.CameraFloorOffsetObject;
        if (camOffset == null)
        {
            var co = new GameObject("Camera Offset");
            co.transform.SetParent(origin.transform, false);
            origin.CameraFloorOffsetObject = co;
            camOffset = co;
        }

        // Ensure Main Camera child
        Camera cam = origin.Camera;
        if (cam == null)
        {
            var camGO = new GameObject("Main Camera");
            camGO.transform.SetParent(camOffset.transform, false);
            cam = camGO.AddComponent<Camera>();
            camGO.AddComponent<AudioListener>();
        }

        // Tag it MainCamera
        cam.tag = "MainCamera";

        // Remove ray interactor from camera if any
        var badRay = cam.GetComponent<XRRayInteractor>();
        if (badRay) Object.DestroyImmediate(badRay);

        // Add TrackedPoseDriver (Input System) to camera
        var tpd = cam.GetComponent<TrackedPoseDriver>();
        if (tpd == null) tpd = cam.gameObject.AddComponent<TrackedPoseDriver>();
        // defaults are fine: tracks Rotation & Position; Update & BeforeRender

        // Remove TPD from everything else
        var allTpds = Object.FindObjectsOfType<TrackedPoseDriver>(true);
        foreach (var x in allTpds)
        {
            if (x.gameObject != cam.gameObject)
                Object.DestroyImmediate(x);
        }

        // CharacterController sane defaults on origin
        var cc = origin.GetComponent<CharacterController>();
        if (cc == null) cc = origin.gameObject.AddComponent<CharacterController>();
        cc.radius = Mathf.Max(0.22f, cc.radius);
        cc.height = Mathf.Max(1.7f, cc.height);
        cc.center = new Vector3(0, cc.height * 0.5f, 0);
        cc.skinWidth = Mathf.Max(0.02f, cc.skinWidth);
        cc.stepOffset = Mathf.Max(0.25f, cc.stepOffset);
        cc.slopeLimit = 45f;

        // Movement without actions (so you can move even if input maps are not bound)
        if (!origin.GetComponent<SimpleMoveNoActions>()) origin.gameObject.AddComponent<SimpleMoveNoActions>();
        if (!origin.GetComponent<SimpleSnapTurnNoActions>()) origin.gameObject.AddComponent<SimpleSnapTurnNoActions>();

        // Fixed height runtime bootstrap
        if (Object.FindFirstObjectByType<FixedHeightBootstrap>() == null)
        {
            var go = new GameObject("XR_FixedHeightBootstrap");
            go.AddComponent<FixedHeightBootstrap>();
        }

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();

        // Self-delete
        EditorApplication.delayCall += () =>
        {
            var guid = AssetDatabase.FindAssets("XRHeadFix_OneShot t:Script").FirstOrDefault();
            if (!string.IsNullOrEmpty(guid))
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(guid));
            }
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("XR Starter", "Head tracking & height fixed. Scene saved. Script removed.", "OK");
        };
    }
}
