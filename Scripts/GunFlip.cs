using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GunFlip : MonoBehaviourPunCallbacks
{
    public float angle;

    public Transform playerTransform;

    public Rigidbody2D rb;
    public Camera cam;

    public Vector3 mousePos;
    public Vector3 gunPos;

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        Flip();
    }

    public void Flip()
    {
        mousePos = Input.mousePosition;
        gunPos = cam.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - gunPos.x;
        mousePos.y = mousePos.y - gunPos.y;
        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        Vector3 gunScale = transform.localScale;

        if (angle >= 90 || angle <= -90)
        {
            gunScale.y = -1f;
        }
        else
        {
            gunScale.y = 1f;
        }

        transform.localScale = gunScale;
    }
}
