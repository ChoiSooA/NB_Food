using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEditor;

public class AnimationController : MonoBehaviour
{
    public GameObject Character;
    public Animator animator;

    public Vector3 resetPosition;
    public Vector3 resetRotation;
    public Vector3 resetScale;

    Coroutine currentCoroutine;

    public static AnimationController Instance { get; private set; }

    private void Awake()
    {
        animator = Character.GetComponent<Animator>();
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        resetPosition = Character.transform.localPosition;
        resetRotation = Character.transform.localRotation.eulerAngles;
        resetScale = Character.transform.localScale;
    }

    public void ResetVector()
    {
        Character.transform.localPosition = resetPosition;
        Character.transform.localRotation = Quaternion.Euler(resetRotation);
        Character.transform.localScale = resetScale;
    }

    private void OnDisable()
    {
        PlayAnimation("Idle");
        ResetVector();
    }

    public void PlayAnimation(string animationName)
    {
        Debug.Log($"[PlayAnimation] {animationName}");

        foreach (var param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(param.name);
            }
        }

        if (animationName != "Idle")
            animator.Play("Idle", 0, 0f); // 강제 리셋

        StartCoroutine(TriggerNextFrame(animationName));
    }

    IEnumerator TriggerNextFrame(string triggerName)
    {
        yield return null;
        animator.SetTrigger(triggerName);
    }

    public void PlayOneTime(string animationName)
    {
        if (currentCoroutine != null)           //기존 실행 중인 코루틴 정지
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(PlayOneTimeCo(animationName));    //새 애니메이션 트리거 실행
    }

    IEnumerator PlayOneTimeCo(string animName)
    {
        PlayAnimation(animName);

        // 상태 진입 대기
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(animName));

        float length = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);

        Debug.Log(animName + " 애니메이션 종료");
        PlayAnimation("Idle");

        currentCoroutine = null;
    }

    public void playAnimSet(int animnum)
    {
        playAnimSetCode(animnum, null);
    }
    public void playAnimSetCode(int animnum, System.Action onComplete = null)
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        switch (animnum)
        {
            case 0: StartCoroutine(AnimOne(onComplete)); break;
            case 1: StartCoroutine(AnimSide(onComplete)); break;
            case 2: StartCoroutine(AnimTwo(onComplete)); break;
            case 3: StartCoroutine(AnimThree(onComplete)); break;
            case 4: StartCoroutine(AnimFour(onComplete)); break;
        }
    }

    void jump()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) return;
        PlayAnimation("Jump");
        Character.transform.DOMoveY(-1.5f, 0.7f).SetEase(Ease.Linear);
    }

    public void moveSide()
    {
        Character.transform.DOLocalMove(new Vector3(-1.45f, -1.7f, 6.24f), 0.7f).SetEase(Ease.Linear);
        Character.transform.DOLocalRotate(new Vector3(0f, 167.5f, 0f), 0.7f).SetEase(Ease.Linear);
    }

    IEnumerator AnimOne(System.Action onComplete = null)   //0
    {
        ResetVector();
        yield return new WaitForSeconds(0.5f);
        jump();
        yield return new WaitForSeconds(1f);
        PlayAnimation("Hi");
        yield return new WaitForSeconds(1.84f);
        PlayAnimation("Idle");
        yield return new WaitForSeconds(15f);
        PlayAnimation("Nice");
        yield return new WaitForSeconds(3f);
        PlayAnimation("Idle"); 
        
        onComplete?.Invoke(); 
    }

    IEnumerator AnimSide(System.Action onComplete = null)       //1
    {
        Character.transform.localPosition = new Vector3(-1.45f, -4f, 6.24f);
        yield return new WaitForSeconds(0.01f);
        Character.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        yield return new WaitForSeconds(0.5f);
        jump();
        moveSide();

        onComplete?.Invoke(); 
    }

    IEnumerator AnimTwo(System.Action onComplete = null)        //2
    {
        ResetVector();
        yield return new WaitForSeconds(0.5f);
        jump();
        yield return new WaitForSeconds(1f);
        PlayAnimation("Clap");
        yield return new WaitForSeconds(2.27f);
        PlayAnimation("Idle");
        yield return new WaitForSeconds(15f);
        PlayAnimation("Cheer");

        onComplete?.Invoke();
    }

    IEnumerator AnimThree(System.Action onComplete = null)      //3
    {

        Character.transform.localPosition = new Vector3(0.9f, -4f, 7f);
        yield return new WaitForSeconds(0.5f);
        jump();
        yield return new WaitForSeconds(0.9f);

        onComplete?.Invoke(); 
    }
    IEnumerator AnimFour(System.Action onComplete = null)        //4
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("Dance"))
        {
            PlayOneTime("Idle");
            yield return new WaitForSeconds(6f);
            PlayOneTime("Clap");
        }
        yield return new WaitForSeconds(5f);
        PlayOneTime("Idle");
        yield return new WaitForSeconds(0.01f);
        PlayOneTime("Hi");
        onComplete?.Invoke();
    }
}
