using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Summary : The act of enemy(Idle, Move, Attack, Return, Damaged, Die)

public class EnemyFSM : MonoBehaviour
{
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    EnemyState m_State;

    public float findDistance = 8f;
    public float attackDistance = 2f;
    public float moveDistance = 20f;

    float curDelay = 0f;
    public float moveSpeed = 5f;
    public float attackDelay = 1.5f;
    public int attackPower = 3;
    public float hp = 15f;
    float maxHp = 15f;

    CharacterController cc;
    Transform player;
    Vector3 originPos;

    void Start()
    {
        m_State = EnemyState.Idle;

        player = GameObject.Find("Player").transform;

        cc = GetComponent<CharacterController>();

        originPos = transform.position;
    }

    void Update()
    {
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;

            case EnemyState.Move:
                Move();
                break;

            case EnemyState.Attack:
                Attack();
                break;

            case EnemyState.Return:
                Return();
                break;

            case EnemyState.Damaged:
                Damaged();
                break;

            case EnemyState.Die:
                Die();
                break;

        }
    }

    void Idle()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < findDistance)
        {
            Debug.Log("상태 전환 : Idle -> Move");
            m_State = EnemyState.Move;
        }
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            m_State = EnemyState.Return;
            Debug.Log("상태 전환 : Move -> Return");
        }

        else if (Vector3.Distance(player.transform.position, transform.position) > attackDistance)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;

            cc.Move(dir * moveSpeed * Time.deltaTime);
        }

        else
        {
            Debug.Log("상태 전환 : Move -> Attack");
            m_State = EnemyState.Attack;
            curDelay = attackDelay;
        }
    }

    void Attack()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < attackDistance)
        {
            curDelay += Time.deltaTime;
            if (curDelay > attackDelay)
            {
                player.GetComponent<PlayerMove>().PlayerDamaged(attackPower);
                Debug.Log("Attack!");
                curDelay = 0;
            }
        }

        else
        {
            m_State = EnemyState.Move;
            Debug.Log("상태 전환 : Attack -> Move");
        }
    }

    void Return()
    {
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }

        else
        {
            originPos = transform.position;
            hp = maxHp;
            m_State = EnemyState.Idle;
            Debug.Log("상태 전환 : Return -> Idle");
        }
    }

    public void HitEnemy(int hitPower)
    {
        hp -= hitPower;

        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            Debug.Log("상태 전환 : Any State -> Damaged");
            Damaged();
        }

        else
        {
            m_State = EnemyState.Die;
            Debug.Log("상태 전환 : Any State -> Die");
            Die();
        }
    }

    void Damaged()
    {
        StartCoroutine(DamagedProcess());
    }

    IEnumerator DamagedProcess()
    {
        yield return new WaitForSeconds(0.5f);

        m_State = EnemyState.Move;
        Debug.Log("상태 전환 : Damaged -> Move");
    }

    void Die()
    {
        StopAllCoroutines();

        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        cc.enabled = false;

        yield return new WaitForSeconds(2f);
        Debug.Log("Die!");
        Destroy(gameObject);
    }
}
