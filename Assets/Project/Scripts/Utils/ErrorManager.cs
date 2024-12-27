using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;


public class ErrorManager : MonoBehaviour 
{
    public GameObject noActiveSkillError;
    public static ErrorManager instance;
    private Sequence sequence;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found an Error Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowNoActiveSkillError(float time = 4f, DG.Tweening.Ease ease = DG.Tweening.Ease.Linear)
    {

        if (sequence != null) // only create if there was none before.
        {
            sequence.Kill();
        }

        sequence = DOTween.Sequence();

        noActiveSkillError.SetActive(true);
        noActiveSkillError.GetComponent<CanvasGroup>().alpha = 1;
        sequence.Append(noActiveSkillError.GetComponent<CanvasGroup>().DOFade(0, time)
            .SetEase(ease)
            .OnComplete(() => noActiveSkillError.SetActive(false)));

        sequence.Play();   
    }
}