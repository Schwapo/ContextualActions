using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace ContextualActions.Filters
{
    public class NameStartsWithFilter : IContextualActionFilter
    {
        [LabelText("Name Starts With")]
        public string String;

        public bool Matches(IEnumerable<GameObject> gameObjects)
        {
            foreach (var obj in gameObjects)
            {
                if (String != null && !obj.name.StartsWith(String))
                {
                    return false;
                }
            }

            return true;
        }
    }
}