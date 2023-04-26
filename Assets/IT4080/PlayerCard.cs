using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static It4080.ChatUi;

namespace It4080 {
    /// <summary>
    /// This is a simple UI element that represents a Player.  The card itself
    /// is just for display.  It is not "smart".  It does not know about the
    /// network or any other objects.
    ///
    /// Each card has the following elements
    /// * PlayerName label (see SetPlayerName)
    /// * Status label (see SetStatus)
    /// * Score label (see SetScore)
    /// * A kick button.  This button will trigger the KickPlayer event when
    ///   pressed.  It does do anything else.
    /// * A ready button.  This button will trigger the ReadyToggled event when
    ///   pressed.  It does not do anything else.
    ///
    /// The card has two events which are fired when buttons are pressed.
    /// * ReadyToggled(bool) - emitted when the ready button is pressed.  The
    ///   event passes a boolean representing if the card's state is ready or
    ///   not.
    /// * KickPlayer(ulong) - eitted when the kick button is pressed.  It passes
    ///   the id property for the card.
    /// </summary>
    public class PlayerCard : MonoBehaviour {
        TMPro.TMP_Text lblPlayerName;
        TMPro.TMP_Text lblScore;
        TMPro.TMP_Text lblStatus;
        Button btnReady;
        Button btnKick;

        public event Action<bool> ReadyToggled;
        public event Action<ulong> KickPlayer;

        private bool isReady = false;
        private ulong clientId = ulong.MaxValue;

        // Start is called before the first frame update
        void Awake() {
            lblPlayerName = transform.Find("Row1/LblPlayerName").GetComponent<TMPro.TMP_Text>();
            lblScore = transform.Find("Row1/LblScore").GetComponent<TMPro.TMP_Text>();
            btnKick = transform.Find("Row1/BtnKick").GetComponent<Button>();
            btnKick.onClick.AddListener(OnBtnKickClicked);

            lblStatus = transform.Find("Row2/LblStatus").GetComponent<TMPro.TMP_Text>();
            btnReady = transform.Find("Row2/BtnReady").GetComponent<Button>();
            btnReady.onClick.AddListener(OnBtnReadyClicked);

            // Force update of buttons and any other actions taken
            // when ready is set.
            SetReady(isReady);
            lblPlayerName.text = "<Not Set>";
            SetScore(-1);
        }

        // --------------------------------------
        // Events
        // --------------------------------------
        private void OnBtnReadyClicked() {
            SetReady(!isReady);
            if (ReadyToggled != null) {
                ReadyToggled.Invoke(isReady);
            }
        }


        private void OnBtnKickClicked() {
            if (KickPlayer != null) {
                KickPlayer.Invoke(clientId);
            }
        }


        // --------------------------------------
        // Accessors
        // --------------------------------------

        /// <summary>
        /// Sets the score label text to the value passed in.
        /// </summary>
        /// <param name="newScore"></param>
        public void SetScore(int newScore) {
            lblScore.text = $"{newScore} "; // hack in a space
        }

        /// <summary>
        /// Show/Hide the score label.
        /// </summary>
        /// <param name="should"></param>
        public void ShowScore(bool should)
        {
            lblScore.gameObject.SetActive(should);
        }


        /// <summary>
        /// Sets the text of the Player Name label
        /// </summary>
        /// <param name="newName"></param>
        public void SetPlayerName(string newName) {
            lblPlayerName.text = newName;
        }


        /// <summary>
        /// Sets if this card is ready or not.  The value is represented by the
        /// text of the Ready button.
        /// </summary>
        /// <param name="ready"></param>
        public void SetReady(bool ready) {
            isReady = ready;
            if (isReady) {
                btnReady.GetComponentInChildren<TextMeshProUGUI>().text = "Set Not Ready";
            } else {
                btnReady.GetComponentInChildren<TextMeshProUGUI>().text = "Set Ready";
            }
        }


        /// <summary>
        /// Sets the internal id for the card.  This value is not displayed.
        /// </summary>
        /// <param name="id"></param>
        public void setClientId(ulong id) {
            clientId = id;
        }


        /// <summary>
        /// Returns the id for the card
        /// </summary>
        /// <returns></returns>
        public ulong getClientId() {
            return clientId;
        }


        /// <summary>
        /// Show/hide the kick button.
        /// </summary>
        /// <param name="should"></param>
        public void ShowKick(bool should) {
            btnKick.gameObject.SetActive(should);
        }


        /// <summary>
        /// Show/hide the ready button.
        /// </summary>
        /// <param name="should"></param>
        public void ShowReady(bool should) {
            btnReady.gameObject.SetActive(should);
        }


        /// <summary>
        /// Sets the text of the status label.  The status is not "smart".  It
        /// is does not automatically reflect the state of the card and must
        /// be set externally.  There are no predefined states, you can set
        /// this value to anything.
        /// </summary>
        /// <param name="newStatus"></param>
        public void SetStatus(string newStatus)
        {
            lblStatus.text = $"Status:  {newStatus}";
        }
    }
}
