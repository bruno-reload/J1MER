using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UNITY_J1MER
{
    public class Login : MonoBehaviour
    {
        TcpClient clientOne;
        TcpClient clientTwo;

        public GameObject gameManager;

        string server = "127.0.0.1";

        public Player playerOne;
        public Player playerTwo;

        public GameObject login;
        public GameObject password;
        public GameObject register;

        private PlayerController pc;
        private MatchController mc;
        private void Start()
        {
            pc = new PlayerController();
            mc = new MatchController();
        }

        public void Connect()
        {
            try
            {
                if (clientOne != null || clientTwo != null)
                {
                    return;
                }
                int port = 5000;

                PlayerData pData = Check();

                if (pData == null)
                {
                    register.SetActive(true);
                    return;
                }

                clientOne = new TcpClient(server, port);
                clientTwo = new TcpClient(server, port);

                pData.matchId = mc.Save(pData.id);

                this.playerOne.Enter(clientOne, pData);
                this.playerTwo.Enter(clientTwo);

                if (playerTwo.GetComponent<Player>().tcpClienteId == 1)
                {
                    gameManager.GetComponent<GameManager>().CreateConnections();
                    gameManager.GetComponent<GameManager>().CreateElements();
                    gameManager.GetComponent<GameManager>().DefineElements();
                }
                if (playerTwo.GetComponent<Player>().tcpClienteId == 3)
                {
                    gameManager.GetComponent<GameManager>().CreateConnections();
                    gameManager.GetComponent<GameManager>().CreateElements();
                    gameManager.GetComponent<GameManager>().DefineElements();
                    gameManager.GetComponent<GameManager>().LoadEllements();
                    gameManager.GetComponent<GameManager>().PositionRandomDraw();
                    gameManager.GetComponent<GameManager>().Write();

                    SwapPlayerPosition();
                    SwapPlayerMaterial();

                    playerTwo.GetComponent<Player>().pData.startGame = true;
                }

                gameObject.SetActive(false);
            }
            catch (IOException e)
            {
                Console.WriteLine("erro de conexão {0}", e);
            }
        }
        private void SwapPlayerMaterial()
        {
            Material material = this.playerOne.gameObject.GetComponentInChildren<MeshRenderer>().material;
            this.playerOne.gameObject.GetComponentInChildren<MeshRenderer>().material = this.playerTwo.gameObject.GetComponentInChildren<MeshRenderer>().material;
            this.playerTwo.gameObject.GetComponentInChildren<MeshRenderer>().material = material;
        }
        private void SwapPlayerPosition()
        {
            Vector3 position = this.playerOne.transform.position;
            Quaternion rotation = this.playerOne.transform.rotation;

            this.playerOne.transform.SetPositionAndRotation(this.playerTwo.transform.position, this.playerTwo.transform.rotation);
            this.playerTwo.transform.SetPositionAndRotation(position, rotation);
        }
        private PlayerData Check()
        {
            String log = login.GetComponent<Text>().text;
            String pwd = password.GetComponent<Text>().text;

            return pc.Load(log, pwd);
        }
        public void CreateUser()
        {

            var log = login.GetComponent<Text>().text;
            var pwd = password.GetComponent<Text>().text;

            pc.Save(new PlayerData(log, pwd));
        }
        public void HideRegisterButton()
        {
            register.SetActive(false);
        }
    }
}