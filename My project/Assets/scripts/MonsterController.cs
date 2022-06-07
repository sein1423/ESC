using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MonsterController : MonoBehaviour
{
    NavMeshAgent agent;
    public Animator anim;
    Transform target;
    public enum State { Idle, Walk, Run, Attack, Death };
    State state;

    void Start()
    {
        state = State.Idle;
        agent = GetComponent<NavMeshAgent>();
    }

    
    void Update()
    {
        if (state == State.Idle)
        {
            UpdateIdle();
        }
        else if (state == State.Run)
        {
            UpdateRun();
        }
        else if (state == State.Attack)
        {
            UpdateAttack();
        }
    }


    void UpdateAttack()
    {
        agent.speed = 0;
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if(distance > 2)
        {
            state = State.Run;
            anim.Play("Run");
        }
    }

    void UpdateRun()
    {
        
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if(distance <= 2)
        {
            state = State.Attack;
            anim.Play("Attack");
        }
        agent.speed = 3.5f;
        agent.destination = target.transform.position;
    }
    
    void UpdateIdle()
    {
        agent.speed = 0;
        target = GameObject.FindWithTag("Player").transform;

        if(target != null)
        {
            state = State.Run;
            anim.Play("Run");
        }
    }
}
