using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace UNITY_J1MER
{
    public class Login : MonoBehaviour
    {
        TcpClient clientOne;
        TcpClient clientTwo;


        StreamReader reader;
        StreamWriter writer;
        string server = "127.0.0.1";

        public Player playerOne;
        public Player playerTwo;

        public Text login { get; set; }
        public Text password { get; set; }

        public void Connect()
        {
            try
            {
                if (clientOne != null || clientTwo != null)
                {
                    return;
                }
                int port = 5000;

                clientOne = new TcpClient(server, port);
                clientTwo = new TcpClient(server, port);


                this.playerOne.Enter(clientOne);
                this.playerTwo.Enter(clientTwo);


            }
            catch (IOException e)
            {
                Console.WriteLine("erro de conexão {0}", e);
            }
        }
        public void CreateUser()
        {
            Player p = PlayerBuld();

            PlayerController pController = new PlayerController();

            pController.Save(p);
        }

        private Player PlayerBuld()
        {
            Player p = new Player();
            p.login = this.login.text;
            p.password = this.password.text;
            return p;
        }
    }
}