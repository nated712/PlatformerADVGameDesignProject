using UnityEngine;

public class DefeatAllEnemiesScript : MonoBehaviour
{

    public GameObject EscapeWall;
    public GameObject EntryWall;
    public Transform BoxFloor;

    Bounds RoomBounds;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       EntryWall.GetComponent<Collider>().enabled = false;
       RoomBounds = new Bounds(BoxFloor.GetComponent<Renderer>().bounds.center, new Vector3(20,20,20));

    }
//transform.renderer.bounds.center
    // Update is called once per frame
    void Update()
    {
    }
}
