using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    //플레이어 불러오기
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
        //  마우스 우클릭 중에만 카메라 무빙 적용
        if (Input.GetMouseButton(1))
        {
            xmove += Input.GetAxis("Mouse X"); // 마우스의 좌우 이동량을 xmove 에 누적합니다.
            ymove -= Input.GetAxis("Mouse Y"); // 마우스의 상하 이동량을 ymove 에 누적합니다.
        }
        transform.rotation = Quaternion.Euler(ymove, xmove, 0); // 이동량에 따라 카메라의 바라보는 방향을 조정합니다.

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
            Vector3 reverseDistance = new Vector3(0.0f, 3.9f, 0.2f); // 카메라가 바라보는 앞방향은 Z 축입니다. 이동량에 따른 Z 축방향의 벡터를 구합니다.
            transform.position = player.transform.position + transform.rotation * reverseDistance; // 플레이어의 위치에서 카메라가 바라보는 방향에 벡터값을 적용한 상대 좌표를 차감합니다.
            transform.rotation = player.transform.rotation;
        }
        else if (toggleView == 3)
        {
            Vector3 reverseDistance = new Vector3(0.0f, -3.5f, distance); // 카메라가 바라보는 앞방향은 Z 축입니다. 이동량에 따른 Z 축방향의 벡터를 구합니다.
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

