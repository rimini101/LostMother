using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    //Singleton so that there only exists one single instance of this class and is accessible from anywhere
    public static LevelManager Instance;

    public Slider progressBar;

    //reference to the transition container
    public GameObject transitionsContainer;

    //array of scene transitions
    private SceneTransition[] transitions;
    private void Awake()
    {
        //Si c'est libre on prend et on fait en sorte que ce soit pas supprier quand on change de scene
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //autrement si ce n'est pas null, c'est que l'instance etait déjà en place et que du cup on peut le supprimer
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //get all the transitions
        transitions = transitionsContainer.GetComponentsInChildren<SceneTransition>();
    }
    public void LoadScene(string sceneName, string transitionName)
    {
        StartCoroutine(LoadSceneAsync(sceneName, transitionName));
    }

    private IEnumerator LoadSceneAsync(string sceneName, string transitionName)
    {
        //cherche la premiere transition avec le nom de transition demandé
        SceneTransition transition = transitions.First(t => t.name == transitionName);

        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        yield return transition.AnimateTransitionIn();

        progressBar.gameObject.SetActive(true);

        do
        {
            progressBar.value = scene.progress;
            yield return null;
        } while (scene.progress < 0.9f);

        yield return new WaitForSeconds(1f);

        scene.allowSceneActivation = true;

        progressBar.gameObject.SetActive(false);

        yield return transition.AnimateTransitionOut();
    }
}
