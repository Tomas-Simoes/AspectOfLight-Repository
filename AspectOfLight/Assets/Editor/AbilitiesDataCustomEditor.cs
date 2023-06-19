using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(D_Abilities))]
public class MyScriptableObjectEditor : Editor
{
    private static readonly string[] classTypeNames =
    {
        "MagicCrystalAbility",
        "ThePlagueAbility",
        "DashAbility",
        "RadiantCrystalAbility"
    };

    private SerializedProperty selectedClassIndexProperty;

    private void OnEnable()
    {
        selectedClassIndexProperty = serializedObject.FindProperty("selectedClassIndex");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        D_Abilities abilitiesData = (D_Abilities)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("abilities"), true);

        selectedClassIndexProperty.intValue = EditorGUILayout.Popup(
            "Add Ability",
            selectedClassIndexProperty.intValue,
            classTypeNames
        );

        if (GUILayout.Button("Add"))
        {
            AddAbility(abilitiesData, classTypeNames[selectedClassIndexProperty.intValue]);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void AddAbility(D_Abilities abilitiesData, string className)
    {
        System.Type classType = System.Type.GetType(className);
        Ability newAbility = System.Activator.CreateInstance(classType) as Ability;

        abilitiesData.abilities.Add(newAbility);
        EditorUtility.SetDirty(abilitiesData);
    }
}
