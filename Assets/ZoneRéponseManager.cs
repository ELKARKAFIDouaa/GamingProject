using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;  // Pour utiliser UI Button

public class ZoneRéponsesManager : MonoBehaviour {
    public MotManager motManager;  // Référence au MotManager pour accéder au mot cible
    public GameObject boutonPrefab;  // Référence au prefab du bouton de syllabe
    public Transform zoneRéponses;  // Zone où les boutons de syllabes seront instanciés
    public ZoneConstructionManager zoneConstructionManager;

    private List<string> syllabesActuelles = new List<string>();  // Liste des syllabes à afficher
    private List<string> syllabesCorrectes;  // Liste des syllabes correctes du mot cible

    void Start() {
        // Remplir la liste des syllabes et les afficher
        syllabesCorrectes = motManager.mots[motManager.indexMot].syllabesCorrectes;  // Exemple : syllabes correctes du mot actuel
        syllabesActuelles.AddRange(syllabesCorrectes);  // Ajouter les syllabes correctes
        syllabesActuelles.AddRange(motManager.mots[motManager.indexMot].syllabesLeurres);  // Ajouter des syllabes incorrectes
        AfficherSyllabes();
    }

    public void AfficherSyllabes() {
        // Mélanger les syllabes pour ajouter du challenge
        Shuffle(syllabesActuelles);
        
        // Instancier un bouton pour chaque syllabe
        foreach (string syllabe in syllabesActuelles) {
            GameObject bouton = Instantiate(boutonPrefab, zoneRéponses);  // Créer un bouton
            bouton.GetComponentInChildren<TextMeshProUGUI>().text = syllabe;  // Afficher la syllabe sur le bouton

            // Ajouter l'écouteur d'événements pour chaque bouton
            bouton.GetComponent<Button>().onClick.AddListener(() => VerifierSyllabe(syllabe));
            
        }
    }


    void VerifierSyllabe(string syllabe) {
    if (syllabesCorrectes.Contains(syllabe)) {
        Debug.Log("Syllabe correcte : " + syllabe);
        zoneConstructionManager.AjouterSyllabe(syllabe);
    } else {
        Debug.Log("Syllabe incorrecte : " + syllabe);
    }
}


    // Méthode pour mélanger la liste des syllabes
    void Shuffle(List<string> list) {
        for (int i = 0; i < list.Count; i++) {
            string temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
} 