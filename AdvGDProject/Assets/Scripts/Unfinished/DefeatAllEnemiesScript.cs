using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefeatAllEnemiesScript : MonoBehaviour
{
    //Wall Variables
    public GameObject EntryWall;
    private Collider EntryWallCollider;
    public GameObject ExitWall;
    private Collider ExitWallCollider;
    public Material SolidWallTexture;
    public Material TransparentWallTexture;
    
    //Spawn Variables
    public GameObject EnemytoSpawn;

    public int WaveCount = 2;
    public Transform[] SpawnPoints;
    public int[] EnemiesPerWave;
    public float spawnDelay = 1f;

    private int RoomEnemyCount;
    private int CurrentWave = 0;
    private bool WaveCleared = false;
    private bool WaveActive = false; 
    private bool isTrapped = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EntryWallCollider = GetComponent<Collider>();
        ExitWallCollider = GetComponent<Collider>();
        SetWallState(EntryWall, EntryWallCollider, true, TransparentWallTexture);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTrapped == true && WaveActive == true)
        {
            UpdateRoomEnemyCount();
            if (WaveCleared == true && CurrentWave <= WaveCount)
            {
                Debug.Log("Autostart next wave.");
                StartCoroutine(StartNextWave());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I'm colliding!");
        if (other.CompareTag("Player") && isTrapped == false)
        {
            StartCoroutine(TrapPlayer());
        }
    }


    IEnumerator TrapPlayer()
    {
        yield return new WaitForSeconds(1f);
        isTrapped = true;
        EntryWallCollider.isTrigger = false;
        SetWallState(EntryWall, EntryWallCollider, true, SolidWallTexture);
        SetWallState(ExitWall, ExitWallCollider, true, TransparentWallTexture);
        StartCoroutine(StartNextWave());
    }


    IEnumerator StartNextWave()
    {
        WaveCleared = false;
        if (CurrentWave >= EnemiesPerWave.Length)
        {
            Debug.LogError("Current wave index out of bounds!");
            yield break;
        }
        yield return new WaitForSeconds(2f);
        Debug.LogError("It's a new wave!");
        int enemyCount = EnemiesPerWave[CurrentWave];
        WaveActive = true;
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (SpawnPoints.Length == 0 || EnemytoSpawn == null)
        {
            Debug.LogError("Spawn points or enemy prefab not assigned!");
            return;
        }
        Debug.Log("I'm spawning an enemy!");
        Transform spawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        GameObject enemy = Instantiate(EnemytoSpawn, spawnPoint.position, spawnPoint.rotation);
    }

    

    public void UpdateRoomEnemyCount()
    {
        RoomEnemyCount = GameObject.FindGameObjectsWithTag("RoomEnemy").Length;
        if (RoomEnemyCount == 0 && WaveCleared == false)
        {
            Debug.Log("Room cleared of all enemies.");
            CurrentWave ++;
            WaveCleared = true;
            WaveActive = false;
        }
    }


    IEnumerator OpenExit()
    {
        yield return new WaitForSeconds(1f);
        SetWallState(EntryWall, EntryWallCollider, false, TransparentWallTexture);
        isTrapped = false;
    }


    void SetWallState(GameObject wall, Collider collider, bool active, Material material)
    {
        Debug.Log("I'm changing a wall.");
        collider.enabled = true;
        if (wall.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
        {
            renderer.material = material;
        }
    }

}
