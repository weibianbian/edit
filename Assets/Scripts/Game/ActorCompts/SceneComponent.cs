namespace RailShootGame
{
    public class SceneComponent : ActorComponent
    {
        public SceneComponent(Actor owner) : base(owner)
        {
        }
        protected void DestroyComponent()
        {

        }
    }
    public class DecalComponent : SceneComponent
    {
        public DecalComponent(Actor owner) : base(owner)
        {
        }

        public void SetLifeSpan(float LifeSpan)
        {
            if (LifeSpan > 0)
            {
                //GetWorld()->GetTimerManager().SetTimer(TimerHandle_DestroyDecalComponent, this, &UDecalComponent::LifeSpanCallback, LifeSpan, false);
            }
            else
            {
                //GetWorld()->GetTimerManager().ClearTimer(TimerHandle_DestroyDecalComponent);
            }
        }
        void LifeSpanCallback()
        {
            DestroyComponent();

            //if (bDestroyOwnerAfterFade && (FadeDuration > 0.0f || FadeStartDelay > 0.0f))
            //{
            //    if (AActor * Owner = GetOwner())
            //    {
            //        Owner->Destroy();
            //    }
            //}
        }
    }
}

