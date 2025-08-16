using UnityEngine;
using UnityEngine.Playables;

public class PauseMarkerReceiver : MonoBehaviour, INotificationReceiver
{
    public PlayableDirector director;

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is PauseMarker)
        {
            director.Pause();
            Debug.Log("Timeline Paused at marker!");
        }
    }
}
