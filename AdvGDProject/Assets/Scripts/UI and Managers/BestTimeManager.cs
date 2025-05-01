using UnityEngine;

public class BestTimeManager : MonoBehaviour
{
    public static BestTimeManager Instance;


    public float BestTimeRecord;
    public string BestTimeRecordText = "Shouldn't see this.";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Debug.Log("BestTimeManager Awake called.");
        
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        BestTimeRecord = 99f;
        DontDestroyOnLoad(gameObject);
        Debug.Log("Best Time Start:" + BestTimeRecord);
        SetBestTime(BestTimeRecord);
    }

    public void SetBestTime(float NewBestTime)
    {
        Debug.Log(NewBestTime);
        BestTimeRecord = NewBestTime;
        int minutes = Mathf.FloorToInt(BestTimeRecord / 60);
        int seconds = Mathf.FloorToInt(BestTimeRecord % 60);
        int milliseconds = Mathf.FloorToInt((BestTimeRecord * 1000) % 1000); // Get milliseconds
        BestTimeRecordText = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        Debug.Log("BestTimeRecord after SetBestTime: " + BestTimeManager.Instance.BestTimeRecord);
        Debug.Log("BestTimeRecordText after SetBestTime: " + BestTimeManager.Instance.BestTimeRecordText);
    }

    public float GetBestTime()
    {
        return BestTimeRecord;
    }

    public string GetBestTimeText()
    {
        return BestTimeRecordText;
    }


    
}
