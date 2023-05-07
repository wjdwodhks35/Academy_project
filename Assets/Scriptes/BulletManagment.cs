using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManagment : MonoBehaviour
{
    [SerializeField] private GameObject Impact;
    public float bulletSpeed;

    public int Damage;

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
        if (other.gameObject.tag.Equals("Enemy"))
        {
            Destroy(this.gameObject);
            GameObject obj = Instantiate(Impact, transform.position, Quaternion.identity);

            Destroy(obj, 0.4f);
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
