using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ZoneConstructionManager : MonoBehaviour
{
    public GameObject syllabePrefab; // Le même prefab de bouton que pour la zone de réponses
    public MotManager motManager; // Référence au MotManager pour la logique de vérification et de suppression

    private List<GameObject> syllabesAffichees = new List<GameObject>(); // Garde une trace des boutons affichés

    // Fonction appelée par le MotManager lorsqu'une syllabe est sélectionnée
    public void AjouterSyllabe(string syllabe)
    {
        GameObject nouveauBouton = Instantiate(syllabePrefab, transform); // 'transform' référence la zoneConstruction
        nouveauBouton.GetComponentInChildren<TextMeshProUGUI>().text = syllabe;

        // Ajouter un listener pour permettre de supprimer la syllabe en cliquant dessus
        Button boutonComponent = nouveauBouton.GetComponent<Button>();
        if (boutonComponent != null)
        {
            string syllabeASupprimer = syllabe; // Créer une copie locale pour la closure
            boutonComponent.onClick.AddListener(() => SupprimerSyllabe(syllabeASupprimer, nouveauBouton));
        }

        syllabesAffichees.Add(nouveauBouton);
    }

    // Fonction pour supprimer une syllabe de la zone de construction
    public void SupprimerSyllabe(string syllabeASupprimer, GameObject boutonASupprimer)
    {
        if (syllabesAffichees.Contains(boutonASupprimer))
        {
            syllabesAffichees.Remove(boutonASupprimer);
            motManager.SupprimerSyllabeSelectionnee(syllabeASupprimer); // Informer le MotManager
            Destroy(boutonASupprimer);
            // Réorganiser visuellement les syllabes restantes si nécessaire (LayoutGroup le fera généralement)
        }
    }

    // Fonction pour vider la zone de construction (appelée lors du chargement d'un nouveau mot)
    public void ViderZone()
    {
        foreach (GameObject bouton in syllabesAffichees)
        {
            Destroy(bouton);
        }
        syllabesAffichees.Clear();
    }

    // Fonction pour obtenir le mot reconstruit par le joueur
    public string GetMotConstruit()
    {
        string motConstruit = "";
        foreach (GameObject bouton in syllabesAffichees)
        {
            motConstruit += bouton.GetComponentInChildren<TextMeshProUGUI>().text;
        }
        return motConstruit;
    }
} 