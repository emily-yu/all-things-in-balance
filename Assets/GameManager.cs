using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Vector3 relPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(relPosition);
            Vector3 adjustZ = new Vector3(worldPoint.x, worldPoint.y, enemy.transform.position.z);
            Instantiate(enemy).transform.position = adjustZ;
        }
    }
}
 