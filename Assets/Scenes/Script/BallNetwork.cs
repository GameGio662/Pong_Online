using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallNetwork : NetworkBehaviour
{
    [SerializeField]
    private float speed = 30f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        rb.simulated = true;
        rb.velocity = Vector2.right * speed;
    }

    private float HitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
    {
        return (ballPos.y - racketPos.y) / racketHeight;
    }

    [Server]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<PlayerNetwork>())
        {
            float y = HitFactor(transform.position, collision.transform.position, collision.collider.bounds.size.y);
            float x = collision.relativeVelocity.x > 0 ? 1 : -1;
            Vector2 direction = new Vector2(x, y).normalized;
            rb.velocity = direction * speed;
        }
    }
}