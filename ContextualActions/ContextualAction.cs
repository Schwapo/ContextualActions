using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ContextualActions
{
    [HideReferenceObjectPicker]
    public class ContextualAction
    {
        [HideLabel]
        [HorizontalGroup(20f)]
        public Color Color = new Color(0.24f, 0.24f, 0.24f, 1f);

        [HideLabel]
        [HorizontalGroup]
        [ValueDropdown(nameof(GetMethods), NumberOfItemsBeforeEnablingSearch = 0)]
        public MethodInfo Method;

        private IEnumerable<ValueDropdownItem> GetMethods()
            => from type in Assembly.GetExecutingAssembly().GetTypes()
               from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
               let parameters = method.GetParameters()
               from parameter in parameters
               where parameters.Count() == 1 && parameter.ParameterType == typeof(IEnumerable<GameObject>)
               select new ValueDropdownItem(ObjectNames.NicifyVariableName(method.Name), method);
    }
}