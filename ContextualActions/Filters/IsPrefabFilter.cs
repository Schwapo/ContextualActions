using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ContextualActions.Filters
{
    public class IsPrefabFilter : IContextualActionFilter
    {
        public bool IsPrefab;

        public bool Matches(IEnumerable<GameObject> gameObjects)
        {
            foreach (var obj in gameObjects)
            {
                if (PrefabUtility.IsPartOfAnyPrefab(obj) != IsPrefab)
                {
                    return false;
                }
            }

            return true;
        }
    }
}