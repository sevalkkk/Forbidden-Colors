using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    Vector3 last_mouse_pos;
    public float move_speed;
    public float sensitivity = 0.16f;
    public float clamp_delta = 42f;
    Vector3 pos = new Vector3(0, 0);
    public float bounds = 5;
    public  bool can_move, game_over, finish;
    public GameObject LevelText;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -bounds, bounds), transform.position.y, transform.position.z);
        if(can_move)
        { transform.position += FindObjectOfType<CameraController>().camera_velocity; }
        
       

        if(!can_move && game_over )
        {
            
            if(Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else if (!can_move && !finish)
        {
            if (Input.GetMouseButtonDown(0)) { can_move = true;
                StartCoroutine(Coroutine());
            }
        }
    }
    IEnumerator Coroutine()
    {
        yield return new WaitForSeconds(1f);
        LevelText.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(Input.GetMouseButtonDown(0))
        {
            last_mouse_pos = Input.mousePosition;
        }

        if(can_move && !finish)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 v = last_mouse_pos - Input.mousePosition;
                last_mouse_pos = Input.mousePosition;
                v = new Vector3(v.x, 0, v.y);

                Vector3 move_force = Vector3.ClampMagnitude(v, clamp_delta);
                rb.AddForce(-move_force * sensitivity - rb.velocity / 5f, ForceMode.VelocityChange);
            }
        }

        

        rb.velocity.Normalize();

        
    }
    private void OnCollisionEnter(Collision target)
    {
        if(target.gameObject.tag== "Enemy")
        {
            GameOver();
        }
    }

    IEnumerator NextLevel()
    {
        finish = true;
        can_move = false;
        //last scene
        if( SceneManager.GetActiveScene().name == "Level3")
        {
            winPanel.SetActive(true);
            
        }
        else
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);

            yield return new WaitForSeconds(1);
            SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("Level"));
        }
        
    }
    private void GameOver()
    {
        can_move = false;
        game_over = true;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        gameOverPanel.SetActive(true);
    }

    public void ClickPlayAgainButton()
    {
        SceneManager.LoadScene("Level1");

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "finishLine")
        {
            StartCoroutine(NextLevel());
        }
    }
}
