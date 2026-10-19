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

    [CustomEditor(typeof(CameraMultiTargetingRig)), CanEditMultipleObjects]
    public class CameraMultiTargetingRigEditor : GgEditor
    {
        #region Serialized Properties / OnEnable

        private CameraMultiTargetingRig targetAsType;

        private SerializedProperty defaultColor;
        private SerializedProperty trackedColor;
        
        private SerializedProperty gizmosOnSelected;
        private SerializedProperty zoomCamera;
        
        private SerializedProperty refCamLookAt;
        private SerializedProperty moveSpeed;
        private SerializedProperty targetObjects;
        
        private SerializedProperty cameraRig;
        private SerializedProperty boundsX;
        private SerializedProperty boundsY;
        private SerializedProperty boundsZ;
        private SerializedProperty padding;
        private SerializedProperty minZoom;
        private SerializedProperty maxZoom;
        private SerializedProperty zoomSpeed;
        
        
        private static int selectedTab = 0;
        private string[] tabs = new[] { "Settings", "Debug" };
        private int settingsTab = 0;
        private int debugTab = 1;

        private const string packageRefName = "CameraSystem";
        private Texture banner;

        private void OnEnable()
        {
            banner = EditorWindowUtility.LoadInspectorBanner();
            
            defaultColor = serializedObject.FindProperty(nameof(defaultColor));
            trackedColor = serializedObject.FindProperty(nameof(trackedColor));
            
            gizmosOnSelected = serializedObject.FindProperty(nameof(gizmosOnSelected));
            zoomCamera = serializedObject.FindProperty(nameof(zoomCamera));
            
            refCamLookAt = serializedObject.FindProperty(nameof(refCamLookAt));
            moveSpeed = serializedObject.FindProperty(nameof(moveSpeed));
            
            cameraRig = serializedObject.FindProperty(nameof(cameraRig));
            boundsX = serializedObject.FindProperty(nameof(boundsX));
            boundsY = serializedObject.FindProperty(nameof(boundsY));
            boundsZ = serializedObject.FindProperty(nameof(boundsZ));
            padding = serializedObject.FindProperty(nameof(padding));
            minZoom = serializedObject.FindProperty(nameof(minZoom));
            maxZoom = serializedObject.FindProperty(nameof(maxZoom));
            zoomSpeed = serializedObject.FindProperty(nameof(zoomSpeed));
            
            targetObjects = serializedObject.FindProperty(nameof(targetObjects));
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region OnInspectorGUI

        public override void OnInspectorGUI()
        {
            // get & update references
            if (!targetAsType) { targetAsType = target as CameraMultiTargetingRig; }
            serializedObject.Update();

            // draw banner if turned on in Gaskellgames settings
            EditorWindowUtility.TryDrawBanner(banner, nameof(CameraMultiTargetingRig).NicifyName());

            // custom inspector
            selectedTab = GUILayout.Toolbar(selectedTab, tabs);
            EditorGUILayout.Space();
            if (selectedTab == settingsTab)
            {
                EditorGUILayout.PropertyField(zoomCamera);
                EditorGUILayout.PropertyField(refCamLookAt);
                EditorGUILayout.PropertyField(moveSpeed);
                EditorGUILayout.PropertyField(targetObjects);
                if (zoomCamera.boolValue)
                {
                    EditorGUILayout.PropertyField(cameraRig);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Calculate Bounds");
                    boundsX.boolValue = EditorGUILayout.ToggleLeft("X", boundsX.boolValue, GUILayout.Width(28), GUILayout.ExpandWidth(false));
                    boundsY.boolValue = EditorGUILayout.ToggleLeft("Y", boundsY.boolValue, GUILayout.Width(28), GUILayout.ExpandWidth(false));
                    boundsZ.boolValue = EditorGUILayout.ToggleLeft("Z", boundsZ.boolValue, GUILayout.Width(28), GUILayout.ExpandWidth(false));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.PropertyField(padding);
                    EditorGUILayout.PropertyField(minZoom);
                    EditorGUILayout.PropertyField(maxZoom);
                    EditorGUILayout.PropertyField(zoomSpeed);
                }
            }
            else if (selectedTab == debugTab)
            {
                EditorGUILayout.PropertyField(gizmosOnSelected);
                EditorGUILayout.PropertyField(defaultColor);
                EditorGUILayout.PropertyField(trackedColor);
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