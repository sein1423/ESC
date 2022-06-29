using UnityEngine;
public class CamController : MonoBehaviour
{
    public GameObject player; // 바라볼 플레이어 오브젝트입니다. 
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
        // 카메라가 바라보는 앞방향은 Z 축입니다. 이동량에 따른 Z 축방향의 벡터를 구합니다. 
        transform.position = Eye + transform.rotation * reverseDistance;
        // 플레이어의 위치에서 카메라가 바라보는 방향에 벡터값을 적용한 상대 좌표를 차감합니다. 

    }
}