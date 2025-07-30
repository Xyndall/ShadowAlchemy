using UnityEngine;
using System.Collections;

public class ChallengeLevel : MonoBehaviour
{
    public static ChallengeLevel Instance { get; private set; }

    [Tooltip("Assign the challenge level GameObject here.")]
    public GameObject challengeLevelObject;

    [Tooltip("Assign the basic level GameObject here.")]
    public GameObject basicLevelObject;

    [Tooltip("Assign the loading animation GameObject here.")]
    public GameObject loadingAnimationObject;

    private void Awake()
    {
        // Singleton pattern
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
        if (PlayerPrefs.GetInt("ChallengeLevelEnabled", 0) == 1)
        {
            challengeLevelObject.SetActive(true);
            basicLevelObject.SetActive(false);
        }
        else
        {
            challengeLevelObject.SetActive(false);
            basicLevelObject.SetActive(true);
        }
    }

    public void EnableChallengeLevel()
    {
        Debug.Log("challenge enable function called");
        PlayerPrefs.SetInt("ChallengeLevelEnabled", 1); // 1 = true
        PlayerPrefs.Save();
        StartCoroutine(SwapLevel(true));
    }

    public void DisableChallengeLevel()
    {
        Debug.Log("challenge disable function called");
        PlayerPrefs.SetInt("ChallengeLevelEnabled", 0); // 0 = false
        PlayerPrefs.Save();
        StartCoroutine(SwapLevel(false));
    }

    private IEnumerator SwapLevel(bool enableChallenge)
    {
        if (loadingAnimationObject != null)
            loadingAnimationObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        if (challengeLevelObject != null)
            challengeLevelObject.SetActive(enableChallenge);
        if (basicLevelObject != null)
            basicLevelObject.SetActive(!enableChallenge);

        yield return new WaitForSeconds(4.5f);

        if (loadingAnimationObject != null)
            loadingAnimationObject.SetActive(false);
    }
}
