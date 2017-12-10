using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletState
{
    Unsafe,
    Safe
}

public class Bullet : MonoBehaviour {

    public float speed;
    public Vector2 direcao;
    public int ownerCod;
    Rigidbody2D rb;
    int contador;
    public BulletState state;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direcao * speed, ForceMode2D.Force);
    }

    // Update is called once per frame
    void Update () {
        
        if (contador >= 2)
        {
            //Vector2 forca = new Vector2((rb.velocity.x*-1)-0.5f, (rb.velocity.y * -1) - 0.5f);
            rb.velocity *= 0.9f;
            //Debug.Log(rb.velocity);
            state = BulletState.Safe;
        }
        //if(rb.velocity < 0)
       
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        contador++;
    }
}