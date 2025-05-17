using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening; // Pour la vibration

public class Card : MonoBehaviour
{
    public Sprite imageFace;     // Sprite pour la face cachée (image)
    public Sprite vowelFace;     // Sprite pour la face visible (voyelle)
    public string associatedVowel; // La voyelle associée à cette carte

    private SpriteRenderer spriteRenderer;
    private bool isFaceUp = false;
    private Collider cardCollider; // Référence au Collider

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cardCollider = GetComponent<Collider>(); // Récupérer le Collider (Sphere ou autre)

        if (spriteRenderer == null)
        {
            Debug.LogError("Le GameObject Carte doit avoir un composant SpriteRenderer !");
            enabled = false;
        }

        if (cardCollider == null)
        {
            Debug.LogError("Le GameObject Carte doit avoir un composant Collider !");
            enabled = false;
        }

        // Au départ, afficher la face cachée
        ShowImageFace();
    }

    void OnMouseDown()
    {
        if (!isFaceUp && GameManager.instance.CanFlipCard())
        {
            Debug.Log("Carte cliquée !");
            GameManager.instance.FlipCard(this); // Informer le GameManager du flip
        }
    }

    public void FlipCard(bool reveal)
    {
        isFaceUp = reveal;
        if (isFaceUp)
        {
            ShowVowelFace();
        }
        else
        {
            ShowImageFace();
        }
    }

    public void ShowImageFace()
    {
        if (spriteRenderer != null && imageFace != null)
        {
            spriteRenderer.sprite = imageFace;
        }
    }

    public void ShowVowelFace()
    {
        if (spriteRenderer != null && vowelFace != null)
        {
            spriteRenderer.sprite = vowelFace;
        }
    }

    public bool IsMatch(Card otherCard)
    {
        if (otherCard == null)
        {
            return false;
        }
        return associatedVowel == otherCard.associatedVowel;
    }

    public void DisableCard()
    {
        if (cardCollider != null)
        {
            cardCollider.enabled = false;
        }
    }

    public void EnableCard()
    {
        if (cardCollider != null)
        {
            cardCollider.enabled = true;
            ShowImageFace();
            isFaceUp = false;
        }
    }

    public void VibrateOnWrongMatch()
    {
        // Utilisation de DOTween pour une vibration visuelle simple
        float originalX = transform.position.x;
        float vibrateAmount = 0.1f;
        float vibrateDuration = 0.1f;
        int vibrateTimes = 3;

        Sequence vibrateSequence = DOTween.Sequence();
        for (int i = 0; i < vibrateTimes; i++)
        {
            vibrateSequence.Append(transform.DOMoveX(originalX + vibrateAmount, vibrateDuration / 2f));
            vibrateSequence.Append(transform.DOMoveX(originalX - vibrateAmount, vibrateDuration / 2f));
        }
        vibrateSequence.Append(transform.DOMoveX(originalX, vibrateDuration / 2f)); // Retour à la position originale
    }
}