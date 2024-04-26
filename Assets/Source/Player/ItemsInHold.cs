using System.Collections.Generic;
using Source.InteractiveObjects;
using UnityEngine;

namespace Source.Player
{
    public class ItemsInHold : MonoBehaviour
    {
        private readonly List<InteractiveObject> _itemsInHold = new ();

        public void AddObjectToItemList(InteractiveObject interactiveObject)
            => _itemsInHold.Add(interactiveObject);

        public bool FindItemInList(InteractiveObject neededItem)
        {
            if (_itemsInHold.Count == 0)
                return false;

            foreach (var item in _itemsInHold)
            {
                if (item == neededItem)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
