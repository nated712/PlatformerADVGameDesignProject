using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class Goal : MonoBehaviour
{

    public GameObject ResultScreen;

    //Text Instances
    public TextMeshProUGUI PlayerTimeText;
    private float PlayerElapsedTime;
    public TextMeshProUGUI BestTimeText;
    private float BestElapsedTime = 25f;
    public TextMeshProUGUI TurretsLeftText;
    public TextMeshProUGUI RankText;
    public TextMeshProUGUI BonusText;
    

    //Rank Times
    public float SThresholdTime = 12f;
    public float AThresholdTime = 16f;
    public float BThresholdTime = 23f;


    [SerializeField] private Timer timer; // Drag and drop Timer object in Inspector

    void Start()
    {
        ResultScreen.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player is tagged as "Player"
        {
            if (timer != null)
            {
                timer.StopTimer(); // Stop the timer
                Debug.Log("Player reached the goal! Timer stopped.");
            }

            // Generating Results
            int TurretsLeft = GameObject.FindGameObjectsWithTag("Turret").Length;
            TurretsLeftText.text = TurretsLeft.ToString();
                        //Bonus Determination
            if (TurretsLeft == 0)
            {
                timer.ReduceTime();
                BonusText.text = "All Turrets Destroyed: -00.05.00";
            }
            PlayerTimeText.text = timer.ReturnTimer();
            PlayerElapsedTime = timer.ReturnElapsedTime();
            


            //Did the Player beat their best time?
            if (BestElapsedTime > PlayerElapsedTime)
            {
                BestTimeText.text = PlayerTimeText.text;
            } else {
                BestTimeText.text = "00:99:000";
            }

            //Rank Determination
            if (PlayerElapsedTime < SThresholdTime)
            {
                RankText.text = "S";
            } else if (PlayerElapsedTime < AThresholdTime)
            {
                RankText.text = "A";
            } else if (PlayerElapsedTime < BThresholdTime)
            {
                RankText.text = "B";
            } else {
                RankText.text = "C";
            }
            ResultScreen.gameObject.SetActive(true);
        }
    }
}
