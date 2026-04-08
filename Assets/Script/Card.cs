using UnityEngine;
using UnityEngine.InputSystem;

namespace Composition
{
    public class Card : Dragable
    {
        private bool isFaceUp = true;
        public midCardDrop currPile;
        private handManager cachedHand;

        public void DropCard(IDropArea cardDropArea)
        {
            cardDropArea.dropArea(this);
        }
        void Update()
        {

            if (colItem != null)
            {
                DropCard(colItem);
                colItem = null;
            }
        }
    }
}
