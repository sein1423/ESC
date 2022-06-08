using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class DBManager : MonoBehaviour
{
	public string url;

	//command 목록 { insertName, deleteName, updateTime, select, rank, init }
	public async void DBCommand(string command, string name, string var1, string var2)		//코루틴으로 연결해주는 함수
    {
		GameManagement.staticQueryResult = "";
		StartCoroutine(DBConnectionCo(command, name, var1, var2));
		await Task.Delay(500);
	}

	IEnumerator DBConnectionCo(string command, string name, string var1, string var2)
	{
		WWWForm form = new WWWForm();
		form.AddField("command", command);
		form.AddField("name", name);
		form.AddField("var1", var1);
		form.AddField("var2", var2);
		UnityWebRequest www = UnityWebRequest.Post(url, form);		//url에 명령문 매핑
		
		yield return www.SendWebRequest();          //전달된 값에 따른 리턴값이 도착하기 까지 대기
		string dbResult = www.downloadHandler.text;
		if(dbResult == "{\"message\": \"Internal server error\"}")
        {
			GameManagement.staticQueryResult = "connectFail";
        }
        else
        {
			GameManagement.staticQueryResult = dbResult;
        }
		Debug.Log(dbResult);
	}
}
