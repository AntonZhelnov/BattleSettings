using UnityEngine;

namespace Common.Animating
{
    public class AnimationPlayer
    {
        private readonly Animator _animator;
        private int _triggerHash;
        private string _triggerName;


        public AnimationPlayer(Animator animator)
        {
            _animator = animator;
        }

        public void Play(string triggerName)
        {
            if (_triggerName != triggerName)
            {
                _triggerName = triggerName;

                _triggerHash =
                    triggerName.Length > 0
                        ? Animator.StringToHash(triggerName)
                        : 0;
            }

            _animator.SetTrigger(_triggerHash);
        }
    }
}