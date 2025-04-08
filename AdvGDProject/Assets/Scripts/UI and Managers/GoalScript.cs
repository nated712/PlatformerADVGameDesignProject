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
    public float BThresholdTime = 20f;


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
            TurretsLeftText.text = GameObject.FindGameObjectsWithTag("Turret").Length.ToString();
            PlayerTimeText.text = timer.ReturnTimer();
            PlayerElapsedTime = timer.ReturnElapsedTime();

            //Did the Player beat their best time?
            if (BestElapsedTime > PlayerElapsedTime)
            {
                BestTimeText.text = timer.ReturnTimer();
            } else {
                BestTimeText.text = "00:25:000";
            }

            //Rank Determination
            if (PlayerElapsedTime > SThresholdTime)
            {
                RankText.text = "S";
            } else if ((SThresholdTime > PlayerElapsedTime) && (PlayerElapsedTime > AThresholdTime))
            {
                RankText.text = "A";
            } else if ((AThresholdTime > PlayerElapsedTime) && (PlayerElapsedTime > BThresholdTime))
            {
                RankText.text = "B";
            } else {
                RankText.text = "C";
            }

            //Bonus Determination
            if (TurretsLeftText.text == "0")
            {
                BonusText.text = "All Turrets Destroyed: -00.05.00";
            }

            ResultScreen.gameObject.SetActive(true);
        }
    }
}
