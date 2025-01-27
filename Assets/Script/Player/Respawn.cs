using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Vector3 lastCheckpointCoord;
    [SerializeField] float dead;

    void Start(){
        // coordonnées de checkpoint par défaut = zone de spaw initial du player 
        lastCheckpointCoord = player.transform.position;
    }

    // une fois entré dans le trigger
    void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Checkpoint"))
        {

            //Vecteur xyz de chaque objects avec le tag checkpoint possédant un tigger
            Vector3 checkpointPosition = other.gameObject.transform.position;

            // la variabe lastCheckpointCoord prend la position x du checkpoint et y et z du player quand il entre dans le trigger
            lastCheckpointCoord = new Vector3(checkpointPosition.x, player.transform.position.y, player.transform.position.z);
            
            //Debug.Log($"Coordonnées du checkpoint enregistrées : {lastCheckpointCoord}");

            // le trigger du checkpoint est supprimé
            other.GetComponent<Collider>().enabled = false;

        }
        else if (other.CompareTag("death"))
        {
            RespawnCheckpoint();
            //Debug.Log("(Non je soui mort :( ) zone de respawn");
        }
        else
        {
            Debug.Log("rien");
        }
    }

    public void RespawnCheckpoint()
    {
        // La rotation est reset pour ne pas garder la rotation de la chute
        player.transform.rotation = Quaternion.identity;

        // Le player prend la position des dernières coordonnées de checkpoint
        player.transform.position = lastCheckpointCoord;
       
        // player.transform.position = new Vector3(-4, 2, 19);
        Debug.Log(player.transform.position);
        Debug.Log("Yay i'm alive" + lastCheckpointCoord);
        
    }


}
