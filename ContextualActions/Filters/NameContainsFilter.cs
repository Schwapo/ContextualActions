using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace ContextualActions.Filters
{
    public class NameContainsFilter : IContextualActionFilter
    {
        [LabelText("Name Contains")]
        public string String;

        public bool Matches(IEnumerable<GameObject> gameObjects)
        {
            foreach (var obj in gameObjects)
            {
                if (String != null && !obj.name.Contains(String))
                {
                    return false;
                }
            }

            return true;
        }
    }
}