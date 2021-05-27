using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{

    public Card card;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public Image artworkImage;
    public Image emotionImage;

    void Start()
    {
        SetUpDisplay();

        StartCoroutine(OnFirsFrame());
    }

    // ExecuteSpawnActions Needed to be called after all components are instanciated 
    private IEnumerator OnFirsFrame()
    {
        yield return new WaitForFixedUpdate();
        card.ExecuteSpawnActions();
    }

    public void UpdateCardData(Card newCardData)
    {
        card = newCardData;
        SetUpDisplay();
    }

    private void SetUpDisplay()
    {
        nameText.SetText(card.person.personName);
        descriptionText.SetText(card.person.description);
        artworkImage.sprite = card.person.artwork;
        artworkImage.SetAllDirty();
        if (card.cardEmotion == null)
        {
            var startColor = emotionImage.color;
            startColor.a = 0.0f;
            emotionImage.color = startColor;
        } else
        {
            var startColor = emotionImage.color;
            startColor.a = 1.0f;
            emotionImage.color = startColor;

            emotionImage.sprite = card.cardEmotion.emotionSprite;
            emotionImage.SetAllDirty();
        }
       
        SetCardInfoColor();
    }

    private void SetCardInfoColor()
    {
        if (card.cardInfoColor != null)
        {
            Renderer rend = GetComponent<Renderer>();
            Material[] materials = rend.materials;
            int changableIndex = 0;
            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].name.Contains(CardColor.changableShaderName))
                {
                    changableIndex = i;
                    break;
                }
            }

            materials[changableIndex].SetColor(CardColor.parameterName, card.cardInfoColor.cardColor);
            rend.materials = materials;
        }
    }
}
