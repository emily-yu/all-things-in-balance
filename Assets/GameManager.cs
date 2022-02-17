using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
 using UnityEngine.SceneManagement;
 using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject enemy;
    private AudioSource audio;
    // public AudioClip resetSound;
    public bool isRotating;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = Vector3.zero; // init no gravity
        isRotating = false;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // GetComponent<AudioSource>().Play();
        var speed = 10;
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isRotating) {
            // if isn't rotating, then can add new block
            Vector3 relPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(relPosition);
            Vector3 adjustZ = new Vector3(worldPoint.x, worldPoint.y, enemy.transform.position.z);
            Instantiate(enemy).transform.position = adjustZ;
            isRotating = true;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0)) { // if rotating, then don't add new mouse thing
            Debug.Log("rotate");
            // transform.Rotate(Vector3.up * speed * Time.deltaTime); // [todo] add rotation
            // enemy.GetComponent<AudioSource>().Play();
        }
        else if (Input.GetKeyDown(KeyCode.Return) && isRotating) {
            Debug.Log("release gravity");
            Physics.gravity = new Vector3(0, -10, 0); // reset to standard gravity
            audio.Play();

            // add scoring functionality here
            Debug.Log("score");

            Thread.Sleep(5000); // let it just fall
            isRotating = false;
        }

        // reload scene with key R
        if (Input.GetKeyDown(KeyCode.R)){
             Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
         }
    }
}
 