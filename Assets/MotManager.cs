using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;  // Nécessaire pour utiliser les éléments UI comme Button et Text

[System.Serializable]
public class MotData {
    public string nom;   // Le nom du mot
    public Sprite image; // L'image associée au mot
    public List<string> syllabesCorrectes; // Liste des syllabes correctes
    public List<string> syllabesLeurres;   // Liste des syllabes incorrectes (leurres)
}

public class MotManager : MonoBehaviour {
    public List<MotData> mots;         // Liste des mots (à remplir dans l'inspecteur)
    public Image imageMotUI;         // Image UI qui affichera l'image du mot actuel
    public Transform zoneRéponses;    // Zone UI contenant les boutons de syllabes
    public Transform zoneConstruction; // Zone où les syllabes sélectionnées seront affichées
    public GameObject syllabePrefab;  // Préfabriqué pour chaque syllabe (button ou text)
    // private List<string> syllabesSelectionnees = new List<string>(); // Plus nécessaire, géré par ZoneConstructionManager
    public ZoneConstructionManager zoneConstructionManager; // Référence au ZoneConstructionManager
    public ZoneRéponsesManager zoneRéponsesManager;     // Référence au ZoneRéponsesManager
    public Button boutonValider;
    public int tentativesMax = 2;
    private int tentativesRestantes;
    private MotData motActuel;
    public int indexMot = 0;

    void Start() {
        // Charger le premier mot au début du jeu
        ChargerMot(indexMot);
        boutonValider.onClick.AddListener(VerifierMotFinal);
    }

    // Fonction pour charger un mot donné par son index
    void ChargerMot(int index) {
        if (index >= 0 && index < mots.Count) {
            motActuel = mots[index];
            imageMotUI.sprite = motActuel.image;
            tentativesRestantes = tentativesMax;

            // Vider la zone de construction via le manager dédié
            if (zoneConstructionManager != null) {
                zoneConstructionManager.ViderZone();
            } else {
                Debug.LogError("ZoneConstructionManager non assigné dans MotManager !");
            }

            // Afficher les syllabes dans la zone de réponses via le manager dédié
            if (zoneRéponsesManager != null) {
                //zoneRéponsesManager.AfficherSyllabes(); // Ne pas passer d'argument
            } else {
                Debug.LogError("ZoneRéponsesManager non assigné dans MotManager !");
            }
        } else {
            Debug.Log("Fin de la liste des mots !");
            // Gérer la fin de la liste des mots (fin du niveau ?)
        }
    }

    // Cette fonction n'est plus directement appelée par les boutons de la zone de réponse
    public void SyllabeSelectionnee(string syllabe) {
        // La logique est maintenant gérée par ZoneRéponsesManager qui appelle AjouterSyllabe de ZoneConstructionManager
        if (zoneConstructionManager != null) {
            zoneConstructionManager.AjouterSyllabe(syllabe);
        }
    }

    // Fonction pour supprimer une syllabe sélectionnée (appelée par ZoneConstructionManager)
    public void SupprimerSyllabeSelectionnee(string syllabeASupprimer) {
        // Si vous avez encore besoin de suivre les syllabes sélectionnées ici, faites-le.
        // Sinon, la logique est principalement dans ZoneConstructionManager.
        // if (syllabesSelectionnees.Contains(syllabeASupprimer)) {
        //     syllabesSelectionnees.Remove(syllabeASupprimer);
        // }
    }

    // Fonction pour vérifier si le mot reconstruit est correct
    void VerifierMotFinal() {
        if (zoneConstructionManager != null) {
            string motReconstruit = zoneConstructionManager.GetMotConstruit();
            if (motReconstruit == motActuel.nom) {
                Debug.Log("Mot correct !");
                PasserAuMotSuivant();
            } else {
                tentativesRestantes--;
                Debug.Log("Erreur ! Tentatives restantes : " + tentativesRestantes);
                // Ajouter ici un retour visuel à l'erreur

                if (tentativesRestantes <= 0) {
                    Debug.Log("Plus de tentatives pour ce mot. La réponse était : " + motActuel.nom);
                    PasserAuMotSuivant(); // Ou afficher la réponse et proposer de réessayer
                }
            }
        } else {
            Debug.LogError("ZoneConstructionManager non assigné dans MotManager pour la vérification !");
        }
    }

    // Exemple de méthode pour passer au mot suivant
    public void PasserAuMotSuivant() {
    indexMot++;
    if (indexMot >= mots.Count) {
        // Réinitialiser l'index si on a atteint la fin de la liste de mots.
        indexMot = 0;
    }

    // Vider la zone de construction et afficher les nouvelles syllabes
    zoneConstructionManager.ViderZone();
    ChargerMot(indexMot);

    // Afficher les syllabes dans la zone de réponses
    zoneRéponsesManager.AfficherSyllabes();
}

}