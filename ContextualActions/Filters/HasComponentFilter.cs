using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ContextualActions.Filters
{
    public class HasComponentFilter : IContextualActionFilter
    {
        [LabelText("Has Component")]
        [ValueDropdown(nameof(GetComponents))]
        public Type Component;

        public bool Matches(IEnumerable<GameObject> gameObjects)
        {
            foreach (var obj in gameObjects)
            {
                if (obj.GetComponent(Component) == null)
                {
                    return false;
                }
            }

            return true;
        }

        private IEnumerable<Type> GetComponents()
            => from assembly in AppDomain.CurrentDomain.GetAssemblies()
               from type in assembly.GetTypes()
               where typeof(Component).IsAssignableFrom(type)
               select type;
    }
}