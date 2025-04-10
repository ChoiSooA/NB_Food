using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class AnimationController : MonoBehaviour
{
    public GameObject Character;

    public Animator animator;

    public Vector3 resetPosition;
    public Vector3 resetRotation;
    public Vector3 resetScale;

    public static AnimationController Instance { get; private set; }


    private void Awake()
    {
        animator = Character.GetComponent<Animator>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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

    void PlayAnimation(string animationName)
    {
        animator.SetTrigger(animationName);
    }
    public void PlayOneTime(string animationName)
    {
        StartCoroutine(PlayOneTimeCo(animationName));
    }
    public void playAnimSet(int animnum)
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        switch (animnum)
        {
            case 0:
                StartCoroutine(AnimOne());
                break;
            case 1:
                StartCoroutine(AnimSide());
                break;
            case 2:
                StartCoroutine(AnimTwo());
                break;
            case 3:
                StartCoroutine(AnimThree());
                break;
        }
    }

    void jump()
    {
        PlayAnimation("Jump");
        Character.transform.DOMoveY(-1.5f, 0.7f).SetEase(Ease.Linear);  //DOMoveY는 y축으로 이동하는 함수, Y축으로 -1.5만큼이 아니라 Y를 -1.5로 만들어줌
    }

    public void moveSide()
    {
        Character.transform.DOLocalMove(new Vector3(-1.45f,-1.7f,6.24f), 0.7f).SetEase(Ease.Linear);  //DOMoveX는 x축으로 이동하는 함수
        Character.transform.DOLocalRotate(new Vector3(0f,167.5f,0f), 0.7f).SetEase(Ease.Linear);
    }
    
    IEnumerator AnimOne()                                   //0번 애니메이션
    {
        ResetVector();
        yield return new WaitForSeconds(0.5f);   //애니메이션 시작 전 대기 시간
        jump();
        yield return new WaitForSeconds(1f);
        PlayAnimation("Hi");
        yield return new WaitForSeconds(1.5f);
        PlayAnimation("Idle");
        yield return new WaitForSeconds(7f);   //마지막 nice 전까지 대기 시간
        PlayAnimation("Nice");
        yield return new WaitForSeconds(3f);
        PlayAnimation("Idle");
    }
    IEnumerator AnimSide()                                  //1번 애니메이션
    {
        Character.transform.localPosition = new Vector3(-1.45f, -4f, 6.24f);
        yield return new WaitForSeconds(0.01f);   //애니메이션 시작 전 대기 시간
        Character.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        yield return new WaitForSeconds(0.5f);   //애니메이션 시작 전 대기 시간
        jump();
        moveSide();
    }                                                   
    IEnumerator AnimTwo()                                   //2번 애니메이션
    {
        ResetVector();
        yield return new WaitForSeconds(0.5f);   //애니메이션 시작 전 대기 시간
        jump();
        yield return new WaitForSeconds(1f);
        PlayAnimation("Clap");
        yield return new WaitForSeconds(2f);
        PlayAnimation("Idle");
        yield return new WaitForSeconds(4f);    //마지막 cheer 전까지 대기 시간
        PlayAnimation("Cheer");
    }
    IEnumerator AnimThree()                                 //3번 애니메이션
    {
        ResetVector();
        yield return new WaitForSeconds(0.5f);   //애니메이션 시작 전 대기 시간
        jump();
        yield return new WaitForSeconds(1f);
        PlayAnimation("Clap");
        Audio_Manager.Instance.PlayEffect(3);
        yield return new WaitForSeconds(2.2f);
        PlayAnimation("Idle");
        yield return new WaitForSeconds(9f);    //마지막 clap 전까지 대기 시간
        PlayAnimation("Hi");
    }

    IEnumerator PlayOneTimeCo(string animName)
    {
        StopCoroutine(PlayOneTimeCo(animName)); //애니메이션이 실행 중일 때 다시 실행하면 애니메이션이 중복으로 실행되는 것을 방지
        PlayAnimation("idle");
        PlayAnimation(animName);
        yield return new WaitForSeconds(0.01f);     //애니메이션이 실행 후 애니메이션 시간을 읽어올 수 있어 실행까지 아주 작은 대기
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log(animName + " 애니메이션 종료");
        PlayAnimation("Idle");
    }
}
