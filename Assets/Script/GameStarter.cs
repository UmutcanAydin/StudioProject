using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameStarter : MonoBehaviour
{
    VideoPlayer vp;

    private void Awake()
    {
        vp = GetComponent<VideoPlayer>();
    }

    private void Start()
    {
        vp.loopPointReached += EndReached;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndReached(vp);
        }
    }

    void EndReached(VideoPlayer vp)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
