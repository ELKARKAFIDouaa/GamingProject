using UnityEngine;
using UnityEngine.SceneManagement;

public class retour : MonoBehaviour
{
    public string sceneToLoad; // Nom de la scène à charger

    private void OnMouseDown()
    {
        // Vérifie si le nom de la scène à charger est renseigné
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.Log("Clic sur la roche. Chargement de la scène : " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Le nom de la scène à charger n'est pas défini sur l'objet : " + gameObject.name);
        }
    }
}
