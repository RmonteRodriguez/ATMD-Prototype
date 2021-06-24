using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPunCallbacks
{
    public Rigidbody2D rb;

    private Vector2 movement;

    public GameObject cameraParent;

    public float moveSpeed;
    public float maxHealth;
    public float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        cameraParent.SetActive(photonView.IsMine);
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        //Controls
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
