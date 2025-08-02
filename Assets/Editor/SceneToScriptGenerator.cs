// Place this in an "Editor" folder
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

public class SceneToScriptGenerator : EditorWindow
{
    private bool includeInactiveObjects = true;
    private bool generateComments = true;
    private bool includeTransforms = true;
    private bool includeComponents = true;
    private bool includeUnityEvents = true;
    private string outputFolder = "Assets/GeneratedScripts";
    private Vector2 scrollPosition;

    [MenuItem("Tools/Scene to Script Generator")]
    public static void ShowWindow()
    {
        GetWindow<SceneToScriptGenerator>("Scene to Script Generator");
    }

    void OnGUI()
    {
        GUILayout.Label("Scene to Script Generator", EditorStyles.boldLabel);
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        EditorGUILayout.Space(10);
        
        // Options
        includeInactiveObjects = EditorGUILayout.Toggle("Include Inactive GameObjects", includeInactiveObjects);
        generateComments = EditorGUILayout.Toggle("Generate Comments", generateComments);
        includeTransforms = EditorGUILayout.Toggle("Include Transform Components", includeTransforms);
        includeComponents = EditorGUILayout.Toggle("Include Other Components", includeComponents);
        includeUnityEvents = EditorGUILayout.Toggle("Include UnityEvent Wiring", includeUnityEvents);
        
        EditorGUILayout.Space(10);
        
        EditorGUILayout.LabelField("Output Folder:");
        outputFolder = EditorGUILayout.TextField(outputFolder);
        
        EditorGUILayout.Space(10);
        
        // Current scene info
        var currentScene = EditorSceneManager.GetActiveScene();
        EditorGUILayout.LabelField($"Current Scene: {currentScene.name}");
        EditorGUILayout.LabelField($"Output Script: {currentScene.name}Script.cs");
        
        EditorGUILayout.Space(10);
        
        if (GUILayout.Button("Generate Script from Current Scene"))
        {
            GenerateScriptFromScene();
        }
        
        EditorGUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "This will create a script that recreates your entire scene hierarchy programmatically. " +
            "Includes GameObjects, components, transforms, and UnityEvent wiring. " +
            "Interface fields will use GetComponent<T>() calls for automatic assignment.",
            MessageType.Info);
        
        EditorGUILayout.EndScrollView();
    }

    private void GenerateScriptFromScene()
    {
        var currentScene = EditorSceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        string className = $"{sceneName}Script";
        
        // Ensure output directory exists
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }
        
        // Get all root GameObjects
        var rootObjects = currentScene.GetRootGameObjects()
            .Where(go => includeInactiveObjects || go.activeInHierarchy)
            .ToArray();
        
        StringBuilder script = new StringBuilder();
        Dictionary<GameObject, string> objectVariableNames = new Dictionary<GameObject, string>();
        List<GameObject> allObjects = new List<GameObject>();
        
        // Collect all objects and assign variable names
        foreach (var rootObj in rootObjects)
        {
            CollectGameObjectsRecursive(rootObj, allObjects);
        }
        
        for (int i = 0; i < allObjects.Count; i++)
        {
            string varName = SanitizeVariableName(allObjects[i].name) + "_" + i;
            objectVariableNames[allObjects[i]] = varName;
        }
        
        // Generate script
        GenerateScriptHeader(script, className);
        GenerateVariableDeclarations(script, allObjects, objectVariableNames);
        GenerateStartMethod(script, rootObjects, allObjects, objectVariableNames);
        GenerateUtilityMethods(script);
        GenerateScriptFooter(script);
        
        // Write to file
        string filePath = Path.Combine(outputFolder, $"{className}.cs");
        File.WriteAllText(filePath, script.ToString());
        
        // Refresh Unity
        AssetDatabase.Refresh();
        
        Debug.Log($"Generated scene script: {filePath}");
        Debug.Log($"Script contains {allObjects.Count} GameObjects");
        
        // Select the generated script
        var scriptAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(filePath);
        if (scriptAsset != null)
        {
            Selection.activeObject = scriptAsset;
            EditorGUIUtility.PingObject(scriptAsset);
        }
    }

    private void CollectGameObjectsRecursive(GameObject obj, List<GameObject> allObjects)
    {
        if (!includeInactiveObjects && !obj.activeInHierarchy)
            return;
            
        allObjects.Add(obj);
        
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            CollectGameObjectsRecursive(obj.transform.GetChild(i).gameObject, allObjects);
        }
    }

    private void GenerateScriptHeader(StringBuilder script, string className)
    {
        script.AppendLine("using UnityEngine;");
        script.AppendLine("using UnityEngine.Events;");
        script.AppendLine("using System.Collections.Generic;");
        script.AppendLine();
        
        if (generateComments)
        {
            script.AppendLine("/// <summary>");
            script.AppendLine($"/// Auto-generated script to recreate scene: {EditorSceneManager.GetActiveScene().name}");
            script.AppendLine($"/// Generated on: {System.DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            script.AppendLine("/// NOTE: Interface fields and Action delegates may need manual setup");
            script.AppendLine("/// </summary>");
        }
        
        script.AppendLine($"public class {className} : MonoBehaviour");
        script.AppendLine("{");
    }

    private void GenerateVariableDeclarations(StringBuilder script, List<GameObject> allObjects, Dictionary<GameObject, string> objectVariableNames)
    {
        if (generateComments)
        {
            script.AppendLine("    // GameObject references");
        }
        
        foreach (var obj in allObjects)
        {
            script.AppendLine($"    private GameObject {objectVariableNames[obj]};");
        }
        script.AppendLine();
    }

    private void GenerateStartMethod(StringBuilder script, GameObject[] rootObjects, List<GameObject> allObjects, Dictionary<GameObject, string> objectVariableNames)
    {
        script.AppendLine("    void Start()");
        script.AppendLine("    {");
        
        if (generateComments)
        {
            script.AppendLine("        // Create all GameObjects");
        }
        
        // Create GameObjects
        foreach (var obj in allObjects)
        {
            string varName = objectVariableNames[obj];
            script.AppendLine($"        {varName} = new GameObject(\"{obj.name}\");");
            
            if (!obj.activeInHierarchy)
            {
                script.AppendLine($"        {varName}.SetActive(false);");
            }
        }
        
        script.AppendLine();
        
        if (generateComments)
        {
            script.AppendLine("        // Set up hierarchy");
        }
        
        // Set up parent-child relationships
        foreach (var obj in allObjects)
        {
            if (obj.transform.parent != null)
            {
                string childVar = objectVariableNames[obj];
                string parentVar = objectVariableNames[obj.transform.parent.gameObject];
                script.AppendLine($"        {childVar}.transform.SetParent({parentVar}.transform);");
            }
        }
        
        script.AppendLine();
        
        // Set transforms
        if (includeTransforms)
        {
            GenerateTransformSettings(script, allObjects, objectVariableNames);
        }
        
        // Set components
        if (includeComponents)
        {
            GenerateComponentSettings(script, allObjects, objectVariableNames);
        }
        
        script.AppendLine("    }");
    }

    private void GenerateTransformSettings(StringBuilder script, List<GameObject> allObjects, Dictionary<GameObject, string> objectVariableNames)
    {
        if (generateComments)
        {
            script.AppendLine("        // Set transforms");
        }
        
        foreach (var obj in allObjects)
        {
            string varName = objectVariableNames[obj];
            var transform = obj.transform;
            
            // Position
            if (transform.localPosition != Vector3.zero)
            {
                script.AppendLine($"        {varName}.transform.localPosition = {Vector3ToString(transform.localPosition)};");
            }
            
            // Rotation
            if (transform.localRotation != Quaternion.identity)
            {
                script.AppendLine($"        {varName}.transform.localRotation = {QuaternionToString(transform.localRotation)};");
            }
            
            // Scale
            if (transform.localScale != Vector3.one)
            {
                script.AppendLine($"        {varName}.transform.localScale = {Vector3ToString(transform.localScale)};");
            }
        }
        
        script.AppendLine();
    }

    private void GenerateComponentSettings(StringBuilder script, List<GameObject> allObjects, Dictionary<GameObject, string> objectVariableNames)
    {
        if (generateComments)
        {
            script.AppendLine("        // Add and configure components");
        }
        
        foreach (var obj in allObjects)
        {
            string varName = objectVariableNames[obj];
            var components = obj.GetComponents<Component>()
                .Where(c => c != null && !(c is Transform)) // Skip Transform
                .ToArray();
            
            foreach (var component in components)
            {
                GenerateComponentCode(script, component, varName, objectVariableNames);
            }
        }
        
        // Generate post-initialization for interfaces and complex references
        if (generateComments)
        {
            script.AppendLine("        // Set up interfaces and complex references after all components are created");
        }
        
        foreach (var obj in allObjects)
        {
            string varName = objectVariableNames[obj];
            var components = obj.GetComponents<Component>()
                .Where(c => c != null && !(c is Transform))
                .ToArray();
            
            foreach (var component in components)
            {
                GeneratePostInitializationCode(script, component, varName, objectVariableNames);
            }
        }
        
        script.AppendLine();
    }

    private void GenerateComponentCode(StringBuilder script, Component component, string gameObjectVar, Dictionary<GameObject, string> objectVariableNames)
    {
        string componentType = component.GetType().Name;
        string componentVar = $"{gameObjectVar}_{componentType}";
        
        script.AppendLine($"        var {componentVar} = {gameObjectVar}.AddComponent<{componentType}>();");
        
        // Set component properties using SerializedObject
        var serializedObject = new SerializedObject(component);
        var property = serializedObject.GetIterator();
        
        if (property.NextVisible(true)) // Enter children
        {
            do
            {
                if (property.name == "m_Script") continue; // Skip script reference
                
                GeneratePropertyAssignment(script, property, componentVar, objectVariableNames);
                
            } while (property.NextVisible(false));
        }
        
        script.AppendLine();
    }

    private void GeneratePropertyAssignment(StringBuilder script, SerializedProperty property, string componentVar, Dictionary<GameObject, string> objectVariableNames)
    {
        string propertyName = property.name;
        
        // Convert Unity's internal property names to public property names
        if (propertyName.StartsWith("m_"))
        {
            propertyName = propertyName.Substring(2);
            if (propertyName.Length > 0)
            {
                propertyName = char.ToUpper(propertyName[0]) + propertyName.Substring(1);
            }
        }
        
        try
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Float:
                    if (property.floatValue != 0)
                        script.AppendLine($"        {componentVar}.{propertyName} = {property.floatValue}f;");
                    break;
                    
                case SerializedPropertyType.Integer:
                    if (property.intValue != 0)
                        script.AppendLine($"        {componentVar}.{propertyName} = {property.intValue};");
                    break;
                    
                case SerializedPropertyType.Boolean:
                    if (property.boolValue)
                        script.AppendLine($"        {componentVar}.{propertyName} = true;");
                    break;
                    
                case SerializedPropertyType.String:
                    if (!string.IsNullOrEmpty(property.stringValue))
                        script.AppendLine($"        {componentVar}.{propertyName} = \"{property.stringValue.Replace("\"", "\\\"")}\";");
                    break;
                    
                case SerializedPropertyType.Vector2:
                    if (property.vector2Value != Vector2.zero)
                        script.AppendLine($"        {componentVar}.{propertyName} = {Vector2ToString(property.vector2Value)};");
                    break;
                    
                case SerializedPropertyType.Vector3:
                    if (property.vector3Value != Vector3.zero)
                        script.AppendLine($"        {componentVar}.{propertyName} = {Vector3ToString(property.vector3Value)};");
                    break;
                    
                case SerializedPropertyType.Color:
                    if (property.colorValue != Color.white)
                        script.AppendLine($"        {componentVar}.{propertyName} = {ColorToString(property.colorValue)};");
                    break;
                    
                case SerializedPropertyType.ObjectReference:
                    if (property.objectReferenceValue != null)
                    {
                        if (property.objectReferenceValue is GameObject referencedGameObject && 
                            objectVariableNames.ContainsKey(referencedGameObject))
                        {
                            // Reference to another GameObject in the scene
                            script.AppendLine($"        {componentVar}.{propertyName} = {objectVariableNames[referencedGameObject]};");
                        }
                        else if (property.objectReferenceValue is Component referencedComponent && 
                                objectVariableNames.ContainsKey(referencedComponent.gameObject))
                        {
                            // Reference to a component on another GameObject
                            string refGameObjectVar = objectVariableNames[referencedComponent.gameObject];
                            string refComponentType = referencedComponent.GetType().Name;
                            script.AppendLine($"        {componentVar}.{propertyName} = {refGameObjectVar}.GetComponent<{refComponentType}>();");
                        }
                        else
                        {
                            // Asset reference (Material, Texture, etc.)
                            string assetPath = AssetDatabase.GetAssetPath(property.objectReferenceValue);
                            if (!string.IsNullOrEmpty(assetPath))
                            {
                                script.AppendLine($"        {componentVar}.{propertyName} = AssetDatabase.LoadAssetAtPath<{property.objectReferenceValue.GetType().Name}>(\"{assetPath}\");");
                            }
                        }
                    }
                    break;
                    
                case SerializedPropertyType.Enum:
                    script.AppendLine($"        {componentVar}.{propertyName} = ({property.type}){property.enumValueIndex};");
                    break;
                    
                case SerializedPropertyType.Generic:
                    // Handle UnityEvents in post-initialization phase
                    if (includeUnityEvents && property.type.Contains("UnityEvent"))
                    {
                        if (generateComments)
                        {
                            script.AppendLine($"        // UnityEvent {propertyName} will be configured later");
                        }
                    }
                    else if (generateComments)
                    {
                        script.AppendLine($"        // Skipped generic property: {propertyName} (type: {property.type})");
                    }
                    break;
            }
        }
        catch (System.Exception e)
        {
            if (generateComments)
            {
                script.AppendLine($"        // Failed to serialize property {propertyName}: {e.Message}");
            }
        }
    }

    private void GeneratePostInitializationCode(StringBuilder script, Component component, string gameObjectVar, Dictionary<GameObject, string> objectVariableNames)
    {
        string componentType = component.GetType().Name;
        string componentVar = $"{gameObjectVar}_{componentType}";
        
        // Handle interface assignments and UnityEvents
        var serializedObject = new SerializedObject(component);
        var property = serializedObject.GetIterator();
        
        if (property.NextVisible(true))
        {
            do
            {
                if (property.name == "m_Script") continue;
                
                // Handle UnityEvents
                if (includeUnityEvents && property.propertyType == SerializedPropertyType.Generic && property.type.Contains("UnityEvent"))
                {
                    GenerateUnityEventCode(script, property, componentVar, objectVariableNames);
                }
                // Handle interface assignments
                else if (IsLikelyInterfaceProperty(property, component))
                {
                    GenerateInterfaceAssignment(script, property, componentVar, gameObjectVar, objectVariableNames);
                }
                
            } while (property.NextVisible(false));
        }
    }

    private void GenerateUnityEventCode(StringBuilder script, SerializedProperty eventProperty, string componentVar, Dictionary<GameObject, string> objectVariableNames)
    {
        var persistentCallsProperty = eventProperty.FindPropertyRelative("m_PersistentCalls.m_Calls");
        if (persistentCallsProperty == null || persistentCallsProperty.arraySize == 0)
            return;

        if (generateComments)
        {
            script.AppendLine($"        // Configure UnityEvent: {eventProperty.name}");
        }

        for (int i = 0; i < persistentCallsProperty.arraySize; i++)
        {
            var call = persistentCallsProperty.GetArrayElementAtIndex(i);
            var target = call.FindPropertyRelative("m_Target")?.objectReferenceValue;
            var methodName = call.FindPropertyRelative("m_MethodName")?.stringValue;

            if (target != null && !string.IsNullOrEmpty(methodName))
            {
                string targetReference = GetComponentReference(target, objectVariableNames);
                if (!string.IsNullOrEmpty(targetReference))
                {
                    // Generate AddListener call
                    script.AppendLine($"        {componentVar}.{eventProperty.name}.AddListener({targetReference}.{methodName});");
                }
            }
        }
    }

    private bool IsLikelyInterfaceProperty(SerializedProperty property, Component component)
    {
        // Check if property name suggests it's an interface
        var fieldInfo = component.GetType().GetField(property.name, 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        
        if (fieldInfo != null)
        {
            var fieldType = fieldInfo.FieldType;
            return fieldType.IsInterface || 
                   (fieldType == typeof(MonoBehaviour) && property.name.ToLower().Contains("controller"));
        }
        
        return false;
    }

    private void GenerateInterfaceAssignment(StringBuilder script, SerializedProperty property, string componentVar, string gameObjectVar, Dictionary<GameObject, string> objectVariableNames)
    {
        string propertyName = property.name;
        if (propertyName.StartsWith("m_"))
        {
            propertyName = propertyName.Substring(2);
            if (propertyName.Length > 0)
            {
                propertyName = char.ToUpper(propertyName[0]) + propertyName.Substring(1);
            }
        }

        // Special handling for known interface patterns
        if (propertyName.ToLower().Contains("cyclecontroller"))
        {
            script.AppendLine($"        {componentVar}.CycleController = {gameObjectVar}.GetComponent<ICycleController>();");
            
            if (generateComments)
            {
                script.AppendLine("        // Note: Assumes ICycleController component exists on the same GameObject");
            }
        }
        else if (property.objectReferenceValue != null)
        {
            // Try to find the referenced object
            string targetReference = GetComponentReference(property.objectReferenceValue, objectVariableNames);
            if (!string.IsNullOrEmpty(targetReference))
            {
                script.AppendLine($"        {componentVar}.{propertyName} = {targetReference};");
            }
        }
        else if (generateComments)
        {
            script.AppendLine($"        // TODO: Set interface property {propertyName} manually");
        }
    }

    private string GetComponentReference(Object target, Dictionary<GameObject, string> objectVariableNames)
    {
        if (target is GameObject targetGameObject && objectVariableNames.ContainsKey(targetGameObject))
        {
            return objectVariableNames[targetGameObject];
        }
        else if (target is Component targetComponent && objectVariableNames.ContainsKey(targetComponent.gameObject))
        {
            string targetGameObjectVar = objectVariableNames[targetComponent.gameObject];
            string targetComponentType = targetComponent.GetType().Name;
            return $"{targetGameObjectVar}.GetComponent<{targetComponentType}>()";
        }
        
        return null;
    }

    private void GenerateUtilityMethods(StringBuilder script)
    {
        script.AppendLine();
        if (generateComments)
        {
            script.AppendLine("    // Utility methods for finding created objects");
        }
        
        script.AppendLine("    public GameObject FindGameObject(string name)");
        script.AppendLine("    {");
        script.AppendLine("        return GameObject.Find(name);");
        script.AppendLine("    }");
        script.AppendLine();
        
        script.AppendLine("    public T FindComponent<T>() where T : Component");
        script.AppendLine("    {");
        script.AppendLine("        return FindObjectOfType<T>();");
        script.AppendLine("    }");
    }

    private void GenerateScriptFooter(StringBuilder script)
    {
        script.AppendLine("}");
    }

    private string SanitizeVariableName(string name)
    {
        // Remove invalid characters and ensure valid C# variable name
        string sanitized = "";
        bool firstChar = true;
        
        foreach (char c in name)
        {
            if (firstChar)
            {
                if (char.IsLetter(c) || c == '_')
                {
                    sanitized += c;
                    firstChar = false;
                }
                else if (char.IsDigit(c))
                {
                    sanitized += "_" + c;
                    firstChar = false;
                }
            }
            else
            {
                if (char.IsLetterOrDigit(c) || c == '_')
                {
                    sanitized += c;
                }
                else
                {
                    sanitized += "_";
                }
            }
        }
        
        return string.IsNullOrEmpty(sanitized) ? "GameObject" : sanitized;
    }

    private string Vector2ToString(Vector2 v)
    {
        return $"new Vector2({v.x}f, {v.y}f)";
    }

    private string Vector3ToString(Vector3 v)
    {
        return $"new Vector3({v.x}f, {v.y}f, {v.z}f)";
    }

    private string QuaternionToString(Quaternion q)
    {
        return $"new Quaternion({q.x}f, {q.y}f, {q.z}f, {q.w}f)";
    }

    private string ColorToString(Color c)
    {
        if (c == Color.white) return "Color.white";
        if (c == Color.black) return "Color.black";
        if (c == Color.red) return "Color.red";
        if (c == Color.green) return "Color.green";
        if (c == Color.blue) return "Color.blue";
        if (c == Color.yellow) return "Color.yellow";
        if (c == Color.cyan) return "Color.cyan";
        if (c == Color.magenta) return "Color.magenta";
        
        return $"new Color({c.r}f, {c.g}f, {c.b}f, {c.a}f)";
    }
}

// Alternative: Console-based generator for quick use
public static class QuickSceneGenerator
{
    [MenuItem("Tools/Quick Scene Generator/Generate Current Scene Script")]
    public static void GenerateCurrentSceneScript()
    {
        var currentScene = EditorSceneManager.GetActiveScene();
        var rootObjects = currentScene.GetRootGameObjects();
        
        Debug.Log($"=== Generated code for scene: {currentScene.name} ===");
        Debug.Log("// Add this to a MonoBehaviour's Start() method:");
        
        for (int i = 0; i < rootObjects.Length; i++)
        {
            var obj = rootObjects[i];
            Debug.Log($"var obj{i} = new GameObject(\"{obj.name}\");");
            Debug.Log($"obj{i}.transform.position = {Vector3ToString(obj.transform.position)};");
            Debug.Log($"obj{i}.transform.rotation = {QuaternionToString(obj.transform.rotation)};");
            Debug.Log($"obj{i}.transform.localScale = {Vector3ToString(obj.transform.localScale)};");
            Debug.Log("");
        }
        
        Debug.Log("=== End generated code ===");
    }
    
    private static string Vector3ToString(Vector3 v)
    {
        return $"new Vector3({v.x}f, {v.y}f, {v.z}f)";
    }
    
    private static string QuaternionToString(Quaternion q)
    {
        return $"new Quaternion({q.x}f, {q.y}f, {q.z}f, {q.w}f)";
    }
}