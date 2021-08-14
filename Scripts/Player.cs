using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Player : MonoBehaviourPunCallbacks
{
    public Manager manager;

    public Rigidbody2D rb;

    private Vector2 movement;

    public GameObject cameraParent;

    public float moveSpeed;
    public float maxHealth;
    public float currentHealth;

    public GameObject spawnPoints;

    public GameObject player;
    public Transform playerLocation;

    //Shop
    public int money;
    private bool shopOpen;

    //UI
    private Transform UIHealthbar;
    private Text healthbarText;
    private Text UIUsername;
    private Text moneyText;
    public GameObject HUD;
    public GameObject ShopUI;

    //Guns
    public GameObject pistol;
    public GameObject rifle;
    public GameObject machineGun;

    //Teams
    public bool isBlue;
    public bool isRed;
    public GameObject redChar;
    public GameObject blueChar;
    public GameObject noTeamChar;

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine) return;

        cameraParent.SetActive(photonView.IsMine);
        manager = GameObject.Find("Game Manager").GetComponent<Manager>();
        spawnPoints = GameObject.FindGameObjectWithTag("SpawnPoint");

        if (Pause.paused == true)
        {
            GameObject.Find("Pause").GetComponent<Pause>().TogglePause();
        }

        //UI
        UIHealthbar = GameObject.Find("HUD/Health/Bar").transform;
        healthbarText = GameObject.Find("HUD/Health/Text").GetComponent<Text>();
        UIUsername = GameObject.Find("HUD/Username/Text").GetComponent<Text>();
        UIUsername.text = Launcher.myProfile.username;
        //moneyText = GameObject.Find("ShopUI/Money").GetComponent<Text>();
        HUD = GameObject.FindGameObjectWithTag("HUD");
        ShopUI = GameObject.FindGameObjectWithTag("Shop");

        currentHealth = maxHealth;
        photonView.RPC("RefreshHealthBar", RpcTarget.All);

        ShopUI.SetActive(false);
        shopOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        //UI
        healthbarText.text = currentHealth.ToString();
        //moneyText.text = money.ToString();

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

        
        //Shop
        if (Input.GetKeyDown(KeyCode.Q) && shopOpen == false)
        {
            HUD.SetActive(false);
            ShopUI.SetActive(true);
            shopOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && shopOpen == true)
        {
            HUD.SetActive(true);
            ShopUI.SetActive(false);
            shopOpen = false;
        }

       
        if (Input.GetKeyDown(KeyCode.Alpha1) && shopOpen == true && money > 0)
        {
            photonView.RPC("PurchasePistol", RpcTarget.All);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && shopOpen == true && money > 10)
        {
            photonView.RPC("PurchaseRifle", RpcTarget.All);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && shopOpen == true && money > 20)
        {
            photonView.RPC("PurchaseMachineGun", RpcTarget.All);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            money = 100;
            Death();
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "RedTrigger")
        {
            photonView.RPC("JoinRedTeam", RpcTarget.All);
        }

        if (other.gameObject.tag == "BlueTrigger")
        {
            photonView.RPC("JoinBlueTeam", RpcTarget.All);
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
        photonView.RPC("DropDiffuser", RpcTarget.All);
        player.transform.position = spawnPoints.transform.position;
        currentHealth = maxHealth;
        RefreshHealthBar();
    }

    //Purchase Weapons
    [PunRPC]
    public void PurchasePistol()
    {
        money = money - 10;
        pistol.SetActive(true);
        rifle.SetActive(false);
        machineGun.SetActive(false);
    }

    [PunRPC]
    public void PurchaseRifle()
    {
        money = money - 20;
        pistol.SetActive(false);
        rifle.SetActive(true);
        machineGun.SetActive(false);
    }

    [PunRPC]
    public void PurchaseMachineGun()
    {
        money = money - 30;
        pistol.SetActive(false);
        rifle.SetActive(false);
        machineGun.SetActive(true);
    }

    //Teams
    [PunRPC]
    public void JoinRedTeam()
    {
        isRed = true;
        isBlue = false;
        redChar.SetActive(true);
        blueChar.SetActive(false);
        noTeamChar.SetActive(false);
    }

    [PunRPC]
    public void JoinBlueTeam()
    {
        isRed = false;
        isBlue = true;
        redChar.SetActive(false);
        blueChar.SetActive(true);
        noTeamChar.SetActive(false);
    }
}
