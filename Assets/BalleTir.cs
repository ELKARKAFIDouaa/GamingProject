using UnityEngine;
using TMPro;

public class BalleTir : MonoBehaviour
{
    public float speed = 10f;
    private bool aTire = false;

    private Vector3 startPos;
    private Rigidbody2D rb;
    private GameManager_Consonnes gameManager;

    public TextMeshProUGUI scoreText;
    private int score = 0;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // anciennement isKinematic = true

        gameManager = FindObjectOfType<GameManager_Consonnes>();

        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();
    }

    void Update()
    {
        if (!aTire && Input.GetMouseButtonDown(0))
        {
            aTire = true;
            rb.bodyType = RigidbodyType2D.Dynamic; // anciennement isKinematic = false
            rb.linearVelocity = Vector2.up * speed;      // anciennement linearVelocity
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Consonne"))
        {
            string lettre = collision.gameObject.name;

            if (lettre == gameManager.consonneAttendue)
            {
                gameManager.Gagner();
                score++;
                UpdateScoreText();

                Destroy(collision.gameObject); // supprimer le nuage touché
            }
            else
            {
                gameManager.Perdu();
            }

            ResetBalle();
        }
    }

    void ResetBalle()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        transform.position = startPos;
        aTire = false;
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();
    }
}
