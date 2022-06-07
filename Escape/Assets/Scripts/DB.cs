using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 아래는 DB 사용에 필요한 패키지들
using System;
using System.Data;
using Mono.Data.SqliteClient;
using System.IO;
using UnityEngine.Networking;

public class DB : MonoBehaviour
{
    //데이터베이스명 : escape
    //테이블명 : record
    //컬럼명 : name(PK, String), timeStr(String), timeNum(float)

    void Awake()
    {
        StartCoroutine(DBCreate());
        DBConnectionCheck();

        //DB 테스트용
        DBQuery("select * from record;");
        //DBQuery("delete from record;");
        //DBInsertName("캔");
        //DBUpdateTime("10분20초", "캔");
        //DBDelete("캔");
    }

    IEnumerator DBCreate()      //StreamingAssets에 있는 파일은 Read Only이기 때문에 Assets에 db파일 복사
    {
        string filepath = Application.dataPath + "/escape.db";    //경로설정
        if (!File.Exists(filepath))     //파일이 없을때
        {
            File.Copy(Application.streamingAssetsPath + "/escape.db", filepath);      //경로로 파일 복사
        }
        yield return null;
    }

    public string GetDBFilePath()       //파일이 생긴 경로를 얻어오는 함수
    {
        string str = "URI=file:" + Application.dataPath + "/escape.db";
        return str;
    }

    public void DBConnectionCheck()     //DB 연결 상태를 확인하는 코드
    {
        try
        {
            IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
            dbConnection.Open();        //DB 열기

            if (dbConnection.State == ConnectionState.Open)      //DB가 잘 열렸다면
            {
                Debug.Log("DB연결 성공");
            }
            else
            {
                Debug.Log("DB연결 실패(에러)");
            }

            dbConnection.Close();       //DB에는 1개의 쓰레드만 접근할 수 있기때문에, 반드시 사용 후 닫아준다
            dbConnection = null;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void DBQuery(string query)      //DB를 읽어오는 코드
    {
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();    //DB 열기
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query;      //쿼리 입력
        IDataReader dataReader = dbCommand.ExecuteReader();     //쿼리 실행
        while (dataReader.Read())       //들어온 레코드 읽기
        {
            Debug.Log(dataReader.GetString(0) + ", " + dataReader.GetString(1) + ", " + dataReader.GetFloat(2));
        }

        dataReader.Dispose();       //생성 순서와 반대로 닫아준다
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }

    public string[] DBSelectRank(int limit)      //DB 랭크 조회, 배열을 반환
    {
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();    //DB 열기
        IDbCommand dbCommand = dbConnection.CreateCommand();
        //name컬럼과 timeStr컬럼을 조회하는데 timeNum의 오름차순으로 정렬하고 5개까지만 조회
        dbCommand.CommandText = "select name, timeStr from record order by timeNum asc limit " + limit;
        IDataReader dataReader = dbCommand.ExecuteReader();     //쿼리 실행

        string[] rank = new string[limit * 2];
        int i = 0;
        while (dataReader.Read())       //들어온 레코드 읽기
        {
            rank[i] = dataReader.GetString(0);
            rank[i+1] = dataReader.GetString(1);
            i += 2;
        }
        dataReader.Dispose();       //생성 순서와 반대로 닫아준다
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
        return rank;
    }

    public string DBSelectName(string dbName)      //인자로 받은 이름에 대한 DB 조회
    {
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();    //DB 열기
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "select name, timeStr from record where name = \"" + dbName + "\"";
        IDataReader dataReader = dbCommand.ExecuteReader();     //쿼리 실행

        string resultDB = "";
        while (dataReader.Read())       //들어온 레코드 읽기
        {
            resultDB = dataReader.GetString(0) + "님의 기록 : " + dataReader.GetString(1);
        }

        dataReader.Dispose();       //생성 순서와 반대로 닫아준다
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;

        return resultDB;
    }

    public bool DBInsertName(string dbName)      //이름을 DB에 입력하는 함수,  반환값 True / False
    {
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();    //DB 열기
        IDbCommand dbCommand = dbConnection.CreateCommand();    //먼저는 입력된 이름이 겹치는 지 확인한다.
        dbCommand.CommandText = "select name from record";    //record 테이블에서 name 컬럼 데이터 조회
        IDataReader dataReader = dbCommand.ExecuteReader();
        while (dataReader.Read())       //실행된 sql문의 데이터 전체 조회
        {
            if (dataReader.GetString(0) == dbName)       //이름이 겹치는 지 조사
            {
                return false;     //겹치는 이름이 있으면 false 반환
            }
        }

        string query = "insert into record(name, timeStr, timeNum) values(\"" + dbName + "\", \"100분0초\", 6000.00);";      //중복된 이름이 없다면 Insert문 실행
        dbCommand.CommandText = query;      //쿼리 입력
        dbCommand.ExecuteNonQuery();    //쿼리 실행

        dataReader.Dispose();
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
        return true;
    }

    public void DBUpdateTime(string timeStr, object timeNum, string dbName)      //시간을 업데이트 하는 함수
    {
        String query = "update record set timeStr=\"" + timeStr + "\", timeNum=" + timeNum + " where name = \"" + dbName + "\";";
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();    //DB 열기
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query;      //쿼리 입력
        dbCommand.ExecuteNonQuery();    //쿼리 실행

        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }

    public void DBDeleteToName(string dbName)     //인자로 넘겨받은 이름의 데이터를 삭제하는 함수
    {
        String query = "delete from record where name = \"" + dbName + "\";";
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();    //DB 열기
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query;      //쿼리 입력
        dbCommand.ExecuteNonQuery();    //쿼리 실행

        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }

    public void DBDeleteToTime(string dbTime)     //인자로 넘겨받은 타임의 데이터를 삭제하는 함수
    {
        String query = "delete from record where timeStr = \"" + dbTime + "\";";
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();    //DB 열기
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query;      //쿼리 입력
        dbCommand.ExecuteNonQuery();    //쿼리 실행

        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }
}