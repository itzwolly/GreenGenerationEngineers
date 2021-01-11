using UnityEngine;

public class TreeGrowthBehaviour : MonoBehaviour {
    [SerializeField] private Animator _animator;
    [SerializeField] private EnergyIndicatorScript _energyIndicator;

    private float _cachedFillAmount;
    
    private void Start() {
        _animator.SetFloat("playMode", 1);
        _cachedFillAmount = short.MaxValue;
    }

    private void Update() {
        UpdateAnimatorSpeed();
        CheckWin();
    }

    private void UpdateAnimatorSpeed() {
        if (_energyIndicator.GetFillAmount() != 0){
            if (_cachedFillAmount < _energyIndicator.GetFillAmount()) {
                _animator.SetFloat("playMode", 0);
            } else {
                _animator.SetFloat("playMode", 1);
            }

            _animator.SetFloat("normalizedTime", _energyIndicator.GetFillAmount());
            _cachedFillAmount = _energyIndicator.GetFillAmount();
        } else {
            _animator.SetFloat("normalizedTime", 0);
        }
    }
    
    private void CheckWin() {
        if (TouchScript.Instance.HasWon()) {
            if (!_animator.GetBool("hasWon")) {
                _animator.SetBool("hasWon", true);
            }
        }
    }
}
