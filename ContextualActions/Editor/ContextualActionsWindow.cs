using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ContextualActions
{
    public class ContextualActionsWindow : OdinEditorWindow, IHasCustomMenu
    {
        public enum Tab
        {
            Actions,
            Settings
        }

        [ShowIf("@currentTab == Tab.Settings")]
        [ListDrawerSettings(CustomAddFunction = nameof(AddProfile), NumberOfItemsPerPage = 1)]
        public List<ContextualActionProfile> Profiles = new List<ContextualActionProfile>();

        private Tab currentTab;
        private Vector2 scrollPosition;

        private const float RowHeight = 20f;
        private const float ColorRectWidth = 6f;
        private const float ProfileNameWidth = 150f;
        private const string PrefsKey = "ContextualActionsWindow";

        [MenuItem("Tools/Contextual Actions")]
        public static void Open() => GetWindow<ContextualActionsWindow>("Contextual Actions");

        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Actions"), currentTab == Tab.Actions, SelectActionsTab);
            menu.AddItem(new GUIContent("Settings"), currentTab == Tab.Settings, SelectSettingsTab);
        }

        private void SelectActionsTab() => currentTab = Tab.Actions;
        private void SelectSettingsTab() => currentTab = Tab.Settings;

        protected override void OnEnable()
        {
            base.OnEnable();

            var json = EditorPrefs.GetString(PrefsKey);
            JsonUtility.FromJsonOverwrite(json, this);
        }

        private void OnDisable()
        {
            var json = JsonUtility.ToJson(this);
            EditorPrefs.SetString(PrefsKey, json);
        }

        protected override void OnGUI()
        {
            switch (currentTab)
            {
                case Tab.Actions:
                    DrawActions();
                    break;
                case Tab.Settings:
                    base.OnGUI();
                    break;
            }
        }

        private void AddProfile() => Profiles.Add(new ContextualActionProfile());

        private void DrawActions()
        {
            if (Selection.gameObjects.Length == 0) return;

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            foreach (var profile in Profiles)
            {
                var allFiltersMatch = profile.Filters.All(filter => filter?.Matches(Selection.gameObjects) ?? true);

                if (allFiltersMatch)
                {
                    DrawButtons(profile);
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawButtons(ContextualActionProfile profile)
        {
            var buttonWidth = position.width - ProfileNameWidth - (ColorRectWidth * 2f);

            for (var i = 0; i < profile.Actions.Count; i++)
            {
                var action = profile.Actions[i];

                if (action == null) continue;

                var rect = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, RowHeight);
                var actionColorRect = rect.AlignLeft(ColorRectWidth);
                var buttonRect = rect.AddX(ColorRectWidth).SetWidth(buttonWidth);
                var profileNameRect = rect.AddX(ColorRectWidth + buttonWidth).SetWidth(ProfileNameWidth);
                var profileColorRect = rect.AlignRight(ColorRectWidth);
                var horizontalLineRect = rect.AlignBottom(1f);

                var buttonStyle = EditorStyles.toolbarButton;
                buttonStyle.alignment = TextAnchor.MiddleLeft;

                if (action.Method == null)
                {
                    if (GUI.Button(buttonRect, "Null", buttonStyle)) { }
                }
                else
                {
                    var buttonLabel = ObjectNames.NicifyVariableName(action.Method.Name);

                    if (GUI.Button(buttonRect, buttonLabel, buttonStyle))
                    {
                        action.Method.Invoke(null, new object[] { Selection.gameObjects });
                    }
                }

                GUI.Label(profileNameRect, profile.Name, EditorStyles.centeredGreyMiniLabel);

                EditorGUI.DrawRect(actionColorRect, action.Color);
                EditorGUI.DrawRect(profileColorRect, profile.Color);
                EditorGUI.DrawRect(horizontalLineRect, SirenixGUIStyles.BorderColor);
            }
        }
    }
}