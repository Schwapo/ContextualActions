using ContextualActions.Filters;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ContextualActions
{
    [HideReferenceObjectPicker]
    public class ContextualActionProfile
    {
        [HideLabel]
        [HorizontalGroup]
        public string Name;

        [HideLabel]
        [HorizontalGroup(20f)]
        public Color Color = new Color(0.22f, 0.22f, 0.22f, 1f);

        [PropertySpace(10f)]
        [ListDrawerSettings(Expanded = true)]
        [ValueDropdown(nameof(GetFilters), DrawDropdownForListElements = false)]
        public List<IContextualActionFilter> Filters = new List<IContextualActionFilter>();

        [ListDrawerSettings(CustomAddFunction = nameof(AddContextualAction), Expanded = true)]
        public List<ContextualAction> Actions = new List<ContextualAction>();

        private IEnumerable<ValueDropdownItem> GetFilters()
            => from type in Assembly.GetExecutingAssembly().GetTypes()
               where !type.IsInterface && typeof(IContextualActionFilter).IsAssignableFrom(type)
               select new ValueDropdownItem(FormatFilterName(type.Name), Activator.CreateInstance(type));

        private string FormatFilterName(string name) => ObjectNames.NicifyVariableName(name.Replace("Filter", ""));
        private void AddContextualAction() => Actions.Add(new ContextualAction());
    }
}