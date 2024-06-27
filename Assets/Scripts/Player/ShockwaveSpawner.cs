using UnityEngine;
using System.Collections;


//this was made by chatgpt so i barely need to comment it!!
//in fact im lazy and i wont comment it at all
//i edited it a bit tho its not all chatgpt
public class ShockwaveSpawner : MonoBehaviour
{
    public GameObject shockwavePrefab; // Assign the Shockwave prefab in the Inspector
    public float spawnInterval = 0.3f; // Time between each pair of shockwave spawns
    public float lifetime = 1.0f; // Time before shockwave despawns
    public float initialScale = 1.0f; // Scale of the closest shockwaves
    public float scaleDecrement = 0.2f; // Scale decrement for each subsequent shockwave pair
    public float horizontalOffset = 1.0f; // Horizontal offset from the player
    public float verticalOffset = 1.0f; // Vertical distance below the player
    public float riseSpeed = 1f;
    public Vector3 shockwavePosition;

    void Start()
    {
        shockwavePrefab = Resources.Load<GameObject>("Prefabs/Player/Shockwave");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(SpawnShockwaves());
        }
    }

    IEnumerator SpawnShockwaves()
    {
        shockwavePosition = transform.position;
        for (int i = 0; i < 3; i++)
        {
            float currentHorizontalOffset = horizontalOffset * (i + 1);
            float scale = initialScale - (scaleDecrement * i);
            float verticalPosition = shockwavePosition.y - verticalOffset * scale; // Adjust based on scale
            
            Vector3 leftPosition = new Vector3(shockwavePosition.x - currentHorizontalOffset, verticalPosition, shockwavePosition.z);
            Vector3 rightPosition = new Vector3(shockwavePosition.x + currentHorizontalOffset, verticalPosition, shockwavePosition.z);

            // Create shockwaves
            GameObject leftShockwave = Instantiate(shockwavePrefab, leftPosition, Quaternion.identity);
            GameObject rightShockwave = Instantiate(shockwavePrefab, rightPosition, Quaternion.identity);

            // Set scales (height is 2x the width)
            leftShockwave.transform.localScale = new Vector3(scale, scale * 2, scale);
            rightShockwave.transform.localScale = new Vector3(scale, scale * 2, scale);

            // Start rising
            StartCoroutine(RiseAndDespawn(leftShockwave, verticalPosition, 2 - i));
            StartCoroutine(RiseAndDespawn(rightShockwave, verticalPosition, 2 - i));

            // Wait for the next spawn interval
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator RiseAndDespawn(GameObject shockwave, float initialYPosition, int i)
    {
        float elapsedTime = 0;
        Vector3 initialPosition = shockwave.transform.position;
        Vector3 targetPosition = new Vector3(initialPosition.x, shockwavePosition.y - GetComponent<SpriteRenderer>().bounds.size.y / 2, initialPosition.z);

        // Rising animation
        while (elapsedTime < spawnInterval)
        {
            shockwave.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / spawnInterval);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Keep shockwave at the target position for the rest of its lifetime
        shockwave.transform.position = targetPosition;
        yield return new WaitForSeconds(lifetime + spawnInterval * i);

        // Despawn
        Destroy(shockwave);
    }
}
