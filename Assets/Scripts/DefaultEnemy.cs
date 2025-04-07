using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TextCore;
using TMPro;

[Serializable]
public class DefaultEnemy : MonoBehaviour
{
    public static DefaultEnemy instance;

    [SerializeField] public float enemyDamage = 5;
    [SerializeField] public float maxHP = 100f;
    [SerializeField] public float currentHP = 100f;

    public GameObject prefab;
    public GameObject timer;
    public int killCount = 0;
    public float enemySpeed;
    public float stoppingDistance;
    public Transform point;
    public int positionOfPatrol;
    bool movingRight;

    bool isChill = false;
    bool isAngry = false;
    bool isGoingBack = false;
    public bool isEnemyAlive = true;

    Transform player;
    Vector2 direction;
    public SpriteRenderer spriteRendr;

    public Slider slider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        slider.SetValueWithoutNotify(currentHP);

        //œ¿“–”À»–Œ¬¿Õ»≈
        //“€–€ œ€–€
        if (Vector2.Distance(transform.position, point.position) < positionOfPatrol && isAngry == false)
        {
            isChill = true;
        }
        if (Vector2.Distance(transform.position, player.position) < stoppingDistance)
        {
            isAngry = true;
            isChill = false;
            isGoingBack = false;
        }
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            isGoingBack = true;
            isAngry = false;
        }

        //”ÒÎÓ‚Ëˇ
        if (isChill == true) { Chill(); }
        else if (isAngry == true) { Angry(); }
        else if (isGoingBack == true) { GoBack(); }


        //—ÏÂÚ˙
        if (currentHP <= 0)
        {
            isEnemyAlive = false;
            GameObject.Destroy(prefab);
        }
        
    }

    public float EnemyDamage()
    {
        return enemyDamage;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
    }


    void Chill()
    {
        if (transform.position.x > point.position.x + positionOfPatrol)
        {
            movingRight = false;
        }
        else if (transform.position.x < point.position.x - positionOfPatrol)
        {
            movingRight = true;
        }
        if (movingRight)
        {
            transform.position = new Vector2(transform.position.x + enemySpeed * Time.deltaTime, transform.position.y);
            //transform.localScale = new Vector3(-1, 1);
            spriteRendr.flipX = true;
        }
        else
        {
            transform.position = new Vector2(transform.position.x - enemySpeed * Time.deltaTime, transform.position.y);
            //transform.localScale = new Vector3(1, 1);
            spriteRendr.flipX = false;
        }
    }

    void Angry()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, enemySpeed * Time.deltaTime);
        direction = (player.position - transform.position).normalized;
        spriteRendr.flipX = direction.x > 0;
    }

    void GoBack()
    {
        transform.position = Vector2.MoveTowards(transform.position, point.position, enemySpeed * Time.deltaTime);
    }

   
}
