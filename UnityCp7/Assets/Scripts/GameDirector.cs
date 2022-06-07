using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI를 사용하므로 잊지 않고 추가한다

public class GameDirector : MonoBehaviour
{
    GameObject hpGauge;

    void Start()
    {
        this.hpGauge = GameObject.Find("hpGauge");
    }

    public void DecreaseHp() //다른 스크립트에서 불러야 하기 때문에 public으로 작성
    {
        this.hpGauge.GetComponent<Image>().fillAmount -= 0.1f;
    }
}
