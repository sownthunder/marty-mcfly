#if UNITY_EDITOR
#if GASKELLGAMES
#if GASKELLGAMES_INPUTEVENTSYSTEM
using Gaskellgames.EditorOnly;
using UnityEditor;
using UnityEngine;

namespace Gaskellgames.CameraSystem.EditorOnly
{
    /// <remarks>
    /// Code created by Gaskellgames: https://gaskellgames.com
    /// </remarks>
    
    [CustomEditor(typeof(CameraFreelookRig)), CanEditMultipleObjects]
    public class CameraFreelookRigEditor : GgEditor
    {
        #region Serialized Properties / OnEnable

        private CameraFreelookRig targetAsType;
        
        private SerializedProperty gizmosOnSelected;
        private SerializedProperty relativePosition;
        private SerializedProperty cameraCollisions;
        private SerializedProperty customInputAction;

        private SerializedProperty moveCamera;
        private SerializedProperty gmkInputController;

        private SerializedProperty cameraOrbit;

        private SerializedProperty xSensitivity;
        private SerializedProperty ySensitivity;
        private SerializedProperty rotationOffset;
        private SerializedProperty collisionOffset;
        private SerializedProperty collisionLayers;

        private SerializedProperty orbitExtentsColor;
        private SerializedProperty currentOrbitColor;
        private SerializedProperty collisionColor;
        
        private static int selectedTab = 0;
        private string[] tabs = new[] { "Settings", "Debug" };
        private int settingsTab = 0;
        private int debugTab = 1;
        
        private const string packageRefName = "CameraSystem";
        private Texture banner;

        private void OnEnable()
        {
            banner = EditorWindowUtility.LoadInspectorBanner();

            gizmosOnSelected = serializedObject.FindProperty(nameof(gizmosOnSelected));
            relativePosition = serializedObject.FindProperty(nameof(relativePosition));
            cameraCollisions = serializedObject.FindProperty(nameof(cameraCollisions));
            
            customInputAction = serializedObject.FindProperty(nameof(customInputAction));
            moveCamera = serializedObject.FindProperty(nameof(moveCamera));
            gmkInputController = serializedObject.FindProperty(nameof(gmkInputController));

            cameraOrbit = serializedObject.FindProperty(nameof(cameraOrbit));

            xSensitivity = serializedObject.FindProperty(nameof(xSensitivity));
            ySensitivity = serializedObject.FindProperty(nameof(ySensitivity));
            rotationOffset = serializedObject.FindProperty(nameof(rotationOffset));
            collisionOffset = serializedObject.FindProperty(nameof(collisionOffset));
            collisionLayers = serializedObject.FindProperty(nameof(collisionLayers));
            
            orbitExtentsColor = serializedObject.FindProperty(nameof(orbitExtentsColor));
            currentOrbitColor = serializedObject.FindProperty(nameof(currentOrbitColor));
            collisionColor = serializedObject.FindProperty(nameof(collisionColor));
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region OnInspectorGUI

        public override void OnInspectorGUI()
        {
            // get & update references
            if (!targetAsType) { targetAsType = target as CameraFreelookRig; }
            serializedObject.Update();

            // draw banner if turned on in Gaskellgames settings
            EditorWindowUtility.TryDrawBanner(banner, nameof(CameraFreelookRig).NicifyName());

            // custom inspector
            selectedTab = GUILayout.Toolbar(selectedTab, tabs);
            EditorGUILayout.Space();
            if (selectedTab == settingsTab)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(relativePosition);
                EditorGUILayout.PropertyField(cameraCollisions);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(customInputAction);
                EditorGUILayout.PropertyField(customInputAction.boolValue ? moveCamera : gmkInputController);
                EditorGUILayout.PropertyField(cameraOrbit);

                EditorGUILayout.PropertyField(xSensitivity);
                EditorGUILayout.PropertyField(ySensitivity);
                EditorGUILayout.PropertyField(rotationOffset);

                if (cameraCollisions.boolValue)
                {
                    EditorGUILayout.PropertyField(collisionOffset);
                    EditorGUILayout.PropertyField(collisionLayers);
                }
            }
            else if (selectedTab == debugTab)
            {
                EditorGUILayout.PropertyField(gizmosOnSelected);
                EditorGUILayout.PropertyField(orbitExtentsColor);
                EditorGUILayout.PropertyField(currentOrbitColor);
                EditorGUILayout.PropertyField(collisionColor);
            }

            // apply reference changes
            serializedObject.ApplyModifiedProperties();
        }

        #endregion
        
    } // class end
}
#endif
#endif
#endif