using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SuperPower : MonoBehaviour
{
    [SerializeField] private Image  superPowerCoolDownImage = null;
    [SerializeField] private Text   levelText               = null;

    [Space]
    public GameObject[] superPower;

    private float coolDownTime  = 20;
    private bool  isCoolingDown = false;

    public void Initialize()
    {
        StopCoroutine(SuperPowerCoolDown());
        isCoolingDown = false;
        superPowerCoolDownImage.fillAmount = 0;
    }

    public void UpdateLevel(int level)
    {
        levelText.text = level.ToString();
    }

    public void CastSuperPower()
    {
        if (!isCoolingDown)
        {
            isCoolingDown = true;

            StartCoroutine(SuperPowerCoolDown());
            float t = 0;
            for (int i = 0; i < superPower.Length; i++)
            {
                t += 0.2f;
                StartCoroutine(Delay(t, i));
            }
        }
    }

    public void EndSuperPower(GameObject _superPower)
    {
        _superPower.SetActive(false);
    }

    IEnumerator SuperPowerCoolDown()
    {
        CoolDownAnimation();
        yield return new WaitForSeconds(coolDownTime);
        isCoolingDown = false;
    }

    void CoolDownAnimation()
    {
        superPowerCoolDownImage.fillAmount = 1;
        LeanTween.value(gameObject, delegate(float val) { superPowerCoolDownImage.fillAmount = val; }, 1f, 0f, coolDownTime).setOnComplete(delegate() { superPowerCoolDownImage.fillAmount = 0; });
    }

    IEnumerator Delay(float time, int index)
    {
        yield return new WaitForSeconds(time);
        superPower[index].SetActive(true);
    }
}