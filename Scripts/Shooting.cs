using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Shooting : MonoBehaviourPunCallbacks
{
    public GameObject bulletPrefab;

    public Transform firePoint;

    public float bulletForce = 20f;
    public float fireRate;

    public bool canShoot;

    //Ammo and Reload
    public float clipSizeStart;
    private float currentClipSize;
    public float baseReloadTime;
    private float reloadTime;

    //Ammo UI
    public Text ammoText;

    // Start is called before the first frame update
    void Start()
    {
        currentClipSize = clipSizeStart;

        if (photonView.IsMine)
        {
            ammoText = GameObject.Find("HUD/Ammo/Text").GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        if (currentClipSize < 1)
        {
            canShoot = false;
        }

        if (canShoot == true)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                photonView.RPC("Shoot", RpcTarget.All);

                //Cancel any Shoot() method code execution  
                CancelInvoke("Shoot");
            }

            //while the "Fire1" button is being held down  
            if (Input.GetButton("Fire1") && !IsInvoking("Shoot"))
            {
                photonView.RPC("Hold", RpcTarget.All);
            }

            //If the "Fire1" has been released, cancel any scheduled Shoot() method executions  
            if (Input.GetButtonUp("Fire1"))
            {
                //Cancel any Shoot() method code execution  
                CancelInvoke("Shoot");
            }
        }
        else if (canShoot == false)
        {
            photonView.RPC("Reload", RpcTarget.All);
        }

        ammoText.text = (int)currentClipSize + " / " + (int)clipSizeStart;
    }

    [PunRPC]
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
        currentClipSize--;
    }

    [PunRPC]
    void Hold()
    {
        Invoke("Shoot", fireRate);
    }

    [PunRPC]
    void Reload()
    {
        reloadTime -= Time.deltaTime;

        if (reloadTime <= 0)
        {
            currentClipSize = clipSizeStart;
            reloadTime = baseReloadTime;
            canShoot = true;
        }
    }
}
