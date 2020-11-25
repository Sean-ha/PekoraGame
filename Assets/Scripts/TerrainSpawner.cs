using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner : MonoBehaviour
{
    private const float DISTANCE_TO_SPAWN = 15f;

    public GameObject startPlatform;
    public List<GameObject> platforms;

    private Transform lastEndPosition;
    public Transform currentTerrainParent;
    private Rigidbody2D currentRB;

    private float moveSpeed = -5;
    private float maxMoveSpeed = -14;

    private void Awake()
    {
        lastEndPosition = startPlatform.transform.Find("End");
        currentRB = currentTerrainParent.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        SpawnPlatform();
        startPlatform.transform.parent = currentTerrainParent;
        currentRB.velocity = new Vector2(moveSpeed, 0);

        InvokeRepeating("IncreaseSpeed", 1, 1);
        // Invoke("AddBuffNousagi", 45);
    }

    private void Update()
    {
        currentRB.velocity = new Vector2(moveSpeed, 0);
        if(lastEndPosition.position.x < DISTANCE_TO_SPAWN)
        {
            SpawnPlatform();
        }
    }

    // Creates a platform in the correct position, and updates the lastEndPosition
    private void SpawnPlatform()
    {
        GameObject randomPart = platforms[Random.Range(0, platforms.Count)];
        GameObject newSpawned = Instantiate(randomPart, lastEndPosition.position, Quaternion.identity);
        newSpawned.transform.parent = currentTerrainParent;
        lastEndPosition = newSpawned.transform.Find("End");
    }

    // Called every frame. This increases the speed of the platforms moving
    private void IncreaseSpeed()
    {
        moveSpeed -= 0.05f;
        if(moveSpeed < maxMoveSpeed)
        {
            CancelInvoke();
        }
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public void StopPlatforms()
    {
        CancelInvoke();
        moveSpeed = 0;
    }
}
