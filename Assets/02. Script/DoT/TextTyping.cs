using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;

public class TextTyping : MonoBehaviour
{
    [SerializeField] private UnityEvent event_finish;   //타이핑이 끝나고 실행하고 싶은 이벤트
    TMP_Text outMent;
    public string[] addment;
    bool coroutine_running = false;
    string originalText;

    float typingSpeed = 0.1f;

    private void Awake()
    {
        outMent = this.GetComponent<TMP_Text>();
        originalText = outMent.text;
    }
    private void OnEnable()
    {
        outMent.text = "";
        transform.parent.DOScale(transform.localScale * 0.95f, 0.2f).SetLoops(4, LoopType.Yoyo); //시작할 때 커지는 효과 주려고 넣음
        if (addment.Length > 0)
        {
            StartCoroutine(NextText());
        }
        else if (addment.Length == 0 && originalText != null) //addment가 없고 originalText가 있을 때
        {
            StartCoroutine(FinishOriginalMent());
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        outMent.DOKill();
        outMent.text = "";
    }

    IEnumerator NextText()
    {
        yield return new WaitForSeconds(1f);  //너무 바로 시작하는 것 같아서 조금 시간을 줌
        coroutine_running = true;
        for (int i = 0; i < addment.Length; i++)
        {
            outMent.text = "";
            float nextSpeed = addment[i].Length * typingSpeed;
            outMent.DOText(addment[i], nextSpeed).SetEase(Ease.Linear);
            yield return new WaitForSeconds(nextSpeed + 0.6f);
        }
        if (event_finish != null)
        {
            event_finish.Invoke();
        }
        coroutine_running = false;
        yield return new WaitForSeconds(0.3f);
    }
    IEnumerator FinishOriginalMent()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("오리지널멘트실행됨");
        float nextSpeed = originalText.Length * typingSpeed;
        outMent.DOText(originalText, nextSpeed).SetEase(Ease.Linear);
        yield return new WaitForSeconds(nextSpeed + 0.6f);
        if (event_finish != null)
        {
            event_finish.Invoke();
        }
    }
}
