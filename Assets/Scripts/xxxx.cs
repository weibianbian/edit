using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(xxxxxxxAsset))]
[ExcludeFromPreset]
public class xxxxTrack : TrackAsset
{
    public int xixi = 0;
    protected override void OnCreateClip(TimelineClip clip)
    {
        clip.displayName = "实例";
        base.OnCreateClip(clip);
    }
}
