using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float m_angle = 0f; //AI �þ߰�
    [SerializeField] float m_distance = 0f; //AI �þ� �Ÿ�
    [SerializeField] LayerMask m_layerMask = 0; // �÷��̾� ���̾� ����ũ (Ÿ���� ���̾ ������ �� �ִ� ��)
    public enum CurrentState { Idle, Walk, Run, attack, Death };
    public CurrentState curState = CurrentState.Walk;

    Animator anim;
    NavMeshAgent m_enemy = null;

    [SerializeField] Transform[] m_tfWayPoints = null;
    int m_count = 0;

    Transform m_target = null;

    public void SetTarget(Transform p_target) //�������� ������ ������ ����ϰ� Ÿ���� ���� �Ѿư�
    {
        curState = CurrentState.Walk;
        anim.Play("Walk");
        CancelInvoke(); //������ ���
        m_target = p_target;
    }
    public void RemoveTarget()
    {
        m_target = null;
        curState = CurrentState.Idle;
        anim.Play("Idle");
        InvokeRepeating("MoveToNextWayPoint", 0f, 0.1f); // ���������� ������ �ٽ� ������ �簳
    }

    void MoveToNextWayPoint() //�����Լ�
    {
        if (m_target == null) //Ÿ���� ������
        {
            curState = CurrentState.Walk;
            anim.Play("Walk");
            if (m_enemy.velocity == Vector3.zero)
            {
                m_enemy.SetDestination(m_tfWayPoints[m_count++].position);

                if (m_count >= m_tfWayPoints.Length)
                    m_count = 0;
            }
        }
    }



    void Start()
    {
        anim = GetComponent<Animator>(); //������ Animator ������Ʈ
        m_enemy = GetComponent<NavMeshAgent>();
        InvokeRepeating("MoveToNextWayPoint", 0f, 0.1f); //3�ʸ��� �ݺ�
    }


    void Update()
    {
        Collider[] t_cols = Physics.OverlapSphere(transform.position, m_distance, m_layerMask); // �ֺ��� �ִ� �÷��̾� �ö��̴��� ����

        if (t_cols.Length > 0) //���� �Ǿ��ٸ�
        {
            Transform t_tfPlayer = t_cols[0].transform; //�÷��̾��� Ʈ������ ������ �޾� �� (�÷��̾�� 1���, ����� �ö��̴��� �Ѱ��Ƿ� �ε��� 0�� ������ �÷��̾�)

            Vector3 t_direction = (t_tfPlayer.position - transform.position).normalized; // �÷��̾ ��� ���⿡ �ִ��� ����
            float t_angle = Vector3.Angle(t_direction, transform.forward); // �÷��̾� ������ ���� ����
            if (t_angle < m_angle * 0.5f) // ������ ����� �÷��̾� ������ ���� ���̰� �þ߰����� ������
            {
                if (Physics.Raycast(transform.position, t_direction, out RaycastHit t_hit, m_distance)) //�þ߰� �ȿ� �ִٸ� Ray�� �÷��̾ �� (��ֹ��� ������ üũ)
                {
                    if (t_hit.transform.CompareTag("Player")) //Ray�� ���� ��ü�� Player��� �� ���̿� ��ֹ��� ���� �ɷ� ����
                    {
                        CancelInvoke();
                        m_target = t_tfPlayer;
                        curState = CurrentState.Run;
                        anim.Play("Run");
                        transform.position = Vector3.Lerp(transform.position, t_hit.transform.position, 0.001f); //�÷��̾ ���� �ٰ���
                    }
                   
                }
                
            }
            else
            {
                m_target = null;
                InvokeRepeating("MoveToNextWayPoint", 0f, 0.1f); // �ٽ� ������ �簳
            }

        }

        if (m_target != null)
        {
            m_enemy.SetDestination(m_target.position);
        }
    }

    void Sight() //�þ� üũ �Լ�
    {
        Collider[] t_cols = Physics.OverlapSphere(transform.position, m_distance, m_layerMask); // �ֺ��� �ִ� �÷��̾� �ö��̴��� ����

        if (t_cols.Length > 0) //���� �Ǿ��ٸ�
        {
            Transform t_tfPlayer = t_cols[0].transform; //�÷��̾��� Ʈ������ ������ �޾� �� (�÷��̾�� 1���, ����� �ö��̴��� �Ѱ��Ƿ� �ε��� 0�� ������ �÷��̾�)

            Vector3 t_direction = (t_tfPlayer.position - transform.position).normalized; // �÷��̾ ��� ���⿡ �ִ��� ����
            float t_angle = Vector3.Angle(t_direction, transform.forward); // �÷��̾� ������ ���� ����
            if (t_angle < m_angle * 0.5f) // ������ ����� �÷��̾� ������ ���� ���̰� �þ߰����� ������
            {
                if (Physics.Raycast(transform.position, t_direction, out RaycastHit t_hit, m_distance)) //�þ߰� �ȿ� �ִٸ� Ray�� �÷��̾ �� (��ֹ��� ������ üũ)
                {
                    if (t_hit.transform.CompareTag("Player")) //Ray�� ���� ��ü�� Player��� �� ���̿� ��ֹ��� ���� �ɷ� ����
                    {
                        CancelInvoke();
                        m_target = t_tfPlayer;
                        curState = CurrentState.Run;
                        anim.Play("Run");
                        transform.position = Vector3.Lerp(transform.position, t_hit.transform.position, 0.001f); //�÷��̾ ���� �ٰ���
                    }
                    else
                    {
                        m_target = null;
                    }
                }
            }
        }
    }

    /* private void OnTriggerStay(Collider other)
     {
         if (m_target == null)
         {
             if (other.CompareTag("WayPoint"))
             {
                 curState = CurrentState.Idle;
                 anim.Play("Idle");
             }
         }

     }



     private void OnTriggerExit(Collider other)
     {
         if (m_target == null)
         {
             if (other.CompareTag("WayPoint"))
             {
                 curState = CurrentState.Walk;
                 anim.Play("Walk");
             }
         }

     }*/

}
