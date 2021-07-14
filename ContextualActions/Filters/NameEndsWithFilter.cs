using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace ContextualActions.Filters
{
    public class NameEndsWithFilter : IContextualActionFilter
    {
        [LabelText("Name Ends With")]
        public string String;

        public bool Matches(IEnumerable<GameObject> gameObjects)
        {
            foreach (var obj in gameObjects)
            {
                if (String != null && !obj.name.EndsWith(String))
                {
                    return false;
                }
            }

            return true;
        }
    }
}