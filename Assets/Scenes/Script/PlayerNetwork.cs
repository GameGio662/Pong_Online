using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField]
    private float speed = 30f;

    public Rigidbody2D rb;

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            rb.velocity = new Vector2(0, Input.GetAxisRaw("Vertical")) * speed * Time.fixedDeltaTime;
        }
    }
}