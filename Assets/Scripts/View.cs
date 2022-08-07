using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class View : MonoBehaviour
{
    [SerializeField] float m_angle = 0f; //AI �þ߰�
    [SerializeField] float m_distance = 0f; //AI �þ� �Ÿ�
    [SerializeField] LayerMask m_layerMask = 0; // �÷��̾� ���̾� ����ũ (Ÿ���� ���̾ ������ �� �ִ� ��)
    public enum CurrentState { Idle, Walk, Run, attack, Death };
    public CurrentState curState = CurrentState.Idle;

   
    Animator anim;
    NavMeshAgent m_enemy = null;

   

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
                    if (t_hit.transform.name == "Player") //Ray�� ���� ��ü�� Player��� �� ���̿� ��ֹ��� ���� �ɷ� ����
                    {
                        curState = CurrentState.Run;
                        anim.Play("Run");
                        transform.position = Vector3.Lerp(transform.position, t_hit.transform.position, 0.001f); //�÷��̾ ���� �ٰ���
                    }
                }
            }
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>(); //������ Animator ������Ʈ
        m_enemy = GetComponent<NavMeshAgent>();


    }

    void Update()
    {
        Sight();
    }
}