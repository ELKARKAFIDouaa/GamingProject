using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager_Consonnes : MonoBehaviour
{
    public List<AudioClip> consonneClips; // Sons : C, D, G, L, M ,B, K
    public AudioClip winSound; // Son de victoire
    public AudioClip loseSound; // Son de perte
    public AudioSource audioSource;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timerText;

    public GameObject winImage;
    public GameObject loseImage;
    public GameObject gameOverPanel;

    [HideInInspector]
    public string consonneAttendue;

    private List<int> consonneIndicesRestants;
    private List<string> consonnesDisponibles = new List<string> { "C", "D", "G", "L", "M","B", "K" };

    private int score = 0;
    private int lives = 5;
    private bool canPlaySound = true;
    private bool jeuTermine = false;
    private bool jeuCommence = false;

    public float tempsLimite = 40f;
    private float timer;

    void Start()
    {
        UpdateUI();
        timer = tempsLimite;

        // Initialise la liste des indices disponibles
        consonneIndicesRestants = new List<int>();
        for (int i = 0; i < consonneClips.Count; i++)
        {
            consonneIndicesRestants.Add(i);
        }

        winImage.SetActive(false);
        loseImage.SetActive(false);
        gameOverPanel.SetActive(false);

        ChoisirNouvelleConsonne();
    }

    void Update()
    {
        if (jeuTermine || !jeuCommence) return;

    timer -= Time.deltaTime;

    if (timer <= 0f)
    {
        TerminerJeuPerdu();
        return; // ⚠️ Ajoute ce return pour éviter que d'autres choses s'exécutent
    }

    timerText.text = "Temps : " + Mathf.CeilToInt(timer) + "s";
    }

    public void ChoisirNouvelleConsonne()
    {
        if (!canPlaySound || consonneIndicesRestants.Count == 0) return;

        int index = Random.Range(0, consonneIndicesRestants.Count);
        int clipIndex = consonneIndicesRestants[index];

        consonneAttendue = consonnesDisponibles[clipIndex].Trim().ToUpper();
        audioSource.clip = consonneClips[clipIndex];
        audioSource.Play();

        canPlaySound = false;

        // ✅ Lance le timer seulement après la première consonne
        if (!jeuCommence)
        {
            jeuCommence = true;
        }

        Debug.Log("✅ Nouvelle consonne attendue : " + consonneAttendue);
    }

    public void Gagner()
    {
        score++;
        UpdateUI();
        canPlaySound = true;

        // ✅ Supprimer la consonne uniquement après réussite
        int indexASupprimer = consonnesDisponibles.FindIndex(c => c == consonneAttendue);
        if (indexASupprimer >= 0)
        {
            consonneIndicesRestants.Remove(indexASupprimer);
        }

        if (score >= 7)
        {
            TerminerJeuGagne();
        }
        else
        {
            Invoke("ChoisirNouvelleConsonne", 1f);
        }

        Debug.Log("🎉 Bonne réponse ! Score: " + score);
    }

    public void Perdu()
    {
        canPlaySound = true;

        if (!jeuTermine)
        {
            Invoke("ChoisirNouvelleConsonne", 1f);
        }

        Debug.Log("❌ Mauvaise réponse !");
    }

    void TerminerJeuGagne()
{
    if (jeuTermine) return; // ✅ éviter double appel
    jeuTermine = true;

    audioSource.PlayOneShot(winSound);
    winImage.SetActive(true);
    
    Debug.Log("🏆 VICTOIRE FINALE !");
}

void TerminerJeuPerdu()
{
    if (jeuTermine) return; // ✅ éviter double appel
    jeuTermine = true;

    audioSource.PlayOneShot(loseSound);
    loseImage.SetActive(true);
    gameOverPanel.SetActive(true);
    Debug.Log("💥 TEMPS ÉCOULÉ : GAME OVER");
}


    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;
    }

    void HideWinImage() => winImage.SetActive(false);
    void HideLoseImage() => loseImage.SetActive(false);

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
