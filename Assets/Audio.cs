using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int score = 0;
    [Header("Cartes")]
    public GameObject cardPrefab;
    public Sprite imageFace;                                     // Image commune pour toutes les cartes (face cachée)
    public Sprite[] vowelFaces;                                 // Images pour les voyelles (face visible)
    public string[] vowels = { "a", "e", "i", "o", "u", "y" };
    public int numberOfPairs = 3;
    private int totalCardsToMatch;

    [Header("Spawn")]
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    [Header("Temps & Audio & Victoire & Défaite")]
    public float initialTimeLimit = 40f;
    private float currentTime;
    public float heartLossInterval = 10f; // Intervalle de perte de cœur en secondes
    private float lastHeartLossTime;
    public int initialHearts = 4;
    private int currentHearts;
    public AudioClip pairFoundClip;
    public AudioClip victoryClip;
    public AudioClip defeatClip;
    private AudioSource audioSource;
    public Dictionary<string, AudioClip> vowelSounds = new Dictionary<string, AudioClip>();
    public AudioClip sound_a;
    public AudioClip sound_e;
    public AudioClip sound_i;
    public AudioClip sound_o;
    public AudioClip sound_u;
    public AudioClip sound_y;
    public GameObject victoryImage;
    public GameObject defeatImage;
    public string islandSceneName = "IleDesVoyelles";

    [Header("UI")]
    
    public Image[] heartImages; // Tableau pour les images de cœur
    public TMP_Text scoreTextUI;
    public TMP_Text timerTextUI;

    private List<Card> allCards = new List<Card>();
    private List<Card> flippedCards = new List<Card>();
    private int matchedPairs = 0;
    private bool canFlip = true;
    private bool gameWon = false;
    private bool gameLost = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
        currentTime = initialTimeLimit;
        currentHearts = initialHearts;
        lastHeartLossTime = Time.time;
        totalCardsToMatch = numberOfPairs * 2;
        LoadVowelSounds();
        CreateCards();
        UpdateScoreUI();
        UpdateTimerUI();
        UpdateHeartsUI();

        if (victoryImage != null)
        {
            victoryImage.SetActive(false);
        }
        else
        {
            Debug.LogError("L'image de victoire n'est pas assignée dans l'Inspecteur !");
        }
        if (defeatImage != null)
        {
            defeatImage.SetActive(false);
        }
        else
        {
            Debug.LogError("L'image de défaite n'est pas assignée dans l'Inspecteur !");
        }

        // Assurez-vous que le tableau heartImages a la bonne taille
        if (heartImages.Length != initialHearts)
        {
            Debug.LogError("Le tableau heartImages n'a pas la taille de initialHearts dans l'Inspecteur !");
        }
    }

    void Update()
    {
        if (!gameWon && !gameLost)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();

            if (Time.time - lastHeartLossTime >= heartLossInterval && currentHearts > 0)
            {
                currentHearts--;
                UpdateHeartsUI();
                lastHeartLossTime = Time.time;
            }

            if (currentTime <= 0 || currentHearts <= 0)
            {
                gameLost = true;
                
                audioSource.PlayOneShot(defeatClip);
                if (defeatImage != null)
                {
                    defeatImage.SetActive(true);
                    Invoke(nameof(LoadIslandScene), 3f);
                }
                else
                {
                    Invoke(nameof(LoadIslandScene), 3f);
                }
            }

            if (matchedPairs * 2 == totalCardsToMatch && !gameWon)
            {
                gameWon = true;
                
                audioSource.PlayOneShot(victoryClip);
                if (victoryImage != null)
                {
                    victoryImage.SetActive(true);
                    Invoke(nameof(LoadIslandScene), 3f);
                }
                else
                {
                    Invoke(nameof(LoadIslandScene), 3f);
                }
            }
        }
    }

    void LoadVowelSounds()
    {
        vowelSounds.Add("a", sound_a);
        vowelSounds.Add("e", sound_e);
        vowelSounds.Add("i", sound_i);
        vowelSounds.Add("o", sound_o);
        vowelSounds.Add("u", sound_u);
        vowelSounds.Add("y", sound_y);
    }

    void CreateCards()
    {
        List<string> chosenVowels = vowels.Take(numberOfPairs).ToList();
        List<string> allVowels = new List<string>();

        foreach (string v in chosenVowels)
        {
            allVowels.Add(v);
            allVowels.Add(v);
        }

        allVowels = allVowels.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < allVowels.Count; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, GetRandomSpawnPosition(), Quaternion.identity);
            Card card = cardObj.GetComponent<Card>();

            string vowel = allVowels[i];
            int index = System.Array.IndexOf(vowels, vowel);

            card.associatedVowel = vowel;
            card.vowelFace = vowelFaces[index];
            card.imageFace = imageFace;
            card.ShowImageFace();

            allCards.Add(card);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        return new Vector3(x, y, 0f);
    }

    public bool CanFlipCard()
    {
        return canFlip && flippedCards.Count < 2 && !gameWon && !gameLost;
    }

    public void FlipCard(Card card)
    {
        if (!CanFlipCard()) return;

        card.FlipCard(true);
        flippedCards.Add(card);

        if (flippedCards.Count == 2)
        {
            canFlip = false;
            Invoke(nameof(CheckMatch), 1f);
        }
    }

    void UpdateScoreUI()
    {
        if (scoreTextUI != null)
        {
            scoreTextUI.text = $"Score: {score}";
        }
        else
        {
            Debug.LogError("Le texte UI du score n'est pas assigné dans l'Inspecteur !");
        }
    }

    void UpdateTimerUI()
    {
        if (timerTextUI != null)
        {
            timerTextUI.text = $"Temps: {Mathf.CeilToInt(currentTime)}s";
        }
        else
        {
            Debug.LogError("Le texte UI du timer n'est pas assigné dans l'Inspecteur !");
        }
    }

    void UpdateHeartsUI()
    {
        if (heartImages != null && heartImages.Length == initialHearts)
        {
            for (int i = 0; i < heartImages.Length; i++)
            {
                heartImages[i].gameObject.SetActive(i < currentHearts);
            }
        }
        else
        {
            Debug.LogError("Le tableau heartImages n'est pas correctement configuré dans l'Inspecteur !");
        }
    }

    void CheckMatch()
    {
        if (flippedCards[0].IsMatch(flippedCards[1]))
        {
            PlayVowelSound(flippedCards[0].associatedVowel);
            flippedCards[0].DisableCard();
            flippedCards[1].DisableCard();
            matchedPairs++;
            score++;
            UpdateScoreUI();
        }
        else
        {
            flippedCards[0].VibrateOnWrongMatch();
            flippedCards[1].VibrateOnWrongMatch();
            flippedCards[0].FlipCard(false);
            flippedCards[1].FlipCard(false);
        }

        flippedCards.Clear();
        canFlip = true;
    }

    void PlayVowelSound(string vowel)
    {
        if (vowelSounds.ContainsKey(vowel) && audioSource != null)
        {
            audioSource.PlayOneShot(vowelSounds[vowel]);
        }
        else
        {
            Debug.LogWarning("Son non trouvé pour la voyelle : " + vowel);
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        currentTime = initialTimeLimit;
        currentHearts = initialHearts;
        lastHeartLossTime = Time.time;
        UpdateTimerUI();
        UpdateHeartsUI();
        score = 0;
        UpdateScoreUI();
        matchedPairs = 0;
        gameWon = false;
        gameLost = false;
        foreach (Card card in allCards)
        {
            card.EnableCard();
        }
        
        if (defeatImage != null)
        {
            defeatImage.SetActive(false);
        }
    }

    public void LoadIslandScene()
    {
        SceneManager.LoadScene(islandSceneName);
    }
}