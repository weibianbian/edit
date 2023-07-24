using RailShootGame;

namespace GameplayAbilitySystem
{
    public class GameplayAbility
    {
        //	The important functions:
        //	
        //		CanActivateAbility()	-   Const函数检查ability是否可激活。可由UI等调用
        //
        //		TryActivateAbility()	- 尝试激活该能力。调用CanActivateAbility()。输入事件可以直接调用它。.
        //								- 还处理每个执行的实例化逻辑和复制/预测调用.
        //		
        //		CallActivateAbility()	- Protected, non virtual function. Does some boilerplate 'pre activate' stuff, then calls ActivateAbility()
        //
        //		ActivateAbility()		- What the abilities *does*. This is what child classes want to override.
        //	
        //		CommitAbility()			- Commits reources/cooldowns etc. ActivateAbility() must call this!
        //		
        //		CancelAbility()			- Interrupts the ability (from an outside source).
        //
        //		EndAbility()			- The ability has ended. This is intended to be called by the ability to end itself.
        public virtual bool CanActivateAbility()
        {
            return false;
        }
        public virtual bool ActivateAbility(Character owner)
        {
            return false;
        }
        public virtual void CancelAbility()
        {

        }
        public virtual bool CommitAbility()
        {
            return false;
        }
    }
}

