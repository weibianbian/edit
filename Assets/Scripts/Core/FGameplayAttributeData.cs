namespace RailShootGame
{
    //放置在AttributeSet中，创建一个可以使用FGameplayAttribute访问的属性。强烈建议使用这个而不是原始的float属性
    public class FGameplayAttributeData
    {
        public float BaseValue;
        public float CurrentValue;

        public FGameplayAttributeData(float DefaultValue)
        {
            BaseValue = DefaultValue;
            CurrentValue = DefaultValue;
        }
        public float GetBaseValue()
        {
            return BaseValue;
        }
        public float GetCurrentValue()
        {
            return CurrentValue;
        }
        //修改当前值，通常仅由能力系统调用或在初始化期间调用
        public void SetCurrentValue(float NewValue)
        {
            CurrentValue = NewValue;
        }
        //修改永久基值，通常只在技能系统或初始化时调用
        public void SetBaseValue(float NewValue)
        {
            BaseValue = NewValue;
        }
    }
}

