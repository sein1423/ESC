using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// �Ʒ��� DB ��뿡 �ʿ��� ��Ű����
using System;
using System.Data;
using Mono.Data.SqliteClient;
using System.IO;
using UnityEngine.Networking;

public class DB : MonoBehaviour
{
    //�����ͺ��̽��� : escape
    //���̺�� : record
    //�÷��� : name(PK, String), timeStr(String), timeNum(float)

    void Awake()
    {
        StartCoroutine(DBCreate());
        DBConnectionCheck();

        //DB �׽�Ʈ��
        DBQuery("select * from record;");
        //DBQuery("delete from record;");
        //DBInsertName("ĵ");
        //DBUpdateTime("10��20��", "ĵ");
        //DBDelete("ĵ");
    }

    IEnumerator DBCreate()      //StreamingAssets�� �ִ� ������ Read Only�̱� ������ Assets�� db���� ����
    {
        string filepath = Application.dataPath + "/escape.db";    //��μ���
        if (!File.Exists(filepath))     //������ ������
        {
            File.Copy(Application.streamingAssetsPath + "/escape.db", filepath);      //��η� ���� ����
        }
        yield return null;
    }

    public string GetDBFilePath()       //������ ���� ��θ� ������ �Լ�
    {
        string str = "URI=file:" + Application.dataPath + "/escape.db";
        return str;
    }

    public void DBConnectionCheck()     //DB ���� ���¸� Ȯ���ϴ� �ڵ�
    {
        try
        {
            IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
            dbConnection.Open();        //DB ����

            if (dbConnection.State == ConnectionState.Open)      //DB�� �� ���ȴٸ�
            {
                Debug.Log("DB���� ����");
            }
            else
            {
                Debug.Log("DB���� ����(����)");
            }

            dbConnection.Close();       //DB���� 1���� �����常 ������ �� �ֱ⶧����, �ݵ�� ��� �� �ݾ��ش�
            dbConnection = null;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void DBQuery(string query)      //DB�� �о���� �ڵ�
    {
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();    //DB ����
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query;      //���� �Է�
        IDataReader dataReader = dbCommand.ExecuteReader();     //���� ����
        while (dataReader.Read())       //���� ���ڵ� �б�
        {
            Debug.Log(dataReader.GetString(0) + ", " + dataReader.GetString(1) + ", " + dataReader.GetFloat(2));
        }

        dataReader.Dispose();       //���� ������ �ݴ�� �ݾ��ش�
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }

    public string[] DBSelectRank(int limit)      //DB ��ũ ��ȸ, �迭�� ��ȯ
    {
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();    //DB ����
        IDbCommand dbCommand = dbConnection.CreateCommand();
        //name�÷��� timeStr�÷��� ��ȸ�ϴµ� timeNum�� ������������ �����ϰ� 5�������� ��ȸ
        dbCommand.CommandText = "select name, timeStr from record order by timeNum asc limit " + limit;
        IDataReader dataReader = dbCommand.ExecuteReader();     //���� ����

        string[] rank = new string[limit * 2];
        int i = 0;
        while (dataReader.Read())       //���� ���ڵ� �б�
        {
            rank[i] = dataReader.GetString(0);
            rank[i+1] = dataReader.GetString(1);
            i += 2;
        }
        dataReader.Dispose();       //���� ������ �ݴ�� �ݾ��ش�
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
        return rank;
    }

    public string DBSelectName(string dbName)      //���ڷ� ���� �̸��� ���� DB ��ȸ
    {
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();    //DB ����
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "select name, timeStr from record where name = \"" + dbName + "\"";
        IDataReader dataReader = dbCommand.ExecuteReader();     //���� ����

        string resultDB = "";
        while (dataReader.Read())       //���� ���ڵ� �б�
        {
            resultDB = dataReader.GetString(0) + "���� ��� : " + dataReader.GetString(1);
        }

        dataReader.Dispose();       //���� ������ �ݴ�� �ݾ��ش�
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;

        return resultDB;
    }

    public bool DBInsertName(string dbName)      //�̸��� DB�� �Է��ϴ� �Լ�,  ��ȯ�� True / False
    {
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();    //DB ����
        IDbCommand dbCommand = dbConnection.CreateCommand();    //������ �Էµ� �̸��� ��ġ�� �� Ȯ���Ѵ�.
        dbCommand.CommandText = "select name from record";    //record ���̺��� name �÷� ������ ��ȸ
        IDataReader dataReader = dbCommand.ExecuteReader();
        while (dataReader.Read())       //����� sql���� ������ ��ü ��ȸ
        {
            if (dataReader.GetString(0) == dbName)       //�̸��� ��ġ�� �� ����
            {
                return false;     //��ġ�� �̸��� ������ false ��ȯ
            }
        }

        string query = "insert into record(name, timeStr, timeNum) values(\"" + dbName + "\", \"100��0��\", 6000.00);";      //�ߺ��� �̸��� ���ٸ� Insert�� ����
        dbCommand.CommandText = query;      //���� �Է�
        dbCommand.ExecuteNonQuery();    //���� ����

        dataReader.Dispose();
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
        return true;
    }

    public void DBUpdateTime(string timeStr, object timeNum, string dbName)      //�ð��� ������Ʈ �ϴ� �Լ�
    {
        String query = "update record set timeStr=\"" + timeStr + "\", timeNum=" + timeNum + " where name = \"" + dbName + "\";";
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();    //DB ����
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query;      //���� �Է�
        dbCommand.ExecuteNonQuery();    //���� ����

        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }

    public void DBDeleteToName(string dbName)     //���ڷ� �Ѱܹ��� �̸��� �����͸� �����ϴ� �Լ�
    {
        String query = "delete from record where name = \"" + dbName + "\";";
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();    //DB ����
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query;      //���� �Է�
        dbCommand.ExecuteNonQuery();    //���� ����

        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }

    public void DBDeleteToTime(string dbTime)     //���ڷ� �Ѱܹ��� Ÿ���� �����͸� �����ϴ� �Լ�
    {
        String query = "delete from record where timeStr = \"" + dbTime + "\";";
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();    //DB ����
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query;      //���� �Է�
        dbCommand.ExecuteNonQuery();    //���� ����

        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }
}