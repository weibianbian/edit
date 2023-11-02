using GameplayAbilitySystem;
using UnityEditor.PackageManager.Requests;
using UnityEditor.Search;

namespace RailShootGame
{
    public class AAIController : AController
    {
        public UPathFollowingComponent PathFollowingComponent;
        public bool bAllowStrafe = false;
        public AAIController()
        {
            PathFollowingComponent = ReferencePool.Acquire<UPathFollowingComponent>();
            PathFollowingComponent.SetOwner(this);
            PathFollowingComponent.PostInitProperties();
        }
        public void RequestPathAndMove()
        {
            PathData FoundPath = new PathData();
            FindPathForMoveRequest(FoundPath);
        }
        public void FindPathForMoveRequest(PathData InPath)
        {

        }
        public void RequestMove(PathData Path)
        {
            PathFollowingComponent.RequestMove(Path);
        }
    }
}

