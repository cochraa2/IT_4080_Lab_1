using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace It4080
{
    public class Chat : NetworkBehaviour
    {
        const ulong SYSTEM_ID = 9999999;
        public TMPro.TMP_Text txtChatLog;
        public Button btnSend;
        public TMPro.TMP_InputField inputMessage;

        ulong[] singleClientId = new ulong[1];


        public void Start()
        {
            btnSend.onClick.AddListener(ClientOnSendClicked);
            inputMessage.onSubmit.AddListener(ClientOnInputSubmit);
        }

        public override void OnNetworkSpawn()
        {
            txtChatLog.text = "---- Start Chat Log ----";
            if (IsHost)
            {
                DisplayMessageLocally($"Hello there Host! You are Player #{NetworkManager.Singleton.LocalClientId}", SYSTEM_ID);
                NetworkManager.Singleton.OnClientConnectedCallback += HostOnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += HostOnClientDisconnected;
            }
            else
            {
                DisplayMessageLocally($"Welcome to the room! You are Player #{NetworkManager.Singleton.LocalClientId}!", SYSTEM_ID);
                NetworkManager.Singleton.OnClientConnectedCallback += ClientOnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += ClientOnClientDisconnected;
            }
        }

        private void SendUIMessage()
        {
            string msg = inputMessage.text;
            inputMessage.text = "";
            SendChatMessageServerRpc(msg);
            inputMessage.ActivateInputField();
        }


        private void SendDirectMessage(string message, ulong from, ulong to)
        {
            ClientRpcParams rpcParams = default;
            rpcParams.Send.TargetClientIds = singleClientId;

            singleClientId[0] = from;
            SendChatMessageClientRpc($"<whisper> {message}", from, rpcParams);

            singleClientId[0] = to;
            SendChatMessageClientRpc($"<whisper> {message}", from, rpcParams);
        }


        //-----------------------
        // Events
        //-----------------------

        public void ClientOnSendClicked()
        {
            SendUIMessage();
        }

        public void ClientOnInputSubmit(string text)
        {
            SendUIMessage();
        }

        private void HostOnClientConnected(ulong clientId)
        {
            DisplayMessageLocally($"Player #{clientId} has connected!", SYSTEM_ID);
        }

        private void HostOnClientDisconnected(ulong clientId)
        {
            DisplayMessageLocally($"Player #{clientId} has disconnected :(", SYSTEM_ID);
        }

        private void ClientOnClientConnected(ulong clientId)
        {
            DisplayMessageLocally($"Player #{clientId} has connected!", SYSTEM_ID);
        }

        private void ClientOnClientDisconnected(ulong clientId)
        {
            DisplayMessageLocally($"Player #{clientId} has disconnected :(", SYSTEM_ID);
        }


        //----------------------
        // RPC
        //----------------------
        [ClientRpc]
        public void SendChatMessageClientRpc(string message, ulong from, ClientRpcParams clientRpcParams = default)
        {
            DisplayMessageLocally(message, from);
        }


        [ServerRpc(RequireOwnership = false)]
        public void SendChatMessageServerRpc(string message, ServerRpcParams serverRpcParams = default)
        {
            Debug.Log($"Host got message:  {message}");

            if (message.StartsWith("@"))
            {
                string[] parts = message.Split(" ");
                string clientIdStr = parts[0].Replace("@", "");
                ulong toClientId = ulong.Parse(clientIdStr);

                SendDirectMessage(message, serverRpcParams.Receive.SenderClientId, toClientId);
            }
            else
            {
                SendChatMessageClientRpc(message, serverRpcParams.Receive.SenderClientId);
            }

        }

        public void DisplayMessageLocally(string message, ulong from)
        {
            Debug.Log(message);
            string who;

            if (from == NetworkManager.Singleton.LocalClientId)
            {
                who = "You";
            }
            else if (from == SYSTEM_ID)
            {
                who = "System";
            }
            else
            {
                who = $"{from}";
            }

            string newMessage = $"\n[{who}]:  {message}";
            txtChatLog.text += newMessage;
        }
    }
}