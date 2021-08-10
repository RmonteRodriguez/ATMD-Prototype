using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Shop : MonoBehaviourPunCallbacks
{
    public Player player;

    public int money;

    public GameObject HUD;
    public GameObject ShopUI;

    private bool isOpened;

    //UI Text
    private Text moneyText;

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine) return;

        moneyText = GameObject.Find("ShopUI/Money").GetComponent<Text>();

        ShopUI.SetActive(false);
        isOpened = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (Input.GetKeyDown(KeyCode.Q) && isOpened == false)
        {
            HUD.SetActive(false);
            ShopUI.SetActive(true);
            isOpened = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && isOpened == true)
        {
            HUD.SetActive(true);
            ShopUI.SetActive(false);
            isOpened = false;
        }

        /*
        if (Input.GetKeyDown(KeyCode.Y) && isOpened == true && money > 0)
        {
            photonView.RPC("PurchasePistol", RpcTarget.All);
            //PurchasePistol();
        }
        
        if (Input.GetKeyDown(KeyCode.U) && isOpened == true && money > 10)
        {
            photonView.RPC("PurchaseRifle", RpcTarget.All);
            //PurchaseRifle();
        }
        
        if (Input.GetKeyDown(KeyCode.I) && isOpened == true && money > 20)
        {
            photonView.RPC("PurchaseMachineGun", RpcTarget.All);
            //PurchaseMachineGun();
        }
        */

        if (Input.GetKeyDown(KeyCode.E))
        {
            money = money + 10;
        }

        //UI
        moneyText = GameObject.Find("ShopUI/Money").GetComponent<Text>();
        moneyText.text = money.ToString();
    }

    //Purchase Weapons
    [PunRPC]
    public void PurchasePistol()
    {

        if (money >= 10)
        money = money - 10;
        player.pistol.SetActive(true);
        player.rifle.SetActive(false);
        player.machineGun.SetActive(false);
    }

    [PunRPC]
    public void PurchaseRifle()
    {
        money = money - 20;
        player.pistol.SetActive(false);
        player.rifle.SetActive(true);
        player.machineGun.SetActive(false);
    }

    [PunRPC]
    public void PurchaseMachineGun()
    {
        money = money - 30;
        player.pistol.SetActive(false);
        player.rifle.SetActive(false);
        player.machineGun.SetActive(true);
    }
}