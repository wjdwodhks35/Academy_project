using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private SpriteRenderer sprite;

    //이동
    public float Speed;
    public float jumpPower;
    float hor;
    bool isJump = false;

    //공격
    private Transform bulletShooter;
    public GameObject copyBullet;
    public bool canshot = true;
    public float shootDelay;
    float shootTimer = 0;

    //에니메이션
    Animator ani;
    string animationsState = "AnimationState";

    private void Start()
    {
        bulletShooter = this.transform.Find("BulletShooter");
    }
    void Awake()
    {
        ani = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Jump();
        Flip();
        UpdateState();
        ShootController();
    }
    private void FixedUpdate()
    {
        hor = Input.GetAxisRaw("Horizontal");

        rigid.velocity = new Vector2(hor * Speed, rigid.velocity.y);
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            isJump = true;
            rigid.velocity = Vector2.up * jumpPower;
            ani.SetBool("isJump", true);
            ani.SetTrigger("doJumping");
            canshot = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Equals("Layer"))
        {
            isJump = false;
            canshot = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6 && rigid.velocity.y < 0)
            ani.SetBool("isJump", false);
    }
    private void Flip()
    {
        if (hor < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (hor > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
    private void UpdateState()
    {
        if (hor != 0)
        {
            ani.SetBool("isMove", true);
        }
        else
        {
            ani.SetBool("isMove", false);
        }
    }

    private void ShootController()
    {
        if (Input.GetMouseButtonDown(0) && canshot)
        {
            if (shootTimer > shootDelay)
            {
                ani.SetBool("isAttack", true);
                GameObject cloneBullet = Instantiate(copyBullet, bulletShooter.transform.position, transform.rotation);
                shootTimer = 0;
            }

            shootTimer += Time.deltaTime;
        }
        ani.SetBool("isAttack", false);
    }
}
