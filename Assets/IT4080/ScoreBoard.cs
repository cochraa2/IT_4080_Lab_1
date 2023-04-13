using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace It4080
{
    public class ScoreBoard : MonoBehaviour
    {
        public PlayerCard playerDataPrefab;

        public GameObject row1;



        // Start is called before the first frame update
        void Awake()
        {
            GameObject.Destroy(row1.transform.Find("SampleData1").gameObject);
            GameObject.Destroy(row1.transform.Find("SampleData2").gameObject);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void AddPlayer(CarScript player, string name)
        {
            PlayerCard pd = Instantiate(playerDataPrefab);
            pd.transform.SetParent(row1.gameObject.transform, false);

            pd.SetPlayerName(name);
            //pd.SetScore(player.netScore.Value);            
        }
    }

}
