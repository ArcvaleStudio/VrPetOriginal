﻿using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

namespace MalbersAnimations
{
    [CustomEditor(typeof(ActionZone))]
    public class ActionZoneEditor : Editor
    {
        [HideInInspector] public bool EditorShowForce = true, forceEnable = false, forceAction = false;
        [HideInInspector] public bool forceEnd = false, forceSight = false, forceGrab = false;
        private ActionZone M;

        string[] actionNames;
        MonoScript script;
        private void OnEnable()
        {
            M = ((ActionZone)target);
            script = MonoScript.FromMonoBehaviour((ActionZone)target);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical(MalbersEditor.StyleBlue);
            EditorGUILayout.HelpBox("Actions && Emotions for activating the Zones\nJust for gameObjects with the Animal Script ", MessageType.None);
            EditorGUILayout.EndVertical();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical(MalbersEditor.StyleGray);
            {
                EditorGUI.BeginDisabledGroup(true);
                script = (MonoScript)EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                M.actionsToUse = (Actions)EditorGUILayout.ObjectField("Actions Pack to use", M.actionsToUse, typeof(Actions), false);


                if (M.actionsToUse != null)
                {
                    actionNames = new string[M.actionsToUse.actions.Length];
                    for (int i = 0; i < M.actionsToUse.actions.Length; i++)
                    {
                        actionNames[i] = M.actionsToUse.actions[i].name;
                    }
                    M.index = EditorGUILayout.Popup("Actions & Emotions", M.index, actionNames);
                    M.ID = M.actionsToUse.actions[M.index].ID;
                }
                else
                {
                    EditorGUILayout.HelpBox("Add an Actions & Emotions Pack", MessageType.Warning);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    M.HeadOnly = EditorGUILayout.Toggle(new GUIContent("Head Only", "Enable only when the 'Head' bone enter the trigger zone"), M.HeadOnly);
                    M.nextTargetOverride = EditorGUILayout.Toggle(new GUIContent("Next Target Override", "If enabled, the fox will ignore any attempt to set its target by the player"), M.nextTargetOverride);
                }
                EditorGUILayout.EndVertical();
                //EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                //M.automatic = EditorGUILayout.Toggle(new GUIContent("Automatic", "As soon as the animal enters the zone play the action"), M.automatic);

                //    //if (M.automatic)
                //    //{
                //    //    M.AutomaticDisabled = EditorGUILayout.FloatField(new GUIContent("Disabled", "if true the Trigger will be disabled for this value in seconds"), M.AutomaticDisabled);
                //    //    if (M.AutomaticDisabled < 0) M.AutomaticDisabled = 0;
                //    //}
                //EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUILayout.BeginHorizontal();
                    M.Align = EditorGUILayout.Toggle(new GUIContent("Align", "Aligns the Animal to the Align Position and Rotation of the AlignPoint"), M.Align);
                    if (M.Align)
                    {
                        M.AlignPos = GUILayout.Toggle(M.AlignPos, new GUIContent("P", "Align Position"), EditorStyles.miniButton, GUILayout.MaxWidth(25));
                        M.AlignRot = GUILayout.Toggle(M.AlignRot, new GUIContent("R", "Align Rotation"), EditorStyles.miniButton, GUILayout.MaxWidth(25));
                        if (M.AlignPos || M.AlignRot)
                        {
                            M.AlignLookAt = false;
                        }

                        M.AlignLookAt = GUILayout.Toggle(M.AlignLookAt, new GUIContent("L", "Align Looking at the Zone"), EditorStyles.miniButton, GUILayout.MaxWidth(25));
                        if (M.AlignLookAt)
                        {
                            M.AlignPos = M.AlignRot = false;
                        }


                        if (M.AlingPoint== null)
                        {
                            M.AlingPoint = M.transform;
                        }
                        serializedObject.ApplyModifiedProperties();
                    }

                    EditorGUILayout.EndHorizontal();
                    if (M.Align)
                    {
                        M.AlingPoint =(Transform) EditorGUILayout.ObjectField("Align Point", M.AlingPoint, typeof(Transform), true);
                        M.AlignTime = EditorGUILayout.FloatField(new GUIContent("Align Time", "time to aling"), M.AlignTime);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("AlignCurve"), new GUIContent("Align Curve"));

                        if (M.AlignTime < 0)  M.AlignTime = 0;
                          
                    }
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUI.indentLevel++;
                M.EditorShowEvents = EditorGUILayout.Foldout(M.EditorShowEvents, "Events");
                EditorGUI.indentLevel--;

                if (M.EditorShowEvents)
                {
                    EditorGUILayout.BeginHorizontal();
                    forceEnd = GUILayout.Toggle(forceEnd, new GUIContent("X", "Fire Cannons"), EditorStyles.miniButton, GUILayout.MaxWidth(25));
                    if (forceEnd)
                    {
                        M.onEnd.Invoke();
                        forceEnd = false;
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("onGrab"), new GUIContent("On The Player Grabbing This"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("onEnable"), new GUIContent("On Actionzone Being Enabled"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("onAction"), new GUIContent("On Animal Action"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("onSight"), new GUIContent("On The Animal First Seeing This"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("onEnd"), new GUIContent("On The Action Ending"));
                }
                EditorGUILayout.EndVertical();


                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUI.indentLevel++;
                M.EditorAI = EditorGUILayout.Foldout(M.EditorAI, "AI");
                EditorGUI.indentLevel--;
                if (M.EditorAI)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("stoppingDistance"), new GUIContent("Stopping Distance"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("NextTarget"), new GUIContent("Next Target"));
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}