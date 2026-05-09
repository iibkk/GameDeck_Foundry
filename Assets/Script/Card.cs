using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace Composition
{
    public class Card : Dragable
    {
        private bool isFaceUp = true;
        public midCardDrop currPile;
        private handManager cachedHand;

        [SerializeField] private TextMeshPro numberText;
        [SerializeField] private Sprite cardFront;
        [SerializeField] private Sprite cardBack;

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


        public void setCardFacing(bool facing)
        {
            isFaceUp = facing;

            if (isFaceUp)
            {
                numberText.fontSize = 10;
                GetComponent<SpriteRenderer>().sprite = cardFront;
            }
            else
            {
                numberText.fontSize = 0;
                GetComponent<SpriteRenderer>().sprite = cardBack;
            }
        }
    }
}
