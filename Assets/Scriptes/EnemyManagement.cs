using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagement : MonoBehaviour
{
    [SerializeField] private GameObject death;
    Rigidbody2D rigid;
    public int nextMove;
    public int EnemyLife;

    Animator ani;
    SpriteRenderer sprite;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        Invoke("Think", 5f);

        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {

        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.4f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (nextMove == 1)
            transform.localScale = new Vector3(-5f, 5f, 1f);
        else if (nextMove == -1)
            transform.localScale = new Vector3(5f, 5f, 1f);

        if (raycast.collider == null)
        {
            Turn();
        }
    }
    private void Turn()
    {
        nextMove *= -1;

        CancelInvoke();
        Invoke("Think", 2f);
    }
    private void Think()
    {
        nextMove = Random.Range(-1, 2);

        ani.SetInteger("walkSpeed", nextMove);



        float time = Random.Range(2f, 5f);
        Invoke("Think", time);
    }

    public void SetDamage(int p_val)
    {

        EnemyLife -= p_val;
        if (EnemyLife <= 0)
        {
            GameObject.Destroy(gameObject);

            GameObject obj = Instantiate(death, transform.position, Quaternion.identity);
            Destroy(obj, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BulletManagment bullet = other.GetComponent<BulletManagment>();

        if (bullet != null)
        {
            SetDamage(bullet.Damage);
        }
    }
}
