using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using System;
public class PlayerController : MonoBehaviour
{
    [SerializeField] public float walkSpeed = 5f;
    [SerializeField] public float runMultiplier = 1.5f;
    [SerializeField] public float dodgeTime = 0.3f;
    [SerializeField] public float jumpForce = 10f;
    [SerializeField] public float maxHP = 100f;
    [SerializeField] public float currentHP = 100f;
    [SerializeField] public float invincibleTime = 0.1f;

    [SerializeField] private DefaultEnemy defaultEnemy;

    [SerializeField] private InputActionReference Move;
    [SerializeField] private InputActionReference Jump;
    [SerializeField] private InputActionReference Dodge;
    [SerializeField] private InputActionReference Attack;

    public Slider slider;
    public Animator animator;

    Rigidbody2D rb;
    Sprite sprite;
    float walk;
    float positionwalk;
    bool isOnFloor;

    bool isInvincible;
    public bool isDamaged;
    float timerTime;
    float timerDodge;
    bool isFight;
    public GameObject gameOverBttn;
    //¬—ﬂ Œ≈ — ¿“¿ Œ…
    
    private float timeBtwAttack;
    [Header("Attack")]
    public float startTimeBtwAttack;
    public Transform attackPose;
    public float attackRange;
    public LayerMask whatIsEnemy;
    public float damage;
    public GameObject gameOver;



    private void OnEnable()
    {
        Move.action.Enable();
        Jump.action.Enable();
        Dodge.action.Enable();
        Attack.action.Enable();
    }
    private void OnDisable()
    {
        Move.action.Disable();
        Jump.action.Disable();
        Dodge.action.Disable();
        Attack.action.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<Sprite>();
        gameOver.SetActive(false);

        gameOverBttn.GetComponent<Button>().onClick.AddListener(ExitGame);
     }

    private void Update()
    {

        walk = Move.action.ReadValue<float>();
        //œ–€∆Œ 
        if (isOnFloor)
        {
            Jump.action.Enable();
            if (Jump.action.IsPressed())
            {
                rb.AddForceY(jumpForce, ForceMode2D.Impulse);
                Jump.action.Disable();
            }
        }
        //”¬Œ–Œ“
        if (Dodge.action.IsPressed())
        {
            isInvincible = true;
            timerTime = dodgeTime;
            animator.SetBool("isDodging", true);
        }
        //¿“¿ ¿
        if (Attack.action.IsPressed()) //”—“¿À, ƒŒƒ≈À¿“‹!
        {
            animator.SetBool("isFight", true);          
            Attack.action.Disable();
            isFight = true;
            if (timeBtwAttack <= 0)
            {
                if (isFight)
                {
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPose.position, attackRange, whatIsEnemy);
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        enemiesToDamage[i].GetComponent<DefaultEnemy>().TakeDamage(damage);
                    }
                }


                timeBtwAttack = startTimeBtwAttack;
            }
            else
            {
                timeBtwAttack -= Time.deltaTime;
            }
        }
        else { animator.SetBool("isFight", false); Attack.action.Enable(); isFight = true; }

        //œŒÀ”◊≈Õ»≈ œ»«ƒﬁÀ≈…
        if (isDamaged && isInvincible == false)
        {
            currentHP = currentHP - defaultEnemy.EnemyDamage();
            isInvincible = true;
            timerTime = invincibleTime;

        }


        //Õ≈”ﬂ«¬»ÃŒ—“‹
        if (isInvincible)
        {
            isDamaged = false;
        }
        //“¿…Ã≈– Õ≈”ﬂ«¬»ÃŒ—“»
        timerTime -= Time.deltaTime;
        if (timerTime <= 0)
        {
            TimerInvincibleEnded();
        }
        //œŒÀŒ— ¿ «ƒŒ–Œ¬‹ﬂ
        slider.SetValueWithoutNotify(currentHP);
        //¿Õ»Ã¿÷»»
        animator.SetFloat("walk", Mathf.Abs(walk));
        if (walk > 0)
        {
            animator.transform.localScale = new Vector3(-1, 1);
        }
        if (walk < 0)
        {
            animator.transform.localScale = new Vector3(1, 1);
        }

    }

    private void FixedUpdate()
    {
        positionwalk = walk * walkSpeed * Time.deltaTime;
        transform.position = transform.position + new Vector3(positionwalk, 0);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isOnFloor = true;
            animator.SetBool("isJumping", false);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            //Debug.Log("·˚ÎÓ");
            isDamaged = true;
        }
        if (collision.gameObject.tag == "GameOver")
        {
            OnDisable();
            gameOver.SetActive(true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isOnFloor = false;
            animator.SetBool("isJumping", true);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            //Debug.Log("ÌÂ ·˚ÎÓ");
            isDamaged = false;
        }
        
    }

    //‘”Õ ÷»ﬂ  ŒÕ÷¿ “¿…Ã≈–¿ Õ≈”ﬂ«¬»ÃŒ—“»
    void TimerInvincibleEnded()
    {
        isInvincible = false;
        animator.SetBool("isDodging", false);
    }

    //‘”Õ ÷»ﬂ ¬€ƒ¿◊» œ»«ƒﬁÀ≈…
    void Fighting()
    {
        if (timeBtwAttack <= 0)
        {
            if (isFight)
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPose.position, attackRange, whatIsEnemy);
                for (int i = 0; i <enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<DefaultEnemy>().TakeDamage(damage); 
                }

            }


            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPose.position, attackRange);
    }

    private void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
