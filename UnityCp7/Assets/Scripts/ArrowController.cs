using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        this.player = GameObject.Find("player");  //Player 오브젝트를 찾음
    }

    void Update()
    {
        // 프레임마다 등속으로 낙하시킨다
        GetComponent<AudioSource>().Play();
        transform.Translate(0, -0.1f, 0);
        
        // 화면 밖으로 나오면 오브젝트를 삭제한다
        if (transform.position.y < -6.0f)
        {
            Destroy(gameObject); //오브젝트 삭제
        }

        // 충돌 판정
        Vector2 p1 = transform.position;              // 화살의 중심 좌표
        Vector2 p2 = this.player.transform.position;  // 플레이어의 중심 좌표
        Vector2 dir = p1 - p2; 
        float d = dir.magnitude; //두 오브젝트의 거리
        float r1 = 0.5f;  // 화살 반경 (반지름 예측한거임)
        float r2 = 1.0f;  // 플레이어 반경 (반지름 예측한거임)

        if (d < r1 + r2) //충돌 했을 때(두 오브젝트가 겹칠 때)
        {
            // 감독 스크립트에 플레이어와 화살이 충돌했다고 전달한다 
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseHp(); //함수 호출
            

            // 충돌했다면 화살을 지운다
            Destroy(gameObject);
        }
    }
}
