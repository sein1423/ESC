using UnityEngine;
public class CamController : MonoBehaviour
{
    public GameObject player; // �ٶ� �÷��̾� ������Ʈ�Դϴ�. 
    private Vector3 Player_Height;
    void Start()
    {
        Player_Height = new Vector3(0, 3.5f, 0f);
    }

    // Update is called once per frame 
    void Update()
    {
        transform.rotation = player.transform.rotation;
        Vector3 Eye = player.transform.position + Player_Height;
        Vector3 reverseDistance = new Vector3(0.0f, 0.0f, 0.5f);
        // ī�޶� �ٶ󺸴� �չ����� Z ���Դϴ�. �̵����� ���� Z ������� ���͸� ���մϴ�. 
        transform.position = Eye + transform.rotation * reverseDistance;
        // �÷��̾��� ��ġ���� ī�޶� �ٶ󺸴� ���⿡ ���Ͱ��� ������ ��� ��ǥ�� �����մϴ�. 

    }
}