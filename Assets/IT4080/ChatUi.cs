using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System;
using System.Net;

namespace It4080 {
    /// <summary>
    /// This is a simple control for inputting and displaying messages.  This
    /// is a "dumb" control that only displays messages you tell it to.  It has
    /// some convenience methods for formatting text.  It also has the
    /// OnTextSubmitted event that is fired when text is entered into the
    /// input box of the control.  Inputted text is not displayed automatically,
    /// you must connect to the event and then handle the input yourself.
    /// </summary>
    public class ChatUi : MonoBehaviour
    {
        public const string MSG_SYSTEM = "SYSTEM";
        public Button btnSend;
        public TMPro.TMP_InputField inputMessage;
        public TMPro.TMP_Text txtChatLog;

        /// <summary>
        /// Emitted when text is entered into the input box on the control and
        /// then the send button is pressed or "enter" is pressed.  The entered
        /// text is sent with the signal.
        /// </summary>
        public event Action<string> OnTextSubmitted;

        public void Start() {
            btnSend.onClick.AddListener(BtnSendOnClick);
            inputMessage.onSubmit.AddListener(InputMessageOnSubmit);
            txtChatLog.text = "Super Chat 64 Plus v2\n ";
        }


        // -----------------------------
        // Private
        // -----------------------------
        private void SendMessage()
        {            
            if(OnTextSubmitted != null) {
                OnTextSubmitted.Invoke(inputMessage.text);
            }
            inputMessage.text = "";
        }


        private void InputMessageOnSubmit(string text)
        {
            SendMessage();
        }


        private void BtnSendOnClick()
        {
            SendMessage();
        }


        // ----------------
        // Public
        // ----------------

        /// <summary>
        /// Enables/disables the input box and send button.
        /// </summary>
        /// <param name="should"></param>
        public void enable(bool should = true)
        {
            inputMessage.enabled = should;
            btnSend.enabled = should;
        }


        /// <summary>
        /// Displays the text passed in.  A line feed is added to the end of
        /// the passed in string.
        /// </summary>
        /// <param name="message"></param>
        public void DisplayText(string message)
        {
            txtChatLog.text += $"{message}\n";
        }


        /// <summary>
        /// Formats the text to be displayed to contain who it is from and
        /// the message.
        /// </summary>
        /// <param name="from">String representing who sent the message</param>
        /// <param name="message">The message to display</param>
        public void AddMessage(string from, string message) {
            DisplayText($"[{from}] {message}");
        }


        /// <summary>
        /// Override to display a clientId as the from instead of a string.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="message"></param>
        public void AddMessage(ulong from, string message) {
            AddMessage(from.ToString(), message);
        }
    }
}