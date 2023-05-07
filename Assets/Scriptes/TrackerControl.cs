using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerControl : MonoBehaviour
{
    Rigidbody2D rb;
    private Transform target;

    [Header("추격 속도")]
    [SerializeField] [Range(1f, 4f)] float moveSpeed = 3f;

    [Header("근접 거리")]
    [SerializeField] [Range(0f, 3f)] float contactDistance = 1f;

    bool follow = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        FollowTarget();
    }
    private void FollowTarget()
    {
        if (target == null)
            return;

        if (Vector2.Distance(transform.position, target.position) > contactDistance && follow)
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
            follow = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        follow = false;
    }
}
