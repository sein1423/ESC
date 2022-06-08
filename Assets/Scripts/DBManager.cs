using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class DBManager : MonoBehaviour
{
	public string url;

	//command ��� { insertName, deleteName, updateTime, select, rank, init }
	public async void DBCommand(string command, string name, string var1, string var2)		//�ڷ�ƾ���� �������ִ� �Լ�
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
		UnityWebRequest www = UnityWebRequest.Post(url, form);		//url�� ��ɹ� ����
		
		yield return www.SendWebRequest();          //���޵� ���� ���� ���ϰ��� �����ϱ� ���� ���
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
