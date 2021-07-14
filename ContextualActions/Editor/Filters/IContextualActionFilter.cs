using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace ContextualActions.Filters
{
    [HideReferenceObjectPicker]
    public interface IContextualActionFilter
    {
        bool Matches(IEnumerable<GameObject> gameObjects);
    }
}