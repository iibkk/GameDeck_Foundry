using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public TMP_Text cardNameText;
    public TMP_Text cardText;
    public SpriteRenderer frontImage;
    public SpriteRenderer backImage;
    public SpriteRenderer cardTextBackground;
    public SpriteRenderer cardBackground;
    public string frontImageUrl;
    public string backImageUrl;
    private bool isFaceUp = true;

    public bool IsFaceUp()
    {
        return isFaceUp;
    }

    public void SetFaceUp(bool faceUp)
    {
        isFaceUp = faceUp;
        bool hasFront = !string.IsNullOrEmpty(frontImageUrl) && frontImageUrl.StartsWith("http");

        if (frontImage != null)
            frontImage.gameObject.SetActive(faceUp && hasFront);

        if (backImage != null)
            backImage.gameObject.SetActive(!faceUp); // always show Unity back sprite

        if (cardNameText != null)
            cardNameText.gameObject.SetActive(faceUp);

        if (cardText != null)
            cardText.gameObject.SetActive(faceUp);

        if (cardTextBackground != null)
            cardTextBackground.gameObject.SetActive(faceUp);

        SetSortingOrder(0);
    }

    public void SetCardInfo(string name, string text, string frontUrl = "", string backUrl = "")
    {
        frontImageUrl = frontUrl;
        backImageUrl = backUrl;

        if (cardNameText != null)
            cardNameText.text = name;

        if (cardText != null)
            cardText.text = text;
        if (cardTextBackground != null)
        {
            cardTextBackground.gameObject.SetActive(true);

            if (string.IsNullOrEmpty(text))
                cardTextBackground.gameObject.SetActive(false);
        }
        if (cardBackground != null)
        {
            cardBackground.gameObject.SetActive(true);
            cardBackground.sortingLayerName = "Default";
            cardBackground.sortingOrder = 0;
        }

        MakeTextOnTop();

        if (frontImage != null)
            frontImage.gameObject.SetActive(false);

        if (backImage != null)
            backImage.gameObject.SetActive(false);

        if (!string.IsNullOrEmpty(frontUrl) && frontImage != null)
            StartCoroutine(LoadImage(frontUrl, frontImage));

        //if (!string.IsNullOrEmpty(backUrl) && backImage != null)
        //StartCoroutine(LoadImage(backUrl, backImage));
        SetFaceUp(true);
    }
    IEnumerator LoadImage(string url, SpriteRenderer target)
    {
        if (target == null)
            yield break;

        if (string.IsNullOrEmpty(url) || !url.StartsWith("http"))
        {
            target.gameObject.SetActive(false);
            yield break;
        }

        target.gameObject.SetActive(true);

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Image load failed: " + request.error);
            target.gameObject.SetActive(false);
            yield break;
        }

        Texture2D texture = DownloadHandlerTexture.GetContent(request);

        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            100f
        );

        target.sprite = sprite;
        target.gameObject.SetActive(true);

        target.sortingLayerName = "Default";
        target.sortingOrder = 1;

        // image area below card name
        target.transform.localPosition = new Vector3(0f, 0.4f, 0f);

        float imageWidth = 0.75f;
        float imageHeight = 0.8f;

        float spriteWidth = sprite.bounds.size.x;
        float spriteHeight = sprite.bounds.size.y;

        float scale = Mathf.Min(
            imageWidth / spriteWidth,
            imageHeight / spriteHeight
        );

        target.transform.localScale = new Vector3(scale, scale, 1f);

        target.gameObject.SetActive(false);
        target.gameObject.SetActive(true);

        CardDisplay display = GetComponent<CardDisplay>();
        if (display != null)
        {
            display.SetSortingOrder(0);
        }
    }

    // Keep this because handManager still calls SetCardValue(randomValue)
    public void SetCardValue(int value)
    {
        SetCardInfo("Card " + value, "Value: " + value);
    }
    void MakeTextOnTop()
    {
        MeshRenderer[] texts = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer text in texts)
        {
            text.sortingLayerName = "Default";
            text.sortingOrder = 20;
        }
    }

    public void SetSortingOrder(int order)
    {
        if (cardBackground != null)
            cardBackground.sortingOrder = order;

        if (frontImage != null)
            frontImage.sortingOrder = order + 1;

        if (backImage != null)
            backImage.sortingOrder = order + 1;

        if (cardTextBackground != null)
            cardTextBackground.sortingOrder = order + 2;

        MeshRenderer[] texts = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer text in texts)
        {
            text.sortingOrder = order + 10;
        }
    }
    public string GetCardName()
    {
        return cardNameText != null ? cardNameText.text : "card";
    }
    public string GetCardText()
    {
        return cardText != null ? cardText.text : "";
    }

}