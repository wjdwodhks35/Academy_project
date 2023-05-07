using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private SpriteRenderer sprite;

    //이동
    public float Speed;
    public float ShootandRunSpeed;
    public float jumpPower;
    float hor;
    bool isJump = false;

    //공격
    private Transform bulletShooter;
    public GameObject copyBullet;
    public bool canshot = true;
    public float shootDelay;
    float shootTimer = 0;
    bool isShoot;

    //에니메이션
    Animator ani;
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
        UpdateAttackAnim();

    }
    private void FixedUpdate()
    {
        hor = Input.GetAxisRaw("Horizontal");

        rigid.velocity = new Vector2(hor * Speed, rigid.velocity.y);
        if (isShoot)
            rigid.velocity = new Vector2(hor * ShootandRunSpeed, rigid.velocity.y);
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
        if(other.gameObject.tag.Equals("Enemy"))
        {
            ani.SetBool("isLive", false);
            GameObject.Destroy(gameObject, 1.1f);
            Speed = 0;
            jumpPower = 0;
            canshot = false;
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


    public float AnimationDelaySec = 0f;

    public void UpdateAttackAnim()
    {
        if (AnimationDelaySec <= 0f)
            return;

        AnimationDelaySec -= Time.deltaTime;
        if (AnimationDelaySec <= 0f)
        {
            ani.SetBool("isAttack", false);
            ani.SetFloat("AttackWeight", 0);
            isShoot = false;
        }
    }

    private void ShootController()
    {
        if (Input.GetMouseButton(0) && canshot)
        {
            if (shootTimer > shootDelay)
            {
                ani.SetFloat("AttackWeight", 1);
                ani.SetBool("isAttack", true);
                GameObject cloneBullet = Instantiate(copyBullet, bulletShooter.transform.position, transform.rotation);
                shootTimer = 0;

                isShoot = true;
                AnimationDelaySec = 0.5f;
            }
        }

        shootTimer += Time.deltaTime;
        //ani.SetBool("isAttack", false);
    }
}
