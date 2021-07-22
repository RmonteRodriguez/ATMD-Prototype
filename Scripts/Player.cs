using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    //UI
    private Transform UIHealthbar;
    private Text healthbarText;

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine) return;

        UIHealthbar = GameObject.Find("HUD/Health/Bar").transform;
        healthbarText = GameObject.Find("HUD/Health/Text").GetComponent<Text>();

        cameraParent.SetActive(photonView.IsMine);
        manager = GameObject.Find("Game Manager").GetComponent<Manager>();

        currentHealth = maxHealth;
        photonView.RPC("RefreshHealthBar", RpcTarget.All);
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        healthbarText.text = currentHealth.ToString();

        if (currentHealth <= 0)
        {
            Death();
        }

        //Controls
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        bool pause = Input.GetKeyDown(KeyCode.Escape);

        //Pause
        if (pause)
        {
            GameObject.Find("Pause").GetComponent<Pause>().TogglePause();
        }

        if (Pause.paused)
        {
            movement.x = 0f;
            movement.y = 0f;
        }
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

        if (collision.gameObject.tag == "SniperBullet")
        {
            photonView.RPC("SniperDamage", RpcTarget.All);
        }

        if (collision.gameObject.tag == "RifleBullet")
        {
            photonView.RPC("RifleDamage", RpcTarget.All);
        }
    }

    [PunRPC]
    public void RefreshHealthBar()
    {
        float t_healthRatio = currentHealth / maxHealth;
        UIHealthbar.localScale = new Vector3(t_healthRatio, 1, 1);
    }

    //Guns Damage
    [PunRPC]
    public void PistolDamage()
    {
        currentHealth = currentHealth - 2f;

        photonView.RPC("RefreshHealthBar", RpcTarget.All);
        CameraShake.Instance.ShakeCamera(.1f, .1f);
    }

    [PunRPC]
    public void SniperDamage()
    {
        currentHealth = currentHealth - 25f;

        photonView.RPC("RefreshHealthBar", RpcTarget.All);
        CameraShake.Instance.ShakeCamera(.1f, .1f);
    }

    [PunRPC]
    public void RifleDamage()
    {
        currentHealth = currentHealth - 5f;

        photonView.RPC("RefreshHealthBar", RpcTarget.All);
        CameraShake.Instance.ShakeCamera(.1f, .1f);
    }

    public void Death()
    {
        manager.Spawn();
        PhotonNetwork.Destroy(gameObject);
    }
}
