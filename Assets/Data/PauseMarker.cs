using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class PauseMarker : Marker, INotification
{
    public PropertyName id => new PropertyName();
}
