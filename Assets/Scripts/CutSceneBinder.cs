using UnityEngine;
using UnityEngine.Playables;

public class TimelineAutoBinder : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject player;
    public GameObject cameraTarget;

    void Start()
    {
        foreach (var output in director.playableAsset.outputs)
        {
            if (output.streamName == "PlayerTrack")
                director.SetGenericBinding(output.sourceObject, player);
            else if (output.streamName == "CameraTrack")
                director.SetGenericBinding(output.sourceObject, cameraTarget);
        }

        director.Play();
    }
}


