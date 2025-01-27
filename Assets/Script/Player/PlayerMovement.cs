using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    Rigidbody rb;

    float xInput;

    public float moveForce = 100; // Force de mouvement, force appliquée pour avancer
    public float maxSpeed = 6;  // Vitesse maximale
    public float stopForce = 0.2f; // Force de freinage
    public float jumpForce = 25; // Force de saut
    public float groundDistance = 1.3f; // Distance au sol (Variable pour accepter le saut)
    public float rotationSpeed = 10f; // Vitesse à laquelle le personnage se retourne

    private bool isGrounded; //verification si le player est sous la distance au sol
    private Vector3 targetDirection; // La direction cible (droite ou gauche)


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        xInput = Input.GetAxis("Horizontal");

        // Gestion du saut (si on clique sur space et que isGrounded est respecté alors on applique une force sur "up" )
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        // Si l'input n'est pas à nul
        if (xInput != 0)
        {
            // on ajoute une force sur la trajectoire horizontale
            rb.AddForce(xInput * moveForce, 0, 0);

            // on gère la rotation selon l'input horizontal, si positif -> a droite, si négatif -> à gauche
            float targetAngle = xInput > 0 ? 0f : 180f;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);    // rotation fluide avec Slerp + vitesse déterminée par rotationSpeed
        }
        else if (xInput == 0)
        {
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, 0);
            // Réduit la vitesse horizontale lorsqu'il n'y a pas d'input(de touches cliquées) = gère le freinage
            rb.AddForce(-horizontalVelocity * moveForce * stopForce); // Ajuste stopForce pour augmenter ou réduire le freinage

            // Arrêter toute rotation lorsque l'input est nul
            rb.angularVelocity = Vector3.zero; 

        }

        // Limite la vitesse horizontale (sur l'axe X) à maxSpeed
        if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed)
        {
            rb.linearVelocity = new Vector3(Mathf.Sign(rb.linearVelocity.x) * maxSpeed, rb.linearVelocity.y, rb.linearVelocity.z);
        }

        //vérifie si le perso est sous la groundDistance (voir le if de update)
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance);

        // Debug.Log($"Vitesse du perso: {rb.linearVelocity.magnitude} unités/s (Vecteur vitesse : {rb.linearVelocity})");

    }

}
