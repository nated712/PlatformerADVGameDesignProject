using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;



public class Goal : MonoBehaviour
{
    public GameObject ResultScreen;

    //Text Instances
    public TextMeshProUGUI PlayerTimeText;
    private float PlayerElapsedTime;
    public TextMeshProUGUI BestTimeText;
    private float BestElapsedTime = 100f;
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
        Debug.Log("Instance is null: " + (BestTimeManager.Instance == null));
        //BestTimeText.text = BestTimeManager.Instance.BestTimeRecordText;
    }

    void Update()
    {
        if (ResultScreen.gameObject.activeSelf == true && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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

            PlayerElapsedTime = timer.ReturnElapsedTime();
            PlayerTimeText.text = timer.ReturnTimerText(PlayerElapsedTime);
            Debug.Log(PlayerTimeText.text);
            

            //Did the Player beat their best time?
            float previousBest = BestTimeManager.Instance.BestTimeRecord;

            if (PlayerElapsedTime < BestTimeManager.Instance.BestTimeRecord)
            {
                BestTimeManager.Instance.SetBestTime(PlayerElapsedTime);
                BestTimeText.text = BestTimeManager.Instance.BestTimeRecordText;
                Debug.Log("New Best Time: " + BestTimeText.text);
            }
            else
            {
                BestTimeText.text =  BestTimeManager.Instance.BestTimeRecordText;
                Debug.Log("Did not beat best time. Current best: " + BestTimeText.text);
            }

            Debug.Log("The best time is:" + BestTimeText.text );

            Debug.Log("SThresholdTime: " + SThresholdTime);
            Debug.Log("AThresholdTime: " + AThresholdTime);
            Debug.Log("BThresholdTime: " + BThresholdTime);

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
