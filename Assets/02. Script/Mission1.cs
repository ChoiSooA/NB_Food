using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class Mission1 : MonoBehaviour
{
    public TMP_Text ment;
    public TMP_Text how_many_left;

    int totalCount = 9;
    int healthTotalCount = 4;
    int curruntCount = 0;
    int healthCount = 0;

    public GameObject SuccessAll;

    public Transform goodGroup;
    public Transform badGroup;

    public ParticleSystem popParticle;

    public AudioClip[] right_wrong; //0: 맞음, 1: 틀림
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        how_many_left.text = $"{healthCount}/{healthTotalCount}"; 
        if (goodGroup != null)
        {
            foreach (Transform child in goodGroup)
            {
                child.gameObject.SetActive(true);
                child.GetComponent<Collider>().enabled = true; // 클릭 가능하게
            }
        }
        if (badGroup != null)
        {
            foreach (Transform child in badGroup)
            {
                child.gameObject.SetActive(true);
                child.GetComponent<Collider>().enabled = true; // 클릭 가능하게
                child.GetChild(0).gameObject.SetActive(false);      //이건 x표시 없애는 겁니다 그래서 true에는 없음
                child.GetChild(1).gameObject.SetActive(true);      //이건 파티클 켜주는 겁니다 그래서 true에는 없음
            }
        }
        
        // 카운트 초기화
        curruntCount = 0;
        healthCount = 0;
        how_many_left.text = $"{healthCount}/{healthTotalCount}"; // 점수 업데이트

        // 성공창 숨기기
        if (SuccessAll != null)
            SuccessAll.transform.GetChild(0).GetComponent<DoTEffect>().Close();
    }
    private void OnDisable()
    {
        audioSource.Stop();
    }

    public void IsItRight()
    {
        GameObject touchObj = TouchObjectDetector.touchObj;
        string particle = GetPostPosition_EunNeun(touchObj.name);
        ment.DOKill();
        ment.text = "";

        if (touchObj.transform.parent.name == "Good")
        {
            AnimationController.Instance.PlayOneTime("Clap");
            Audio_Manager.Instance.PlayEffect(1);
            popParticle.transform.position = touchObj.transform.position;
            popParticle.Play();
            PlaySoundExclusive(right_wrong[0]);
            ment.DOText($"{touchObj.name}{particle} 몸에 좋은 음식이 맞아요!", 1f);
            touchObj.gameObject.SetActive(false);
            Debug.Log("정답입니다.");
            healthCount++;
            how_many_left.text = $"{healthCount}/{healthTotalCount}"; // 점수 업데이트
            if (healthCount == healthTotalCount)
            {
                AnimationController.Instance.PlayOneTime("Cheer");
                Audio_Manager.Instance.PlayEffect(3);
                StartCoroutine(AllDone());
            }
        }
        else if (touchObj.transform.parent.name == "Bad")
        {
            AnimationController.Instance.PlayOneTime("SideToSide");
            Audio_Manager.Instance.PlayEffect(2);
            touchObj.transform.GetChild(0).gameObject.SetActive(true);          //X 표시 켜주는 거임
            touchObj.transform.GetChild(1).gameObject.SetActive(false);         //파티클 꺼주는 거임
            touchObj.GetComponent<Collider>().enabled = false;       //X표시가 켜지면 더이상 클릭 못하게 막는거임
            PlaySoundExclusive(right_wrong[1]);
            ment.DOText($"{touchObj.name}{particle} 몸에 좋은 음식이 아니에요!", 1f);
            Debug.Log("오답입니다.");
        }
        else
        {
            return;
        }
        curruntCount++;

        if (curruntCount == totalCount)
        {
            StartCoroutine(AllDone());
            Debug.Log("모든 오브젝트를 확인했습니다.");
        }
    }

    string GetPostPosition_EunNeun(string word)
    {
        if (string.IsNullOrEmpty(word)) return "";

        char lastChar = word[word.Length - 1];
        int unicode = lastChar - 0xAC00;

        if (unicode < 0 || unicode > 11171) return "는";

        int jongseongIndex = unicode % 28;

        return (jongseongIndex == 0) ? "는" : "은";
    }

    public void PlaySoundExclusive(AudioClip clip)
    {
        if (audioSource.clip!=null)
        {
            Audio_Manager.Instance.Ment_audioSource.Stop();      // 현재 재생 중인 사운드 정지
        }
        Audio_Manager.Instance.Ment_audioSource.clip = clip;
        Audio_Manager.Instance.Ment_audioSource.Play();      // 새로운 클립 재생
    }

    IEnumerator AllDone()   //마지막 오브젝트를 확인한 후 적당한 시간이 지나면 성공창을 띄우는 코루틴
    {
        yield return new WaitForSeconds(2f);
        SuccessAll.SetActive(true);
    }
}
