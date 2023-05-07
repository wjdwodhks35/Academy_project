using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManagment : MonoBehaviour
{
    public float bulletSpeed;
    
    void Start()
    {

    }

    void FixedUpdate()
    {
        if (transform.rotation.y == 0)
            transform.Translate(transform.right * bulletSpeed * Time.deltaTime);
        else
            transform.Translate(transform.right * -1 * bulletSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(this.gameObject);
    }
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
