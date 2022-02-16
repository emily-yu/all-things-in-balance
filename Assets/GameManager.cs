using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemy;
    public bool isRotating;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = Vector3.zero; // init no gravity
        isRotating = false;
    }

    // Update is called once per frame
    void Update()
    {
        var speed = 10;
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (!isRotating) { // if isn't rotating, then can add new block
                Vector3 relPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(relPosition);
                Vector3 adjustZ = new Vector3(worldPoint.x, worldPoint.y, enemy.transform.position.z);
                Instantiate(enemy).transform.position = adjustZ;
            }
            // if rotating, then don't add new mouse thing
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Space");
            transform.Rotate(Vector3.up * speed * Time.deltaTime); // [todo] add rotation
        }
        else if (Input.GetKeyDown(KeyCode.Return)) {
            Debug.Log("release gravity");
            Physics.gravity = new Vector3(0, -10, 0); // reset to standard gravity
            isRotating = false;
        }
    }
}
 