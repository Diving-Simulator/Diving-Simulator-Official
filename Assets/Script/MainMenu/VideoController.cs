using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer vp;

    void Start()
    {
        vp.Play();
    }
}
