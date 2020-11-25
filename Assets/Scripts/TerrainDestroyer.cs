using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDestroyer : MonoBehaviour
{
    // Destroys all platforms that touch the destroy box
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 11)
        {
            Destroy(collision.transform.parent.gameObject);
        }
    }
}
