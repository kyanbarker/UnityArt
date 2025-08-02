// Place this file in an "Editor" folder in your project
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ComponentMigrationTool : EditorWindow
{
    private bool dryRun = true;
    private bool removeOldComponents = false;
    private Vector2 scrollPosition;

    [MenuItem("Tools/Component Migration Tool")]
    public static void ShowWindow()
    {
        GetWindow<ComponentMigrationTool>("Component Migration");
    }

    void OnGUI()
    {
        GUILayout.Label("Component Migration Tool", EditorStyles.boldLabel);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        EditorGUILayout.Space(10);

        dryRun = EditorGUILayout.Toggle("Dry Run (Preview Only)", dryRun);
        removeOldComponents = EditorGUILayout.Toggle("Remove Old Components", removeOldComponents);

        EditorGUILayout.Space(10);

        if (dryRun)
        {
            EditorGUILayout.HelpBox(
                "Dry Run Mode: No changes will be made. Check console for preview.",
                MessageType.Info
            );
        }
        else if (removeOldComponents)
        {
            EditorGUILayout.HelpBox(
                "WARNING: Old components will be removed! Make sure to backup your project.",
                MessageType.Warning
            );
        }

        EditorGUILayout.Space(10);

        // LFO Migration Section
        GUILayout.Label("LFO → LFO2 Migration", EditorStyles.boldLabel);

        var lfoComponents = Object.FindObjectsOfType<LFO>();
        EditorGUILayout.LabelField($"Found {lfoComponents.Length} LFO components");

        if (GUILayout.Button("Migrate LFO → LFO2"))
        {
            MigrateLFOToLFO2(lfoComponents);
        }

        EditorGUILayout.Space(10);

        // CopyAndTransform Migration Section
        GUILayout.Label("CopyAndTransform1 → CopyAndTransform2 Migration", EditorStyles.boldLabel);

        var copyTransformComponents = Object.FindObjectsOfType<CopyAndTransform1>();
        EditorGUILayout.LabelField(
            $"Found {copyTransformComponents.Length} CopyAndTransform1 components"
        );

        if (GUILayout.Button("Migrate CopyAndTransform1 → CopyAndTransform2"))
        {
            MigrateCopyAndTransform1ToCopyAndTransform2(copyTransformComponents);
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Migrate All Components"))
        {
            MigrateLFOToLFO2(lfoComponents);
            MigrateCopyAndTransform1ToCopyAndTransform2(copyTransformComponents);
        }

        EditorGUILayout.EndScrollView();
    }

    private void MigrateLFOToLFO2(LFO[] lfoComponents)
    {
        if (dryRun)
        {
            Debug.Log($"[DRY RUN] Would migrate {lfoComponents.Length} LFO components:");
            foreach (var lfo in lfoComponents)
            {
                Debug.Log(
                    $"  - {lfo.gameObject.name}: waveform={lfo.waveform}, bpm={lfo.BPM}, beatsPerCycle={lfo.BeatsPerCycle}"
                );
            }
            return;
        }

        int migratedCount = 0;

        foreach (var oldLFO in lfoComponents)
        {
            GameObject gameObject = oldLFO.gameObject;

            // Register undo for the entire GameObject
            Undo.RecordObject(gameObject, "Migrate LFO to LFO2");

            // Check if LFO2 already exists
            if (gameObject.GetComponent<LFO2>() != null)
            {
                Debug.LogWarning(
                    $"GameObject {gameObject.name} already has LFO2 component. Skipping."
                );
                continue;
            }

            // Add new component with undo support
            var newLFO = Undo.AddComponent<LFO2>(gameObject);

            // Copy values using SerializedObject for proper undo support
            var oldSO = new SerializedObject(oldLFO);
            var newSO = new SerializedObject(newLFO);

            // Copy waveform
            newSO.FindProperty("waveform").enumValueIndex = oldSO
                .FindProperty("waveform")
                .enumValueIndex;

            // Copy customCurve
            newSO.FindProperty("customCurve").animationCurveValue = oldSO
                .FindProperty("customCurve")
                .animationCurveValue;

            // Copy phaseOffset
            newSO.FindProperty("phaseOffset").floatValue = oldSO
                .FindProperty("phaseOffset")
                .floatValue;

            // Copy BPM-related fields (now inherited from BPMController → CycleInBeatsController)
            newSO.FindProperty("useExternalBPMTime").boolValue = oldSO
                .FindProperty("useExternalBPM")
                .boolValue;
            newSO.FindProperty("externalBPMTime").objectReferenceValue = oldSO
                .FindProperty("externalBPM")
                .objectReferenceValue;
            newSO.FindProperty("bpm").floatValue = oldSO.FindProperty("bpm").floatValue;

            // Copy beatsPerCycle (now inherited from CycleInBeatsController)
            newSO.FindProperty("beatsPerCycle").floatValue = oldSO
                .FindProperty("beatsPerCycle")
                .floatValue;

            newSO.ApplyModifiedProperties();

            migratedCount++;
            Debug.Log($"Migrated LFO → LFO2 on {gameObject.name}");

            // Remove old component if requested
            if (removeOldComponents)
            {
                Undo.DestroyObjectImmediate(oldLFO);
                Debug.Log($"Removed old LFO component from {gameObject.name}");
            }
        }

        Debug.Log($"Successfully migrated {migratedCount} LFO components to LFO2");
    }

    private void MigrateCopyAndTransform1ToCopyAndTransform2(
        CopyAndTransform1[] copyTransformComponents
    )
    {
        if (dryRun)
        {
            Debug.Log(
                $"[DRY RUN] Would migrate {copyTransformComponents.Length} CopyAndTransform1 components:"
            );
            foreach (var ct in copyTransformComponents)
            {
                // Show rotation conversion preview
                var oldSO = new SerializedObject(ct);
                float rotationDegrees = oldSO.FindProperty("rotationDegrees").floatValue;
                int rotationAxisIndex = oldSO.FindProperty("rotationAxis").enumValueIndex;

                Vector3 deltaRotation = Vector3.zero;
                switch ((RotationAxis)rotationAxisIndex)
                {
                    case RotationAxis.X:
                        deltaRotation = new Vector3(rotationDegrees, 0, 0);
                        break;
                    case RotationAxis.Y:
                        deltaRotation = new Vector3(0, rotationDegrees, 0);
                        break;
                    case RotationAxis.Z:
                        deltaRotation = new Vector3(0, 0, rotationDegrees);
                        break;
                }

                Debug.Log(
                    $"  - {ct.gameObject.name}: rotation {rotationDegrees}° on {(RotationAxis)rotationAxisIndex} → {deltaRotation}, numCopies={oldSO.FindProperty("numCopies").intValue}, delayBeats={oldSO.FindProperty("delayBeats").floatValue}"
                );
            }
            return;
        }

        int migratedCount = 0;

        foreach (var oldCopyTransform in copyTransformComponents)
        {
            GameObject gameObject = oldCopyTransform.gameObject;

            // Register undo for the entire GameObject
            Undo.RecordObject(gameObject, "Migrate CopyAndTransform1 to CopyAndTransform2");

            // Check if CopyAndTransform2 already exists
            if (gameObject.GetComponent<CopyAndTransform2>() != null)
            {
                Debug.LogWarning(
                    $"GameObject {gameObject.name} already has CopyAndTransform2 component. Skipping."
                );
                continue;
            }

            // Add new component with undo support
            var newCopyTransform = Undo.AddComponent<CopyAndTransform2>(gameObject);

            // Copy values using SerializedObject
            var oldSO = new SerializedObject(oldCopyTransform);
            var newSO = new SerializedObject(newCopyTransform);

            // Copy direct mappings
            newSO.FindProperty("numCopies").intValue = oldSO.FindProperty("numCopies").intValue;
            newSO.FindProperty("deltaPosition").vector3Value = oldSO
                .FindProperty("deltaPosition")
                .vector3Value;
            newSO.FindProperty("deltaScale").vector3Value = oldSO
                .FindProperty("deltaScale")
                .vector3Value;

            // Map gameObjectsToClone -> originalGameObjects
            var oldGameObjectsList = oldSO.FindProperty("gameObjectsToClone");
            var newGameObjectsList = newSO.FindProperty("originalGameObjects");
            newGameObjectsList.arraySize = oldGameObjectsList.arraySize;
            for (int i = 0; i < oldGameObjectsList.arraySize; i++)
            {
                newGameObjectsList.GetArrayElementAtIndex(i).objectReferenceValue =
                    oldGameObjectsList.GetArrayElementAtIndex(i).objectReferenceValue;
            }

            // Convert rotationDegrees + rotationAxis to deltaRotation
            float rotationDegrees = oldSO.FindProperty("rotationDegrees").floatValue;
            int rotationAxisIndex = oldSO.FindProperty("rotationAxis").enumValueIndex;

            Vector3 deltaRotation = Vector3.zero;
            switch ((RotationAxis)rotationAxisIndex)
            {
                case RotationAxis.X:
                    deltaRotation = new Vector3(rotationDegrees, 0, 0);
                    break;
                case RotationAxis.Y:
                    deltaRotation = new Vector3(0, rotationDegrees, 0);
                    break;
                case RotationAxis.Z:
                    deltaRotation = new Vector3(0, 0, rotationDegrees);
                    break;
            }
            newSO.FindProperty("deltaRotation").vector3Value = deltaRotation;

            newSO.ApplyModifiedProperties();

            // Add CycleInBeatsController to handle BPM/timing logic
            var cycleInBeatsController = Undo.AddComponent<CycleInBeatsController>(gameObject);
            var cycleBeatsSO = new SerializedObject(cycleInBeatsController);

            // Copy BPM-related fields to CycleInBeatsController
            cycleBeatsSO.FindProperty("useExternalBPMTime").boolValue = oldSO
                .FindProperty("useParentBPM")
                .boolValue;
            cycleBeatsSO.FindProperty("bpm").floatValue = oldSO.FindProperty("bpm").floatValue;

            // Set beatsPerCycle to delayBeats (since CopyAndTransform1 waits delayBeats between copies)
            cycleBeatsSO.FindProperty("beatsPerCycle").floatValue = oldSO
                .FindProperty("delayBeats")
                .floatValue;

            cycleBeatsSO.ApplyModifiedProperties();

            // Add OnNewCycleController to handle the copy triggering
            var onNewCycleController = Undo.AddComponent<OnNewCycleController>(gameObject);
            var cycleSO = new SerializedObject(onNewCycleController);
            Debug.Assert(cycleSO != null);
            Debug.Assert(cycleSO.FindProperty("cycleController") != null);
            // Set the cycleController reference
            cycleSO.FindProperty("cycleController").objectReferenceValue = cycleInBeatsController;

            cycleSO.ApplyModifiedProperties();

            // Wire up the action to call CopyAndTransform2.CreateCopy
            var actionProperty = cycleSO.FindProperty("action.m_PersistentCalls.m_Calls");
            actionProperty.arraySize = 1;
            var callProperty = actionProperty.GetArrayElementAtIndex(0);

            callProperty.FindPropertyRelative("m_Target").objectReferenceValue = newCopyTransform;
            callProperty.FindPropertyRelative("m_MethodName").stringValue = "CreateCopy";
            callProperty.FindPropertyRelative("m_Mode").intValue = 0; // Dynamic int
            callProperty.FindPropertyRelative("m_CallState").intValue = 2; // RuntimeOnly

            cycleSO.ApplyModifiedProperties();

            Debug.Log(
                $"Added CycleInBeatsController + OnNewCycleController with beatsPerCycle={oldSO.FindProperty("delayBeats").floatValue} and wired to CreateCopy"
            );

            migratedCount++;
            Debug.Log($"Migrated CopyAndTransform1 → CopyAndTransform2 on {gameObject.name}");
            Debug.Log(
                $"  Converted rotation: {rotationDegrees}° on {(RotationAxis)rotationAxisIndex} axis → {deltaRotation}"
            );

            // Remove old component if requested
            if (removeOldComponents)
            {
                Undo.DestroyObjectImmediate(oldCopyTransform);
                Debug.Log($"Removed old CopyAndTransform1 component from {gameObject.name}");
            }
        }

        Debug.Log(
            $"Successfully migrated {migratedCount} CopyAndTransform1 components to CopyAndTransform2"
        );
    }
}

// Additional utility for batch operations
public static class MigrationUtilities
{
    [MenuItem("Tools/Quick Migration/Migrate Selected LFO Components")]
    public static void MigrateSelectedLFOComponents()
    {
        var selectedObjects = Selection.gameObjects;
        List<LFO> lfoComponents = new List<LFO>();

        foreach (var gameObject in selectedObjects)
        {
            var lfo = gameObject.GetComponent<LFO>();
            if (lfo != null)
            {
                lfoComponents.Add(lfo);
            }
        }

        if (lfoComponents.Count == 0)
        {
            Debug.LogWarning("No LFO components found in selected GameObjects");
            return;
        }

        Debug.Log(
            $"Found {lfoComponents.Count} LFO components in selection. Use the Migration Tool window for full control."
        );
    }

    [MenuItem("Tools/Quick Migration/Migrate Selected CopyAndTransform1 Components")]
    public static void MigrateSelectedCopyAndTransformComponents()
    {
        var selectedObjects = Selection.gameObjects;
        List<CopyAndTransform1> copyTransformComponents = new List<CopyAndTransform1>();

        foreach (var gameObject in selectedObjects)
        {
            var ct = gameObject.GetComponent<CopyAndTransform1>();
            if (ct != null)
            {
                copyTransformComponents.Add(ct);
            }
        }

        if (copyTransformComponents.Count == 0)
        {
            Debug.LogWarning("No CopyAndTransform1 components found in selected GameObjects");
            return;
        }

        Debug.Log(
            $"Found {copyTransformComponents.Count} CopyAndTransform1 components in selection. Use the Migration Tool window for full control."
        );
    }

    [MenuItem("Tools/Quick Migration/Find All Migration Candidates")]
    public static void FindAllMigrationCandidates()
    {
        var lfoComponents = Object.FindObjectsOfType<LFO>();
        var copyTransformComponents = Object.FindObjectsOfType<CopyAndTransform1>();

        Debug.Log($"Migration Candidates Found:");
        Debug.Log($"  LFO components: {lfoComponents.Length}");
        Debug.Log($"  CopyAndTransform1 components: {copyTransformComponents.Length}");

        if (lfoComponents.Length > 0)
        {
            Debug.Log("LFO components found in:");
            foreach (var lfo in lfoComponents)
            {
                Debug.Log($"  - {lfo.gameObject.name} (Scene: {lfo.gameObject.scene.name})");
            }
        }

        if (copyTransformComponents.Length > 0)
        {
            Debug.Log("CopyAndTransform1 components found in:");
            foreach (var ct in copyTransformComponents)
            {
                Debug.Log($"  - {ct.gameObject.name} (Scene: {ct.gameObject.scene.name})");
            }
        }
    }
}
