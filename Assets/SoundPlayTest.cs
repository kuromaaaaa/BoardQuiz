using UnityEngine;

public class SoundPlayTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BgmManager.Instance.Play(SoundType.BGMTEST);
    }
}
