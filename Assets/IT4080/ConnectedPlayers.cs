using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace It4080 {
    /// <summary>
    /// This class creates and displays PlayerCards.  It is not aware of the
    /// network.  It has convenience methods for creating PlayerCards and
    /// accessing the cards via the id used when adding them.
    /// </summary>
    public class ConnectedPlayers : MonoBehaviour {
        public PlayerCard playerCardPrefab;
        private Hashtable playerCards = new Hashtable();

        /// <summary>
        /// Creates a PlayerCard and adds it to the display.  It populates the
        /// PlayerName and ClientId for the card.  The created card is returned.
        /// </summary>
        /// <param name="playerName">
        ///     The name of the player.
        /// </param>
        /// <param name="id">
        ///     The id for the player.  This should be the player's clientId
        /// </param>
        /// <returns>
        ///     The created PlayerCard with all properties set.
        /// </returns>
        public PlayerCard AddPlayer(string playerName, ulong id) {
            PlayerCard pd = null;
            if (playerCardPrefab == null) {
                Debug.Log("It is null, cannot make panel");
            } else {
                pd = Instantiate(playerCardPrefab);
                pd.transform.SetParent(transform);
                pd.SetPlayerName(playerName);
                pd.setClientId(id);
                playerCards.Add(id, pd);
            }
            return pd;
        }

        /// <summary>
        /// Destroys all PlayerCards that have been added via AddPlayer
        /// </summary>
        public void Clear() {
            playerCards.Clear();

            var kids = transform.GetComponentsInChildren<PlayerCard>();
            foreach (PlayerCard kid in kids) {
                Destroy(kid.gameObject);
            }
        }


        /// <summary>
        /// Returns all the PlayerCards that have been added via AddPlayer.
        /// </summary>
        /// <returns></returns>
        public PlayerCard[] GetPlayerCards() {
            return transform.GetComponentsInChildren<PlayerCard>();
        }


        /// <summary>
        /// Returns the player card for the given ID.  If the Id is not found
        /// then null is returned.
        /// </summary>
        /// <param name="id">
        /// The id used when calling AddPlayer
        /// </param>
        /// <returns>
        /// The PlayerCard for the id or null if it does not exist.
        /// </returns>
        public PlayerCard GetCardForId(ulong id) {
            PlayerCard toReturn = null;

            if (playerCards.Contains(id)) {
                toReturn = (PlayerCard)playerCards[id];
            }
            return toReturn;
        }
    }

}
