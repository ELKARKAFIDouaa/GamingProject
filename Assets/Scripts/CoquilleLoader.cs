using UnityEngine;
using UnityEngine.SceneManagement;

public class CoquilleLoader : MonoBehaviour
{
    private void OnMouseDown()
    {
        
        SceneManager.LoadScene("MiniJeuConsonnes");
    }
}
