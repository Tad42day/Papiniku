using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Player player = col.gameObject.GetComponent<Player>();

            GameObject.Find("Manager").GetComponent<Manager>().pontuacaoPlayers[player.cod - 1]++;
            Destroy(gameObject);
        }
    }
}