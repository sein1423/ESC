using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowGenerator : MonoBehaviour
{
    public GameObject arrowPrefab; //만든 프리팹을 매개변수로 적용해야함
    float span = 1.0f; //1초마다 생성
    float delta = 0; //프레임을 확인하기 위해

    void Update()
    {
        this.delta += Time.deltaTime; //delta 변수에 누적시킴 Time.deltaTime멤버변수:update 함수가 한번 수행될때 걸리는 시간
        if (this.delta > this.span) //1초보다 크면
        {
            this.delta = 0;
            GameObject go = Instantiate(arrowPrefab); //중요 Instantiate함수 알고있기
            int px = Random.Range(-6, 7); //-6에서 7까지 랜덤으로 변수에 대입
            go.transform.position = new Vector3(px, 7, 0); //프리팹 오브젝트의 위치(생성되는 위치)
        }
    }
}
