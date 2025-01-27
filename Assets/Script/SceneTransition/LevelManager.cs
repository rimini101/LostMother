using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public Slider progressBar;

    //reference to the transition container
    public GameObject transitionsContainer;

    //array of scene transitions
    private SceneTransition[] transitions;


    private void Awake()
    {
        //On transforme l'instance en Singleton pour qu'il n'existe qu'une instance et qu'elle soit accessible de partout :
        
        // Si l'instance est vide, on affecte la référence et on empêche la suppression de l'objet entre les scènes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // Si l'instance existe déjà, on détruit l'objet dupliqué
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // On récupère toutes les transitions disponibles dans le conteneur de transitions (on)
        transitions = transitionsContainer.GetComponentsInChildren<SceneTransition>();
    }

    // Méthode publique pour charger une scène avec un nom de scène et un nom de transition spécifique
    public void LoadScene(string sceneName, string transitionName)
    {
        StartCoroutine(LoadSceneAsync(sceneName, transitionName));
    }

    private IEnumerator LoadSceneAsync(string sceneName, string transitionName)
    {
        //cherche la premiere transition avec le nom de transition demandé
        SceneTransition transition = transitions.First(t => t.name == transitionName);

        // Lance le chargement asynchrone de la scène
        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        yield return transition.AnimateTransitionIn();

        progressBar.gameObject.SetActive(true);

        // Mise à jour continue de la barre de progression pendant le chargement de la scène
        do
        {
            progressBar.value = scene.progress;
            yield return null;
        } while (scene.progress < 0.9f);

        // Petite pause pour laisser le temps à l'animation de transition avant d'activer la scène
        yield return new WaitForSeconds(1f);

        // Permet d'activer la scène une fois le chargement terminé
        scene.allowSceneActivation = true;

        // Cache la barre de progression une fois le chargement terminé
        progressBar.gameObject.SetActive(false);

        // Anime la sortie de la transition après que la scène soit activée
        yield return transition.AnimateTransitionOut();
    }
}
