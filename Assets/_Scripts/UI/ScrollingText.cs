using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollingText : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] private TextMeshProUGUI _itemInfoText;

    [SerializeField] ObjectiveTextSO _startText;
    ObjectiveTextSO _currentObjectiveText;

    private void Start()
    {
        if(_startText != null)
            ActivateText(_startText);
    }

    public void ActivateText(ObjectiveTextSO objectiveText)
    {
        _currentObjectiveText = objectiveText;
        StopAllCoroutines();
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        gameObject.SetActive(true);

        _itemInfoText.text = "";
        for (int i = 0; i < _currentObjectiveText.text.Length; i++)
        {
            _itemInfoText.text += _currentObjectiveText.text[i];
            yield return new WaitForSeconds(_currentObjectiveText.typeSpeed);
        }

        yield return new WaitForSeconds(_currentObjectiveText.duration);
        
        if (_currentObjectiveText.nextText != null)
            ActivateText(_currentObjectiveText.nextText);
        else if (_currentObjectiveText.disappearsAfterDuration)
            gameObject.SetActive(false);
    }
}
