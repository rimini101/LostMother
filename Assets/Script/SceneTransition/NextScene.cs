using UnityEngine;

public class NextScene : MonoBehaviour
{
    public AudioClip endLevelSound; // Clip audio à jouer

    public string sceneToLoad; //nom de la scène demandée

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si le joueur entre dans la zone de fin
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(endLevelSound, transform.position);

            // Appelle le LevelManager pour charger la scène avec transition
            LevelManager.Instance.LoadScene(sceneToLoad, "CrossFade");
            Debug.Log("Nouvelle scene 1");
        }
    }
}
