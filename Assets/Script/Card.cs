using UnityEngine;
using UnityEngine.InputSystem;

namespace Composition
{
    public class Card : Dragable
    {
        private bool isFaceUp = true;
        public midCardDrop currPile;

        public void DropCard(IDropArea cardDropArea)
        {
            cardDropArea.dropArea(this);
        }


        void Update()
        {
            Debug.Log(LayerMask.LayerToName(gameObject.layer));

            if (colItem != null)
            {
                DropCard(colItem);
                colItem = null;
            }
        }
    }
}
