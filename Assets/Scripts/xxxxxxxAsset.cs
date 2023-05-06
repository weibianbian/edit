using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class xxxxxxxAsset : PlayableAsset, ITimelineClipAsset
{
    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var scriptPlayable = ScriptPlayable<xxxxxxxAssetBehaviour>.Create(graph);
        UnityEngine.Debug.Log(owner);
        return scriptPlayable;
    }
}
public class xxxxxxxAssetBehaviour : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        //UnityEngine.Debug.Log("ProcessFrame");
        base.ProcessFrame(playable, info, playerData);
    }
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        UnityEngine.Debug.Log("OnBehaviourPlay");
        base.OnBehaviourPlay(playable, info);
    }
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        UnityEngine.Debug.Log("OnBehaviourPause");
        base.OnBehaviourPause(playable,info);
    }
    public override void OnGraphStart(Playable playable)
    {
        UnityEngine.Debug.Log("OnGraphStart");
        base.OnGraphStart(playable);
    }
    public override void OnPlayableCreate(Playable playable)
    {
        UnityEngine.Debug.Log("OnPlayableCreate");
        base.OnPlayableCreate(playable);
    }
}
