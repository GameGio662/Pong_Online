using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField]
    private float speed = 30f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            rb.velocity = new Vector2(0, Input.GetAxisRaw("Vertical")) * speed * Time.fixedDeltaTime;
        }
    }
}