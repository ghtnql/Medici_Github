using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.IO;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class ClientReDecode1 : MonoBehaviour
{
    #region 연결한 호스트 IP와 Port번호
    const string host = "127.0.0.1";
    const int port = 50000;
    #endregion

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    ArrayList byteToArray;    //스트림으로부터 오는 Data넣을 그릇


    //PNC센서에서 넘어오는 real 변수
    int screenX;
    int screenY;
    float spinTop;
    float spinSide;
    float shootAng;
    float directionAng;
    float velocity;

    /// <summary>
    /// by준희, 다른 메소드에서도 PNC센서의 값을 불러올 수 있도록 한 구조체.
    /// </summary>
    public struct PNCVariable
    {
        public static int UscreenX;
        public static int UscreenY;
        public static float UspinTop;
        public static float UspinSide;
        public static float UshootAng;
        public static float UdirectionAng;
        public static float Uvelocity;

        /// <summary>
        /// 플레이어가 플레이 했을 때 UscreenX, UscreenY, UspinTop, UspinSide, 
        /// UshootAng, UdirectionAng, Uvelocity의 값을 업데이트하는 생성자.
        /// </summary>
        /// <param name="scrx">int값 ScreenX </param>
        /// <param name="scry">int값 ScreenY </param>
        /// <param name="spT">볼의 Top스핀값</param>
        /// <param name="spSd">볼의 Side스핀값</param>
        /// <param name="shtAg">볼의 Shooting Angle</param>
        /// <param name="dirAng">볼의 힘의 방향(날아가고자하는 방향)</param>
        /// <param name="vlct">볼의 속도 Km/h</param>
        public PNCVariable(int scrx, int scry, float spT, float spSd, float shtAg, float dirAng, float vlct)
        {
            UscreenX = scrx;
            UscreenY = scry;
            UspinTop = spT;
            UspinSide = spSd;
            UshootAng = shtAg;
            UdirectionAng = dirAng;
            Uvelocity = vlct;
        }

    }

    /// <summary>
    /// by준희, 서버로 부터 받은 패킷값을 Decode.
    /// </summary>
    private void DecodeByte()
    {
        //by준희, 현재 센서로부터 받아오는 값은 70번~ 77번 (추후 추가를 원할 시 추가가능)
        //70:screenX, 71:screenY, 72:spinTop, 73:spinSide, 74:shootAng, 75:directionAng, 76:velocity
        //패킷으로 넘어오는 자료의 형태 :ex) 카테고리_1바이트, 타입_1바이트, 값_4바이트
        for (int i = 70; i < 77; i++)
        {
            //by준희, byteToArray배열에 넣은 stream데이터중 첫번째 바이트 값을 16진수로 변경해 70~77번인지 확인 
            if (Convert.ToByte(byteToArray[0]).ToString("X2") == i.ToString())
            {
                //by준희, 카테고리값과 타입을 나타내는 2바이트 배열을 제거.(활용되지 않으므로)
                byteToArray.RemoveRange(0, 2);
                //by준희, 값_4바이트를 담을 byte배열 생성
                byte[] data = new byte[4];
                //by준희, 4바이트로 나눠진 값을 다시 합치기 위해 byte배열에 담는다.
                for (int t = 0; t < 4; t++)
                {
                    data[t] = Convert.ToByte(byteToArray[t]);
                }
                //by준희, 후입선출로 쌓인 데이터를 역순으로 바꾸어 원래의 데이터로 변환.
                Array.Reverse(data, 0, 4);
                //by준희, 각각의 센서 값을 약속에 따라 변경.(현재는 바이트값을 모두 int값으로 변경)
                switch (i)
                {
                    case (70):
                        screenX = BitConverter.ToInt32(data, 0);
                        break;
                    case (71):
                        screenY = BitConverter.ToInt32(data, 0);
                        break;
                    case (72):
                        spinTop = BitConverter.ToInt32(data, 0) / 100;
                        break;
                    case (73):
                        spinSide = BitConverter.ToInt32(data, 0) / 100;
                        break;
                    case (74):
                        shootAng = BitConverter.ToInt32(data, 0) / 100;
                        break;
                    case (75):
                        directionAng = BitConverter.ToInt32(data, 0) / 100;
                        break;
                    case (76):
                        velocity = (BitConverter.ToInt32(data, 0));
                        break;
                }
                //by준희, 다음 값을 해독하기 위해 해독한 4바이트를 제거. 
                byteToArray.RemoveRange(0, 4);
            }
        }
        //by준희, 센서값을 생성자로 업데이트
        PNCVariable sendData = new PNCVariable(screenX, screenY, spinTop, spinSide, shootAng, directionAng, velocity);

        //by준희, 센서로 부터 값을 받으면 Unity안에서 게임 실행(Start함수와 같은 역할을 함)
        if (SceneManager.GetActiveScene().name == "TestScene")
        {
            GameManager.StartBall();    //by준희, 패널티킥 모드
        }
        else if (SceneManager.GetActiveScene().name == "PracticeMode")
        {
            Prac_SoccerPlayer.StartPlay();  //by준희, 연습모드
        }
    }

    /// <summary>
    /// by준희, 서버에 연결. 주어진 IP주소와 Port번호로 연결신호 전송.
    /// </summary>
    public void OnConnectedToServer()
    {
        if (socketReady == true)
        {
            //by준희, 소켓이 생성되면 이 메소드를 더이상 생성 하지 않음.
            Debug.Log("Socket is Ready(Stop Trying Connect)");
            return;
        }

        try
        {
            //by준희, 소켓 생성 하여 TCP프로토콜로 서버에 Connect요청
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            socketReady = true;
            
        }
        catch (Exception e)
        {
            Debug.Log("Soccket error :" + e.Message);
        }
    }

    private void Update()
    {

        //by준희, 소켓이 준비가 되었다면,
        if (socketReady)
        {
            //by준희, 스트림에 데이터가 있다면 소켓을 분석하는 ReceiveData() 실행
            if (stream.DataAvailable)
            {
                ReceiveData();
            }
        }
        //by준희, F11키를 눌러 서버에 연결
        if (Input.GetKeyDown(KeyCode.F11))
        {
            OnConnectedToServer();
        }

        //by준희, F12키를 눌러 서버에 끊기
        if (Input.GetKeyDown(KeyCode.F12))
        {
            stream.Close();
            socket.Close();
            socketReady = false;
        }
    }
    /// <summary>
    /// by준희, Stream으로부터 사용가능한 데이터를 받기.
    /// </summary>
    private void ReceiveData()
    {
        //by준희, 소켓의 버퍼사이즈만큼의 배열 생성
        byte[] streamByte = new byte[socket.ReceiveBufferSize];

        try
        {
            //by준희, streamByte 배열에 소켓에 있는 데이터를 읽어 넣기.
            //(0번 배열부터 streamByte의 마지막공간까지 넣기)
            stream.Read(streamByte, 0, streamByte.Length);
            //by준희, ArrayList생성 : 리스트를 활용해 메모리공간을 절약하기 위함. 
            byteToArray = new ArrayList();

            //by준희, 서버로 부터 받는 바이트 배열은 16진수 배열로 받기로 약속되어져 있음
            //첫번 째로 받은 바이트 배열[0]을 16진수 문자열로 변경해 "70"이면 
            //첫번째 센서값의 카테고리(스크린 X값)를 의미. 첫번째 카테고리를 알리는 
            //값을 찾아서 이후의 데이터를 해독.
            //byteToArray 리스트에 데이터를 추가.
            if (Convert.ToByte(streamByte[0]).ToString("X2") == "70")
            {
                for (int i = 0; i < streamByte.Length; i++)
                {
                    byteToArray.Add(streamByte[i]);
                }
                //by준희, byteToArray 리스트를 해독하는 메소드 실행
                DecodeByte();
            }
            //by준희, 사용한 stream을 비워 줌.
            stream.Flush();
        }
        catch (Exception e)
        {
            Debug.Log("Stream Read error :" + e.Message);
        }
    }
}
