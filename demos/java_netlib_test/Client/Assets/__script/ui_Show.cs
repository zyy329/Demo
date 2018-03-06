using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ui_Show : MonoBehaviour {
	public Text content;
    private Connecter conn = null;

    // Use this for initialization
    void Start ()
    {
        Log.LogSetInit.Init();
        conn = new Connecter();
        conn._pros.OnRes_Person += ResPerson;       // 消息监听; 

        content.text = "get zyy";
        //testProtoBuf();
        //onClick_Btn();
    }
	
	// Update is called once per frame
	void Update () {
        conn.ProcessProtocol();
    }

    private void OnDestroy()
    {
        // 必须调用, 否则可能在结束时, 卡在 _socket.Receive 里;
        conn.StopNetWork();
    }

    public void testLog()
    {
        Log.LogSetInit.Init();
        Log.Loggers.net.Debug("Debug");
        Log.Loggers.net.Info("Info");
        Log.Loggers.net.Warning("Warn");
        Log.Loggers.net.Error("Error");
    }

    public void testProtoBuf()
    {
        // 测试 protoBuf;
        tutorial.Person person = new tutorial.Person();
        person.name = "zyy";
        person.id = 1234;
        person.email = "zyy@example.com";
        person.phones.Add(new tutorial.Person.PhoneNumber { number = "555-4321", type = tutorial.Person.PhoneType.HOME });
        person.phones.Add(new tutorial.Person.PhoneNumber { number = "222", type = tutorial.Person.PhoneType.WORK });

        // 写入文件;
        using (var output = File.Create("nToS.dat"))
        {
            //序列化对象到文件
            ProtoBuf.Serializer.Serialize<tutorial.Person>(output, person);
        }

        //// 从文件读出;
        //tutorial.Person per_2;
        //using (var input = File.OpenRead("nToS.dat"))
        //{
        //    //反序列化
        //    per_2 = ProtoBuf.Serializer.Deserialize<tutorial.Person>(input);
        //}
    }

    public void onClick_Btn()
    {
        try
        {
            // 建立连接, 监听消息;
            conn.AskStartNetWork("127.0.0.1", 7731);

            Log.Loggers.nomal.Error("");
        }
        catch (System.Exception e)
        {
            Log.Loggers.nomal.Error(string.Format("Exception error: {0}", e));
        }
    }

    public void onClick_Btn1()
    {
        // 请求测试;
        var msgSend = (ProtoMessage<tutorial.reqPerson>)conn._pros.GetMessage(MessageID_Define.CG_Person);
        msgSend.MsgObj.id = 65;
        conn.SendMsg(msgSend);
    }

    private static void ResPerson(Message msg)
    {
        Log.Loggers.nomal.Error("ResPerson");
    }
}
