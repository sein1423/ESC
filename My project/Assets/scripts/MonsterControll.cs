using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MonsterControll : MonoBehaviour
{
    public enum CurrentState { Idle, Walk, Run, attack, Death};
    public CurrentState curState = CurrentState.Idle;
    

    private Transform _transform;
    private Transform playerTransform;
    private NavMeshAgent nvAgent;
    Animator anim;
    
    
    

    public float traceDist = 15.0f; //������ �����ϴ� �Ÿ�
    public float attackDist = 3.2f; // �����ϴ� �Ÿ�
    private bool isDead = false;

    private void Start()
    {
        _transform = gameObject.GetComponent<Transform>(); //������ ��ġ
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>(); //�÷��̾��� ��ġ
        nvAgent = gameObject.GetComponent<NavMeshAgent>(); //������ navNMeshAgent ������Ʈ
        
        

        anim = GetComponent<Animator>(); //������ Animator ������Ʈ

        

        StartCoroutine(this.CheckState()); //�ڷ�ƾ�Լ�
        StartCoroutine(this.CheckStateForAction()); //�ڷ�ƾ�Լ�

       

    }

    IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.2f);

            float dist = Vector3.Distance(playerTransform.position, _transform.position); //���Ϳ� �÷��̾��� �Ÿ�

            if(dist <= attackDist)
            {
                curState = CurrentState.attack;
                anim.SetFloat("isAttack", 1);
                SceneManager.LoadScene("Die");
                
             
            }
            else if(dist <= traceDist)
            {
                curState = CurrentState.Walk;
                anim.Play("Walk");
            }
            else
            {
                curState = CurrentState.Idle;
                anim.Play("idle");
            }
        }
    }

    IEnumerator CheckStateForAction()
    {
        while (!isDead)
        {
            switch (curState)
            {
                case CurrentState.Idle:
                    nvAgent.Stop();
                    break;
                case CurrentState.Walk:
                    nvAgent.destination = playerTransform.position;
                    nvAgent.Resume();
                    break;
                case CurrentState.attack:
                    break;
            }

            yield return null; //�ڷ�ƾ �Լ� ��ȯ��
        }
    }


    void Update()
    {
        
    }
}