using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateAnimationBehaviour : StateMachineBehaviour
{
    [SerializeField]
    private float _animationLength;
    [SerializeField]
    private int _repsUntilAlternate = 1;
    private float _timeUntilAlternate;

    [SerializeField] private int _randomRepAmount;

    [SerializeField]
    private int _numberOfAnimations;

    private bool _isAlternating;
    private float _baseAnimationTime;
    private int _currentAnimation;

    [SerializeField]
    string _parameterName;

    [SerializeField, Min(0), Tooltip(
        "Si el historial de animacion es 1. La ultima animacion no tendra chances de repetirse. " +
        "Si es 2, las ultimas 2 animaciones no tendran chances de repetirse")]

    int _randomHistory = 1;

    HRandomElementSelector<int> _altSelector;

    private void Awake()
    {
        _timeUntilAlternate = _repsUntilAlternate * _animationLength;

        int[] altAnimations = new int[_numberOfAnimations - 1];
        for (int i = 0; i < altAnimations.Length; i++) 
            altAnimations[i] = i + 1;

        _altSelector = new HRandomElementSelector<int>(altAnimations, _randomHistory);
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_isAlternating)
        {
            _baseAnimationTime += Time.deltaTime;
            
            if (_baseAnimationTime > _timeUntilAlternate && stateInfo.normalizedTime % 1 < 0.02f)
            {
                _isAlternating = true;
                _currentAnimation = _altSelector.SelectRandomElement();

                animator.SetFloat(_parameterName, _currentAnimation);
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98f)
        {
            ResetIdle();
        }

        animator.SetFloat(_parameterName, _currentAnimation, 0.2f, Time.deltaTime);
    }

    private void ResetIdle()
    {
        _currentAnimation = 0;
        _isAlternating = false;
        _baseAnimationTime = 0;

        _timeUntilAlternate = (_repsUntilAlternate + Random.Range(0, _randomRepAmount)) * _animationLength;
    }
}
