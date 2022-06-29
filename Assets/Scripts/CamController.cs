using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    //�÷��̾� �ҷ�����
    public GameObject player;
    public float xmove = 0;
    public float ymove = 0;
    public float distance = 3;

    public float SmoothTime = 0.2f;

    private Vector3 velocity = Vector3.zero;

    private int toggleView = 1;
    private float wheelspeed = 10.0f;

    // Start is called before the first frame update
    void Update()
    {
        //  ���콺 ��Ŭ�� �߿��� ī�޶� ���� ����
        if (Input.GetMouseButton(1))
        {
            xmove += Input.GetAxis("Mouse X"); // ���콺�� �¿� �̵����� xmove �� �����մϴ�.
            ymove -= Input.GetAxis("Mouse Y"); // ���콺�� ���� �̵����� ymove �� �����մϴ�.
        }
        transform.rotation = Quaternion.Euler(ymove, xmove, 0); // �̵����� ���� ī�޶��� �ٶ󺸴� ������ �����մϴ�.

        if (Input.GetMouseButtonDown(2))
            toggleView = 4 - toggleView;

        if (toggleView == 3)
        {
            distance -= Input.GetAxis("Mouse ScrollWheel") * wheelspeed;
            if (distance < 1.0f) distance = 1.0f;
            if (distance > 100.0f) distance = 100.0f;
        }

        if (toggleView == 1)
        {
            Vector3 reverseDistance = new Vector3(0.0f, 3.9f, 0.2f); // ī�޶� �ٶ󺸴� �չ����� Z ���Դϴ�. �̵����� ���� Z ������� ���͸� ���մϴ�.
            transform.position = player.transform.position + transform.rotation * reverseDistance; // �÷��̾��� ��ġ���� ī�޶� �ٶ󺸴� ���⿡ ���Ͱ��� ������ ��� ��ǥ�� �����մϴ�.
            transform.rotation = player.transform.rotation;
        }
        else if (toggleView == 3)
        {
            Vector3 reverseDistance = new Vector3(0.0f, -3.5f, distance); // ī�޶� �ٶ󺸴� �չ����� Z ���Դϴ�. �̵����� ���� Z ������� ���͸� ���մϴ�.
            transform.position = Vector3.SmoothDamp(
                transform.position,
                player.transform.position - transform.rotation * reverseDistance,
                ref velocity,
                SmoothTime);

            Vector3 target = new Vector3(player.transform.position.x, player.transform.position.y + 3.5f, player.transform.position.z);
            transform.LookAt(target);
        }

    }
}

