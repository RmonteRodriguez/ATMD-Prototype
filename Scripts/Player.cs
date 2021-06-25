using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPunCallbacks
{
    private Manager manager;

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
        manager = GameObject.Find("Game Manager").GetComponent<Manager>();

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        if (currentHealth <= 0)
        {
            Death();
        }

        //Controls
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            photonView.RPC("PistolDamage", RpcTarget.All);
        }
    }

    //Guns Damage
    [PunRPC]
    public void PistolDamage()
    {
        currentHealth = currentHealth - 2f;
    }

    public void Death()
    {
        manager.Spawn();
        PhotonNetwork.Destroy(gameObject);
    }
}
