using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// Auto-generated script to recreate scene: Scene4
/// Generated on: 2025-08-02 12:40:16
/// NOTE: Interface fields and Action delegates may need manual setup
/// </summary>
public class Scene4Script : MonoBehaviour
{
    // GameObject references
    private GameObject BPM_0;
    private GameObject Main_Camera_1;
    private GameObject GameObject_2;
    private GameObject A_3;
    private GameObject Container_4;
    private GameObject Sphere_5;
    private GameObject A__2__6;
    private GameObject Container_7;
    private GameObject Sphere_8;
    private GameObject A__1__9;
    private GameObject Container_10;
    private GameObject Sphere_11;
    private GameObject A__3__12;
    private GameObject Container_13;
    private GameObject Sphere_14;
    private GameObject GameObject__1__15;
    private GameObject A_16;
    private GameObject Container_17;
    private GameObject Sphere_18;
    private GameObject A__2__19;
    private GameObject Container_20;
    private GameObject Sphere_21;
    private GameObject A__1__22;
    private GameObject Container_23;
    private GameObject Sphere_24;
    private GameObject A__3__25;
    private GameObject Container_26;
    private GameObject Sphere_27;

    void Start()
    {
        // Create all GameObjects
        BPM_0 = new GameObject("BPM");
        Main_Camera_1 = new GameObject("Main Camera");
        GameObject_2 = new GameObject("GameObject");
        A_3 = new GameObject("A");
        Container_4 = new GameObject("Container");
        Sphere_5 = new GameObject("Sphere");
        A__2__6 = new GameObject("A (2)");
        Container_7 = new GameObject("Container");
        Sphere_8 = new GameObject("Sphere");
        A__1__9 = new GameObject("A (1)");
        Container_10 = new GameObject("Container");
        Sphere_11 = new GameObject("Sphere");
        A__3__12 = new GameObject("A (3)");
        Container_13 = new GameObject("Container");
        Sphere_14 = new GameObject("Sphere");
        GameObject__1__15 = new GameObject("GameObject (1)");
        A_16 = new GameObject("A");
        Container_17 = new GameObject("Container");
        Sphere_18 = new GameObject("Sphere");
        A__2__19 = new GameObject("A (2)");
        Container_20 = new GameObject("Container");
        Sphere_21 = new GameObject("Sphere");
        A__1__22 = new GameObject("A (1)");
        Container_23 = new GameObject("Container");
        Sphere_24 = new GameObject("Sphere");
        A__3__25 = new GameObject("A (3)");
        Container_26 = new GameObject("Container");
        Sphere_27 = new GameObject("Sphere");

        // Set up hierarchy
        Main_Camera_1.transform.SetParent(BPM_0.transform);
        GameObject_2.transform.SetParent(BPM_0.transform);
        A_3.transform.SetParent(GameObject_2.transform);
        Container_4.transform.SetParent(A_3.transform);
        Sphere_5.transform.SetParent(Container_4.transform);
        A__2__6.transform.SetParent(GameObject_2.transform);
        Container_7.transform.SetParent(A__2__6.transform);
        Sphere_8.transform.SetParent(Container_7.transform);
        A__1__9.transform.SetParent(GameObject_2.transform);
        Container_10.transform.SetParent(A__1__9.transform);
        Sphere_11.transform.SetParent(Container_10.transform);
        A__3__12.transform.SetParent(GameObject_2.transform);
        Container_13.transform.SetParent(A__3__12.transform);
        Sphere_14.transform.SetParent(Container_13.transform);
        GameObject__1__15.transform.SetParent(BPM_0.transform);
        A_16.transform.SetParent(GameObject__1__15.transform);
        Container_17.transform.SetParent(A_16.transform);
        Sphere_18.transform.SetParent(Container_17.transform);
        A__2__19.transform.SetParent(GameObject__1__15.transform);
        Container_20.transform.SetParent(A__2__19.transform);
        Sphere_21.transform.SetParent(Container_20.transform);
        A__1__22.transform.SetParent(GameObject__1__15.transform);
        Container_23.transform.SetParent(A__1__22.transform);
        Sphere_24.transform.SetParent(Container_23.transform);
        A__3__25.transform.SetParent(GameObject__1__15.transform);
        Container_26.transform.SetParent(A__3__25.transform);
        Sphere_27.transform.SetParent(Container_26.transform);

        // Set transforms
        GameObject_2.transform.localPosition = new Vector3(0f, 0f, 900f);
        Sphere_5.transform.localScale = new Vector3(3000f, 1f, 1f);
        Container_7.transform.localRotation = new Quaternion(0f, 0f, 0.7071068f, 0.7071068f);
        Sphere_8.transform.localScale = new Vector3(3000f, 1f, 1f);
        Sphere_11.transform.localScale = new Vector3(3000f, 1f, 1f);
        Container_13.transform.localRotation = new Quaternion(0f, 0f, 0.7071068f, 0.7071068f);
        Sphere_14.transform.localScale = new Vector3(3000f, 1f, 1f);
        GameObject__1__15.transform.localPosition = new Vector3(0f, 0f, 900f);
        GameObject__1__15.transform.localRotation = new Quaternion(0f, 0.130526f, 0f, 0.9914449f);
        Sphere_18.transform.localScale = new Vector3(3000f, 1f, 1f);
        Container_20.transform.localRotation = new Quaternion(0f, 0f, 0.7071068f, 0.7071068f);
        Sphere_21.transform.localScale = new Vector3(3000f, 1f, 1f);
        Sphere_24.transform.localScale = new Vector3(3000f, 1f, 1f);
        Container_26.transform.localRotation = new Quaternion(0f, 0f, 0.7071068f, 0.7071068f);
        Sphere_27.transform.localScale = new Vector3(3000f, 1f, 1f);

        // Add and configure components
        var BPM_0_BPMTime = BPM_0.AddComponent<BPMTime>();
        BPM_0_BPMTime.bpm = 52f;

        var Main_Camera_1_Camera = Main_Camera_1.AddComponent<Camera>();
        Main_Camera_1_Camera.ClearFlags = (Enum)1;
        Main_Camera_1_Camera.BackGroundColor = Color.black;
        Main_Camera_1_Camera.ProjectionMatrixMode = 1;
        Main_Camera_1_Camera.GateFitMode = 2;
        Main_Camera_1_Camera.Iso = 200;
        Main_Camera_1_Camera.ShutterSpeed = 0.005f;
        Main_Camera_1_Camera.Aperture = 16f;
        Main_Camera_1_Camera.FocusDistance = 10f;
        Main_Camera_1_Camera.FocalLength = 50f;
        Main_Camera_1_Camera.BladeCount = 5;
        Main_Camera_1_Camera.Curvature = new Vector2(2f, 11f);
        Main_Camera_1_Camera.BarrelClipping = 0.25f;
        Main_Camera_1_Camera.SensorSize = new Vector2(36f, 24f);
        Main_Camera_1_Camera.near clip plane = 0.3f;
        Main_Camera_1_Camera.far clip plane = 2000f;
        Main_Camera_1_Camera.field of view = 60f;
        Main_Camera_1_Camera.orthographic size = 932.83f;
        Main_Camera_1_Camera.Depth = -1f;
        Main_Camera_1_Camera.RenderingPath = (Enum)0;
        Main_Camera_1_Camera.TargetEye = 3;
        Main_Camera_1_Camera.HDR = true;
        Main_Camera_1_Camera.AllowMSAA = true;
        Main_Camera_1_Camera.OcclusionCulling = true;
        Main_Camera_1_Camera.StereoConvergence = 10f;
        Main_Camera_1_Camera.StereoSeparation = 0.022f;

        var Main_Camera_1_AudioListener = Main_Camera_1.AddComponent<AudioListener>();

        var Main_Camera_1_UniversalAdditionalCameraData = Main_Camera_1.AddComponent<UniversalAdditionalCameraData>();
        Main_Camera_1_UniversalAdditionalCameraData.RenderShadows = true;
        Main_Camera_1_UniversalAdditionalCameraData.RequiresDepthTextureOption = (Enum)2;
        Main_Camera_1_UniversalAdditionalCameraData.RequiresOpaqueTextureOption = (Enum)2;
        Main_Camera_1_UniversalAdditionalCameraData.CameraType = (Enum)0;
        // Skipped generic property: Cameras (type: vector)
        Main_Camera_1_UniversalAdditionalCameraData.RendererIndex = -1;
        Main_Camera_1_UniversalAdditionalCameraData.VolumeFrameworkUpdateModeOption = (Enum)2;
        Main_Camera_1_UniversalAdditionalCameraData.Antialiasing = (Enum)0;
        Main_Camera_1_UniversalAdditionalCameraData.AntialiasingQuality = (Enum)2;
        Main_Camera_1_UniversalAdditionalCameraData.ClearDepth = true;
        Main_Camera_1_UniversalAdditionalCameraData.AllowXRRendering = true;
        Main_Camera_1_UniversalAdditionalCameraData.AllowHDROutput = true;
        // Skipped generic property: TaaSettings (type: Settings)

        var Main_Camera_1_LFOFloatController = Main_Camera_1.AddComponent<LFOFloatController>();
        Main_Camera_1_LFOFloatController.useExternalBPMTime = true;
        Main_Camera_1_LFOFloatController.bpm = 120f;
        Main_Camera_1_LFOFloatController.beatsPerCycle = 8f;
        Main_Camera_1_LFOFloatController.waveform = (Enum)0;
        Main_Camera_1_LFOFloatController.loop = true;
        Main_Camera_1_LFOFloatController.min = 60f;
        Main_Camera_1_LFOFloatController.max = 120f;
        // Skipped generic property: action (type: LFOFloatEvent)

        var Main_Camera_1_LFOColorController = Main_Camera_1.AddComponent<LFOColorController>();
        Main_Camera_1_LFOColorController.useExternalBPMTime = true;
        Main_Camera_1_LFOColorController.bpm = 120f;
        Main_Camera_1_LFOColorController.beatsPerCycle = 1f;
        Main_Camera_1_LFOColorController.waveform = (Enum)0;
        Main_Camera_1_LFOColorController.loop = true;
        Main_Camera_1_LFOColorController.min = Color.black;
        Main_Camera_1_LFOColorController.max = new Color(0f, 0.3411765f, 1f, 1f);
        // Skipped generic property: action (type: LFOColorEvent)

        var A_3_LFOVector3Controller = A_3.AddComponent<LFOVector3Controller>();
        A_3_LFOVector3Controller.useExternalBPMTime = true;
        A_3_LFOVector3Controller.bpm = 120f;
        A_3_LFOVector3Controller.beatsPerCycle = 4f;
        A_3_LFOVector3Controller.waveform = (Enum)0;
        A_3_LFOVector3Controller.loop = true;
        A_3_LFOVector3Controller.min = new Vector3(0f, -1000f, 0f);
        // Skipped generic property: action (type: LFOVector3Event)

        var A_3_CopyAndTransform2 = A_3.AddComponent<CopyAndTransform2>();
        A_3_CopyAndTransform2.numCopies = 30;
        // Skipped generic property: originalGameObjects (type: vector)
        A_3_CopyAndTransform2.deltaPosition = new Vector3(0f, 100f, 0f);
        A_3_CopyAndTransform2.deltaScale = new Vector3(1f, 1f, 1f);
        A_3_CopyAndTransform2.colorMode = (Enum)0;
        // Skipped generic property: colors (type: vector)
        A_3_CopyAndTransform2.minColor = Color.black;

        var A_3_CycleInBeatsController = A_3.AddComponent<CycleInBeatsController>();
        A_3_CycleInBeatsController.useExternalBPMTime = true;
        A_3_CycleInBeatsController.bpm = 200f;
        A_3_CycleInBeatsController.beatsPerCycle = 1.401298E-45f;

        var A_3_OnNewCycleController = A_3.AddComponent<OnNewCycleController>();
        A_3_OnNewCycleController.cycleController = A_3.GetComponent<CycleInBeatsController>();
        // Skipped generic property: action (type: NewCycleEvent)

        var Sphere_5_MeshFilter = Sphere_5.AddComponent<MeshFilter>();
        Sphere_5_MeshFilter.Mesh = AssetDatabase.LoadAssetAtPath<Mesh>("Library/unity default resources");

        var Sphere_5_MeshRenderer = Sphere_5.AddComponent<MeshRenderer>();
        Sphere_5_MeshRenderer.CastShadows = (Enum)1;
        Sphere_5_MeshRenderer.ReceiveShadows = true;
        Sphere_5_MeshRenderer.DynamicOccludee = true;
        Sphere_5_MeshRenderer.MotionVectors = (Enum)1;
        Sphere_5_MeshRenderer.LightProbeUsage = 1;
        Sphere_5_MeshRenderer.ReflectionProbeUsage = 1;
        Sphere_5_MeshRenderer.RayTracingMode = 2;
        Sphere_5_MeshRenderer.RayTracingAccelStructBuildFlags = 1;
        Sphere_5_MeshRenderer.SmallMeshCulling = true;
        Sphere_5_MeshRenderer.RenderingLayerMask = 1;
        // Skipped generic property: Materials (type: vector)

        var Sphere_5_SphereCollider = Sphere_5.AddComponent<SphereCollider>();
        Sphere_5_SphereCollider.Radius = 0.5f;

        var Sphere_5_LFOVector3Controller = Sphere_5.AddComponent<LFOVector3Controller>();
        Sphere_5_LFOVector3Controller.useExternalBPMTime = true;
        Sphere_5_LFOVector3Controller.bpm = 120f;
        Sphere_5_LFOVector3Controller.beatsPerCycle = 2.86f;
        Sphere_5_LFOVector3Controller.waveform = (Enum)0;
        Sphere_5_LFOVector3Controller.phaseOffset = 0.64f;
        Sphere_5_LFOVector3Controller.loop = true;
        Sphere_5_LFOVector3Controller.min = new Vector3(3000f, 1f, 1f);
        Sphere_5_LFOVector3Controller.max = new Vector3(3000f, 30f, 30f);
        // Skipped generic property: action (type: LFOVector3Event)

        var A__2__6_LFOVector3Controller = A__2__6.AddComponent<LFOVector3Controller>();
        A__2__6_LFOVector3Controller.useExternalBPMTime = true;
        A__2__6_LFOVector3Controller.bpm = 120f;
        A__2__6_LFOVector3Controller.beatsPerCycle = 5f;
        A__2__6_LFOVector3Controller.waveform = (Enum)0;
        A__2__6_LFOVector3Controller.loop = true;
        A__2__6_LFOVector3Controller.min = new Vector3(-1000f, 0f, 0f);
        // Skipped generic property: action (type: LFOVector3Event)

        var A__2__6_CopyAndTransform2 = A__2__6.AddComponent<CopyAndTransform2>();
        A__2__6_CopyAndTransform2.numCopies = 30;
        // Skipped generic property: originalGameObjects (type: vector)
        A__2__6_CopyAndTransform2.deltaPosition = new Vector3(100f, 0f, 0f);
        A__2__6_CopyAndTransform2.deltaScale = new Vector3(1f, 1f, 1f);
        A__2__6_CopyAndTransform2.colorMode = (Enum)0;
        // Skipped generic property: colors (type: vector)
        A__2__6_CopyAndTransform2.minColor = Color.black;

        var A__2__6_CycleInBeatsController = A__2__6.AddComponent<CycleInBeatsController>();
        A__2__6_CycleInBeatsController.useExternalBPMTime = true;
        A__2__6_CycleInBeatsController.bpm = 200f;
        A__2__6_CycleInBeatsController.beatsPerCycle = 1.401298E-45f;

        var A__2__6_OnNewCycleController = A__2__6.AddComponent<OnNewCycleController>();
        A__2__6_OnNewCycleController.cycleController = A__2__6.GetComponent<CycleInBeatsController>();
        // Skipped generic property: action (type: NewCycleEvent)

        var Sphere_8_MeshFilter = Sphere_8.AddComponent<MeshFilter>();
        Sphere_8_MeshFilter.Mesh = AssetDatabase.LoadAssetAtPath<Mesh>("Library/unity default resources");

        var Sphere_8_MeshRenderer = Sphere_8.AddComponent<MeshRenderer>();
        Sphere_8_MeshRenderer.CastShadows = (Enum)1;
        Sphere_8_MeshRenderer.ReceiveShadows = true;
        Sphere_8_MeshRenderer.DynamicOccludee = true;
        Sphere_8_MeshRenderer.MotionVectors = (Enum)1;
        Sphere_8_MeshRenderer.LightProbeUsage = 1;
        Sphere_8_MeshRenderer.ReflectionProbeUsage = 1;
        Sphere_8_MeshRenderer.RayTracingMode = 2;
        Sphere_8_MeshRenderer.RayTracingAccelStructBuildFlags = 1;
        Sphere_8_MeshRenderer.SmallMeshCulling = true;
        Sphere_8_MeshRenderer.RenderingLayerMask = 1;
        // Skipped generic property: Materials (type: vector)

        var Sphere_8_SphereCollider = Sphere_8.AddComponent<SphereCollider>();
        Sphere_8_SphereCollider.Radius = 0.5f;

        var Sphere_8_LFOVector3Controller = Sphere_8.AddComponent<LFOVector3Controller>();
        Sphere_8_LFOVector3Controller.useExternalBPMTime = true;
        Sphere_8_LFOVector3Controller.bpm = 120f;
        Sphere_8_LFOVector3Controller.beatsPerCycle = 1f;
        Sphere_8_LFOVector3Controller.waveform = (Enum)0;
        Sphere_8_LFOVector3Controller.phaseOffset = 4.13f;
        Sphere_8_LFOVector3Controller.loop = true;
        Sphere_8_LFOVector3Controller.min = new Vector3(3000f, 1f, 1f);
        Sphere_8_LFOVector3Controller.max = new Vector3(3000f, 30f, 30f);
        // Skipped generic property: action (type: LFOVector3Event)

        var A__1__9_LFOVector3Controller = A__1__9.AddComponent<LFOVector3Controller>();
        A__1__9_LFOVector3Controller.useExternalBPMTime = true;
        A__1__9_LFOVector3Controller.bpm = 120f;
        A__1__9_LFOVector3Controller.beatsPerCycle = 3f;
        A__1__9_LFOVector3Controller.waveform = (Enum)0;
        A__1__9_LFOVector3Controller.loop = true;
        A__1__9_LFOVector3Controller.min = new Vector3(0f, 1000f, 0f);
        // Skipped generic property: action (type: LFOVector3Event)

        var A__1__9_CopyAndTransform2 = A__1__9.AddComponent<CopyAndTransform2>();
        A__1__9_CopyAndTransform2.numCopies = 30;
        // Skipped generic property: originalGameObjects (type: vector)
        A__1__9_CopyAndTransform2.deltaPosition = new Vector3(0f, -100f, 0f);
        A__1__9_CopyAndTransform2.deltaScale = new Vector3(1f, 1f, 1f);
        A__1__9_CopyAndTransform2.colorMode = (Enum)0;
        // Skipped generic property: colors (type: vector)
        A__1__9_CopyAndTransform2.minColor = Color.black;

        var A__1__9_CycleInBeatsController = A__1__9.AddComponent<CycleInBeatsController>();
        A__1__9_CycleInBeatsController.useExternalBPMTime = true;
        A__1__9_CycleInBeatsController.bpm = 200f;
        A__1__9_CycleInBeatsController.beatsPerCycle = 1.401298E-45f;

        var A__1__9_OnNewCycleController = A__1__9.AddComponent<OnNewCycleController>();
        A__1__9_OnNewCycleController.cycleController = A__1__9.GetComponent<CycleInBeatsController>();
        // Skipped generic property: action (type: NewCycleEvent)

        var Sphere_11_MeshFilter = Sphere_11.AddComponent<MeshFilter>();
        Sphere_11_MeshFilter.Mesh = AssetDatabase.LoadAssetAtPath<Mesh>("Library/unity default resources");

        var Sphere_11_MeshRenderer = Sphere_11.AddComponent<MeshRenderer>();
        Sphere_11_MeshRenderer.CastShadows = (Enum)1;
        Sphere_11_MeshRenderer.ReceiveShadows = true;
        Sphere_11_MeshRenderer.DynamicOccludee = true;
        Sphere_11_MeshRenderer.MotionVectors = (Enum)1;
        Sphere_11_MeshRenderer.LightProbeUsage = 1;
        Sphere_11_MeshRenderer.ReflectionProbeUsage = 1;
        Sphere_11_MeshRenderer.RayTracingMode = 2;
        Sphere_11_MeshRenderer.RayTracingAccelStructBuildFlags = 1;
        Sphere_11_MeshRenderer.SmallMeshCulling = true;
        Sphere_11_MeshRenderer.RenderingLayerMask = 1;
        // Skipped generic property: Materials (type: vector)

        var Sphere_11_SphereCollider = Sphere_11.AddComponent<SphereCollider>();
        Sphere_11_SphereCollider.Radius = 0.5f;

        var Sphere_11_LFOVector3Controller = Sphere_11.AddComponent<LFOVector3Controller>();
        Sphere_11_LFOVector3Controller.useExternalBPMTime = true;
        Sphere_11_LFOVector3Controller.bpm = 120f;
        Sphere_11_LFOVector3Controller.beatsPerCycle = 1.6f;
        Sphere_11_LFOVector3Controller.waveform = (Enum)0;
        Sphere_11_LFOVector3Controller.loop = true;
        Sphere_11_LFOVector3Controller.min = new Vector3(3000f, 1f, 1f);
        Sphere_11_LFOVector3Controller.max = new Vector3(3000f, 30f, 30f);
        // Skipped generic property: action (type: LFOVector3Event)

        var A__3__12_LFOVector3Controller = A__3__12.AddComponent<LFOVector3Controller>();
        A__3__12_LFOVector3Controller.useExternalBPMTime = true;
        A__3__12_LFOVector3Controller.bpm = 120f;
        A__3__12_LFOVector3Controller.beatsPerCycle = 2f;
        A__3__12_LFOVector3Controller.waveform = (Enum)0;
        A__3__12_LFOVector3Controller.loop = true;
        A__3__12_LFOVector3Controller.min = new Vector3(1000f, 0f, 0f);
        // Skipped generic property: action (type: LFOVector3Event)

        var A__3__12_CopyAndTransform2 = A__3__12.AddComponent<CopyAndTransform2>();
        A__3__12_CopyAndTransform2.numCopies = 30;
        // Skipped generic property: originalGameObjects (type: vector)
        A__3__12_CopyAndTransform2.deltaPosition = new Vector3(-100f, 0f, 0f);
        A__3__12_CopyAndTransform2.deltaScale = new Vector3(1f, 1f, 1f);
        A__3__12_CopyAndTransform2.colorMode = (Enum)0;
        // Skipped generic property: colors (type: vector)
        A__3__12_CopyAndTransform2.minColor = Color.black;

        var A__3__12_CycleInBeatsController = A__3__12.AddComponent<CycleInBeatsController>();
        A__3__12_CycleInBeatsController.useExternalBPMTime = true;
        A__3__12_CycleInBeatsController.bpm = 200f;
        A__3__12_CycleInBeatsController.beatsPerCycle = 1.401298E-45f;

        var A__3__12_OnNewCycleController = A__3__12.AddComponent<OnNewCycleController>();
        A__3__12_OnNewCycleController.cycleController = A__3__12.GetComponent<CycleInBeatsController>();
        // Skipped generic property: action (type: NewCycleEvent)

        var Sphere_14_MeshFilter = Sphere_14.AddComponent<MeshFilter>();
        Sphere_14_MeshFilter.Mesh = AssetDatabase.LoadAssetAtPath<Mesh>("Library/unity default resources");

        var Sphere_14_MeshRenderer = Sphere_14.AddComponent<MeshRenderer>();
        Sphere_14_MeshRenderer.CastShadows = (Enum)1;
        Sphere_14_MeshRenderer.ReceiveShadows = true;
        Sphere_14_MeshRenderer.DynamicOccludee = true;
        Sphere_14_MeshRenderer.MotionVectors = (Enum)1;
        Sphere_14_MeshRenderer.LightProbeUsage = 1;
        Sphere_14_MeshRenderer.ReflectionProbeUsage = 1;
        Sphere_14_MeshRenderer.RayTracingMode = 2;
        Sphere_14_MeshRenderer.RayTracingAccelStructBuildFlags = 1;
        Sphere_14_MeshRenderer.SmallMeshCulling = true;
        Sphere_14_MeshRenderer.RenderingLayerMask = 1;
        // Skipped generic property: Materials (type: vector)

        var Sphere_14_SphereCollider = Sphere_14.AddComponent<SphereCollider>();
        Sphere_14_SphereCollider.Radius = 0.5f;

        var Sphere_14_LFOVector3Controller = Sphere_14.AddComponent<LFOVector3Controller>();
        Sphere_14_LFOVector3Controller.useExternalBPMTime = true;
        Sphere_14_LFOVector3Controller.bpm = 120f;
        Sphere_14_LFOVector3Controller.beatsPerCycle = 1f;
        Sphere_14_LFOVector3Controller.waveform = (Enum)0;
        Sphere_14_LFOVector3Controller.phaseOffset = 0.64f;
        Sphere_14_LFOVector3Controller.loop = true;
        Sphere_14_LFOVector3Controller.min = new Vector3(3000f, 1f, 1f);
        Sphere_14_LFOVector3Controller.max = new Vector3(3000f, 30f, 30f);
        // Skipped generic property: action (type: LFOVector3Event)

        var GameObject__1__15_LFOVector3Controller = GameObject__1__15.AddComponent<LFOVector3Controller>();
        GameObject__1__15_LFOVector3Controller.useExternalBPMTime = true;
        GameObject__1__15_LFOVector3Controller.bpm = 120f;
        GameObject__1__15_LFOVector3Controller.beatsPerCycle = 4f;
        GameObject__1__15_LFOVector3Controller.waveform = (Enum)0;
        GameObject__1__15_LFOVector3Controller.loop = true;
        GameObject__1__15_LFOVector3Controller.min = new Vector3(0f, 0f, 800f);
        GameObject__1__15_LFOVector3Controller.max = new Vector3(0f, 0f, 1000f);
        // Skipped generic property: action (type: LFOVector3Event)

        var GameObject__1__15_LFOVector3Controller = GameObject__1__15.AddComponent<LFOVector3Controller>();
        GameObject__1__15_LFOVector3Controller.useExternalBPMTime = true;
        GameObject__1__15_LFOVector3Controller.bpm = 120f;
        GameObject__1__15_LFOVector3Controller.beatsPerCycle = 5f;
        GameObject__1__15_LFOVector3Controller.waveform = (Enum)3;
        GameObject__1__15_LFOVector3Controller.loop = true;
        GameObject__1__15_LFOVector3Controller.min = new Vector3(0f, -180f, 0f);
        GameObject__1__15_LFOVector3Controller.max = new Vector3(0f, 180f, 0f);
        // Skipped generic property: action (type: LFOVector3Event)

        var A_16_LFOVector3Controller = A_16.AddComponent<LFOVector3Controller>();
        A_16_LFOVector3Controller.useExternalBPMTime = true;
        A_16_LFOVector3Controller.bpm = 120f;
        A_16_LFOVector3Controller.beatsPerCycle = 4f;
        A_16_LFOVector3Controller.waveform = (Enum)0;
        A_16_LFOVector3Controller.phaseOffset = 3.16f;
        A_16_LFOVector3Controller.loop = true;
        A_16_LFOVector3Controller.min = new Vector3(0f, -1000f, 0f);
        // Skipped generic property: action (type: LFOVector3Event)

        var A_16_CopyAndTransform2 = A_16.AddComponent<CopyAndTransform2>();
        A_16_CopyAndTransform2.numCopies = 30;
        // Skipped generic property: originalGameObjects (type: vector)
        A_16_CopyAndTransform2.deltaPosition = new Vector3(0f, 200f, 0f);
        A_16_CopyAndTransform2.deltaScale = new Vector3(1f, 1f, 1f);
        A_16_CopyAndTransform2.colorMode = (Enum)0;
        // Skipped generic property: colors (type: vector)
        A_16_CopyAndTransform2.minColor = Color.black;

        var A_16_CycleInBeatsController = A_16.AddComponent<CycleInBeatsController>();
        A_16_CycleInBeatsController.useExternalBPMTime = true;
        A_16_CycleInBeatsController.bpm = 200f;
        A_16_CycleInBeatsController.beatsPerCycle = 1.401298E-45f;

        var A_16_OnNewCycleController = A_16.AddComponent<OnNewCycleController>();
        A_16_OnNewCycleController.cycleController = A_16.GetComponent<CycleInBeatsController>();
        // Skipped generic property: action (type: NewCycleEvent)

        var Sphere_18_MeshFilter = Sphere_18.AddComponent<MeshFilter>();
        Sphere_18_MeshFilter.Mesh = AssetDatabase.LoadAssetAtPath<Mesh>("Library/unity default resources");

        var Sphere_18_MeshRenderer = Sphere_18.AddComponent<MeshRenderer>();
        Sphere_18_MeshRenderer.CastShadows = (Enum)1;
        Sphere_18_MeshRenderer.ReceiveShadows = true;
        Sphere_18_MeshRenderer.DynamicOccludee = true;
        Sphere_18_MeshRenderer.MotionVectors = (Enum)1;
        Sphere_18_MeshRenderer.LightProbeUsage = 1;
        Sphere_18_MeshRenderer.ReflectionProbeUsage = 1;
        Sphere_18_MeshRenderer.RayTracingMode = 2;
        Sphere_18_MeshRenderer.RayTracingAccelStructBuildFlags = 1;
        Sphere_18_MeshRenderer.SmallMeshCulling = true;
        Sphere_18_MeshRenderer.RenderingLayerMask = 1;
        // Skipped generic property: Materials (type: vector)

        var Sphere_18_SphereCollider = Sphere_18.AddComponent<SphereCollider>();
        Sphere_18_SphereCollider.Radius = 0.5f;

        var Sphere_18_LFOVector3Controller = Sphere_18.AddComponent<LFOVector3Controller>();
        Sphere_18_LFOVector3Controller.useExternalBPMTime = true;
        Sphere_18_LFOVector3Controller.bpm = 120f;
        Sphere_18_LFOVector3Controller.beatsPerCycle = 1f;
        Sphere_18_LFOVector3Controller.waveform = (Enum)1;
        Sphere_18_LFOVector3Controller.loop = true;
        Sphere_18_LFOVector3Controller.min = new Vector3(3000f, 1f, 1f);
        Sphere_18_LFOVector3Controller.max = new Vector3(3000f, 30f, 30f);
        // Skipped generic property: action (type: LFOVector3Event)

        var A__2__19_LFOVector3Controller = A__2__19.AddComponent<LFOVector3Controller>();
        A__2__19_LFOVector3Controller.useExternalBPMTime = true;
        A__2__19_LFOVector3Controller.bpm = 120f;
        A__2__19_LFOVector3Controller.beatsPerCycle = 5f;
        A__2__19_LFOVector3Controller.waveform = (Enum)0;
        A__2__19_LFOVector3Controller.phaseOffset = 3.16f;
        A__2__19_LFOVector3Controller.loop = true;
        A__2__19_LFOVector3Controller.min = new Vector3(-1000f, 0f, 0f);
        // Skipped generic property: action (type: LFOVector3Event)

        var A__2__19_CopyAndTransform2 = A__2__19.AddComponent<CopyAndTransform2>();
        A__2__19_CopyAndTransform2.numCopies = 30;
        // Skipped generic property: originalGameObjects (type: vector)
        A__2__19_CopyAndTransform2.deltaPosition = new Vector3(200f, 0f, 0f);
        A__2__19_CopyAndTransform2.deltaScale = new Vector3(1f, 1f, 1f);
        A__2__19_CopyAndTransform2.colorMode = (Enum)0;
        // Skipped generic property: colors (type: vector)
        A__2__19_CopyAndTransform2.minColor = Color.black;

        var A__2__19_CycleInBeatsController = A__2__19.AddComponent<CycleInBeatsController>();
        A__2__19_CycleInBeatsController.useExternalBPMTime = true;
        A__2__19_CycleInBeatsController.bpm = 200f;
        A__2__19_CycleInBeatsController.beatsPerCycle = 1.401298E-45f;

        var A__2__19_OnNewCycleController = A__2__19.AddComponent<OnNewCycleController>();
        A__2__19_OnNewCycleController.cycleController = A__2__19.GetComponent<CycleInBeatsController>();
        // Skipped generic property: action (type: NewCycleEvent)

        var Sphere_21_MeshFilter = Sphere_21.AddComponent<MeshFilter>();
        Sphere_21_MeshFilter.Mesh = AssetDatabase.LoadAssetAtPath<Mesh>("Library/unity default resources");

        var Sphere_21_MeshRenderer = Sphere_21.AddComponent<MeshRenderer>();
        Sphere_21_MeshRenderer.CastShadows = (Enum)1;
        Sphere_21_MeshRenderer.ReceiveShadows = true;
        Sphere_21_MeshRenderer.DynamicOccludee = true;
        Sphere_21_MeshRenderer.MotionVectors = (Enum)1;
        Sphere_21_MeshRenderer.LightProbeUsage = 1;
        Sphere_21_MeshRenderer.ReflectionProbeUsage = 1;
        Sphere_21_MeshRenderer.RayTracingMode = 2;
        Sphere_21_MeshRenderer.RayTracingAccelStructBuildFlags = 1;
        Sphere_21_MeshRenderer.SmallMeshCulling = true;
        Sphere_21_MeshRenderer.RenderingLayerMask = 1;
        // Skipped generic property: Materials (type: vector)

        var Sphere_21_SphereCollider = Sphere_21.AddComponent<SphereCollider>();
        Sphere_21_SphereCollider.Radius = 0.5f;

        var Sphere_21_LFOVector3Controller = Sphere_21.AddComponent<LFOVector3Controller>();
        Sphere_21_LFOVector3Controller.useExternalBPMTime = true;
        Sphere_21_LFOVector3Controller.bpm = 120f;
        Sphere_21_LFOVector3Controller.beatsPerCycle = 1.6f;
        Sphere_21_LFOVector3Controller.waveform = (Enum)0;
        Sphere_21_LFOVector3Controller.phaseOffset = 0.64f;
        Sphere_21_LFOVector3Controller.loop = true;
        Sphere_21_LFOVector3Controller.min = new Vector3(3000f, 1f, 1f);
        Sphere_21_LFOVector3Controller.max = new Vector3(3000f, 30f, 30f);
        // Skipped generic property: action (type: LFOVector3Event)

        var A__1__22_LFOVector3Controller = A__1__22.AddComponent<LFOVector3Controller>();
        A__1__22_LFOVector3Controller.useExternalBPMTime = true;
        A__1__22_LFOVector3Controller.bpm = 120f;
        A__1__22_LFOVector3Controller.beatsPerCycle = 3f;
        A__1__22_LFOVector3Controller.waveform = (Enum)0;
        A__1__22_LFOVector3Controller.phaseOffset = 3.16f;
        A__1__22_LFOVector3Controller.loop = true;
        A__1__22_LFOVector3Controller.min = new Vector3(0f, 1000f, 0f);
        // Skipped generic property: action (type: LFOVector3Event)

        var A__1__22_CopyAndTransform2 = A__1__22.AddComponent<CopyAndTransform2>();
        A__1__22_CopyAndTransform2.numCopies = 30;
        // Skipped generic property: originalGameObjects (type: vector)
        A__1__22_CopyAndTransform2.deltaPosition = new Vector3(0f, -200f, 0f);
        A__1__22_CopyAndTransform2.deltaScale = new Vector3(1f, 1f, 1f);
        A__1__22_CopyAndTransform2.colorMode = (Enum)0;
        // Skipped generic property: colors (type: vector)
        A__1__22_CopyAndTransform2.minColor = Color.black;

        var A__1__22_CycleInBeatsController = A__1__22.AddComponent<CycleInBeatsController>();
        A__1__22_CycleInBeatsController.useExternalBPMTime = true;
        A__1__22_CycleInBeatsController.bpm = 200f;
        A__1__22_CycleInBeatsController.beatsPerCycle = 1.401298E-45f;

        var A__1__22_OnNewCycleController = A__1__22.AddComponent<OnNewCycleController>();
        A__1__22_OnNewCycleController.cycleController = A__1__22.GetComponent<CycleInBeatsController>();
        // Skipped generic property: action (type: NewCycleEvent)

        var Sphere_24_MeshFilter = Sphere_24.AddComponent<MeshFilter>();
        Sphere_24_MeshFilter.Mesh = AssetDatabase.LoadAssetAtPath<Mesh>("Library/unity default resources");

        var Sphere_24_MeshRenderer = Sphere_24.AddComponent<MeshRenderer>();
        Sphere_24_MeshRenderer.CastShadows = (Enum)1;
        Sphere_24_MeshRenderer.ReceiveShadows = true;
        Sphere_24_MeshRenderer.DynamicOccludee = true;
        Sphere_24_MeshRenderer.MotionVectors = (Enum)1;
        Sphere_24_MeshRenderer.LightProbeUsage = 1;
        Sphere_24_MeshRenderer.ReflectionProbeUsage = 1;
        Sphere_24_MeshRenderer.RayTracingMode = 2;
        Sphere_24_MeshRenderer.RayTracingAccelStructBuildFlags = 1;
        Sphere_24_MeshRenderer.SmallMeshCulling = true;
        Sphere_24_MeshRenderer.RenderingLayerMask = 1;
        // Skipped generic property: Materials (type: vector)

        var Sphere_24_SphereCollider = Sphere_24.AddComponent<SphereCollider>();
        Sphere_24_SphereCollider.Radius = 0.5f;

        var Sphere_24_LFOVector3Controller = Sphere_24.AddComponent<LFOVector3Controller>();
        Sphere_24_LFOVector3Controller.useExternalBPMTime = true;
        Sphere_24_LFOVector3Controller.bpm = 120f;
        Sphere_24_LFOVector3Controller.beatsPerCycle = 0.76f;
        Sphere_24_LFOVector3Controller.waveform = (Enum)0;
        Sphere_24_LFOVector3Controller.phaseOffset = 3.86f;
        Sphere_24_LFOVector3Controller.loop = true;
        Sphere_24_LFOVector3Controller.min = new Vector3(3000f, 1f, 1f);
        Sphere_24_LFOVector3Controller.max = new Vector3(3000f, 30f, 30f);
        // Skipped generic property: action (type: LFOVector3Event)

        var A__3__25_LFOVector3Controller = A__3__25.AddComponent<LFOVector3Controller>();
        A__3__25_LFOVector3Controller.useExternalBPMTime = true;
        A__3__25_LFOVector3Controller.bpm = 120f;
        A__3__25_LFOVector3Controller.beatsPerCycle = 2f;
        A__3__25_LFOVector3Controller.waveform = (Enum)0;
        A__3__25_LFOVector3Controller.phaseOffset = 3.16f;
        A__3__25_LFOVector3Controller.loop = true;
        A__3__25_LFOVector3Controller.min = new Vector3(1000f, 0f, 0f);
        // Skipped generic property: action (type: LFOVector3Event)

        var A__3__25_CopyAndTransform2 = A__3__25.AddComponent<CopyAndTransform2>();
        A__3__25_CopyAndTransform2.numCopies = 30;
        // Skipped generic property: originalGameObjects (type: vector)
        A__3__25_CopyAndTransform2.deltaPosition = new Vector3(-200f, 0f, 0f);
        A__3__25_CopyAndTransform2.deltaScale = new Vector3(1f, 1f, 1f);
        A__3__25_CopyAndTransform2.colorMode = (Enum)0;
        // Skipped generic property: colors (type: vector)
        A__3__25_CopyAndTransform2.minColor = Color.black;

        var A__3__25_CycleInBeatsController = A__3__25.AddComponent<CycleInBeatsController>();
        A__3__25_CycleInBeatsController.useExternalBPMTime = true;
        A__3__25_CycleInBeatsController.bpm = 200f;
        A__3__25_CycleInBeatsController.beatsPerCycle = 1.401298E-45f;

        var A__3__25_OnNewCycleController = A__3__25.AddComponent<OnNewCycleController>();
        A__3__25_OnNewCycleController.cycleController = A__3__25.GetComponent<CycleInBeatsController>();
        // Skipped generic property: action (type: NewCycleEvent)

        var Sphere_27_MeshFilter = Sphere_27.AddComponent<MeshFilter>();
        Sphere_27_MeshFilter.Mesh = AssetDatabase.LoadAssetAtPath<Mesh>("Library/unity default resources");

        var Sphere_27_MeshRenderer = Sphere_27.AddComponent<MeshRenderer>();
        Sphere_27_MeshRenderer.CastShadows = (Enum)1;
        Sphere_27_MeshRenderer.ReceiveShadows = true;
        Sphere_27_MeshRenderer.DynamicOccludee = true;
        Sphere_27_MeshRenderer.MotionVectors = (Enum)1;
        Sphere_27_MeshRenderer.LightProbeUsage = 1;
        Sphere_27_MeshRenderer.ReflectionProbeUsage = 1;
        Sphere_27_MeshRenderer.RayTracingMode = 2;
        Sphere_27_MeshRenderer.RayTracingAccelStructBuildFlags = 1;
        Sphere_27_MeshRenderer.SmallMeshCulling = true;
        Sphere_27_MeshRenderer.RenderingLayerMask = 1;
        // Skipped generic property: Materials (type: vector)

        var Sphere_27_SphereCollider = Sphere_27.AddComponent<SphereCollider>();
        Sphere_27_SphereCollider.Radius = 0.5f;

        var Sphere_27_LFOVector3Controller = Sphere_27.AddComponent<LFOVector3Controller>();
        Sphere_27_LFOVector3Controller.useExternalBPMTime = true;
        Sphere_27_LFOVector3Controller.bpm = 120f;
        Sphere_27_LFOVector3Controller.beatsPerCycle = 1f;
        Sphere_27_LFOVector3Controller.waveform = (Enum)0;
        Sphere_27_LFOVector3Controller.loop = true;
        Sphere_27_LFOVector3Controller.min = new Vector3(3000f, 1f, 1f);
        Sphere_27_LFOVector3Controller.max = new Vector3(3000f, 30f, 30f);
        // Skipped generic property: action (type: LFOVector3Event)

        // Set up interfaces and complex references after all components are created
        A_3_OnNewCycleController.CycleController = A_3.GetComponent<ICycleController>();
        // Note: Assumes ICycleController component exists on the same GameObject
        A__2__6_OnNewCycleController.CycleController = A__2__6.GetComponent<ICycleController>();
        // Note: Assumes ICycleController component exists on the same GameObject
        A__1__9_OnNewCycleController.CycleController = A__1__9.GetComponent<ICycleController>();
        // Note: Assumes ICycleController component exists on the same GameObject
        A__3__12_OnNewCycleController.CycleController = A__3__12.GetComponent<ICycleController>();
        // Note: Assumes ICycleController component exists on the same GameObject
        A_16_OnNewCycleController.CycleController = A_16.GetComponent<ICycleController>();
        // Note: Assumes ICycleController component exists on the same GameObject
        A__2__19_OnNewCycleController.CycleController = A__2__19.GetComponent<ICycleController>();
        // Note: Assumes ICycleController component exists on the same GameObject
        A__1__22_OnNewCycleController.CycleController = A__1__22.GetComponent<ICycleController>();
        // Note: Assumes ICycleController component exists on the same GameObject
        A__3__25_OnNewCycleController.CycleController = A__3__25.GetComponent<ICycleController>();
        // Note: Assumes ICycleController component exists on the same GameObject

    }

    // Utility methods for finding created objects
    public GameObject FindGameObject(string name)
    {
        return GameObject.Find(name);
    }

    public T FindComponent<T>() where T : Component
    {
        return FindObjectOfType<T>();
    }
}
