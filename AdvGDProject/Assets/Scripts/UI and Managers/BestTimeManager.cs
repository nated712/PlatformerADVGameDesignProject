using UnityEngine;

public class BestTimeManager : MonoBehaviour
{
    public static BestTimeManager Instance;


    public float BestTimeRecord = 100f;
    public string BestTimeRecordText = "Shouldn't see this.";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetBestTime(float NewBestTime)
    {
        BestTimeRecord = NewBestTime;
        int minutes = Mathf.FloorToInt(BestTimeRecord / 60);
        int seconds = Mathf.FloorToInt(BestTimeRecord % 60);
        int milliseconds = Mathf.FloorToInt((BestTimeRecord * 1000) % 1000); // Get milliseconds
        BestTimeRecordText = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        
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
