using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpScript : MonoBehaviour
{
    public Image progressBar;
    public Color[] Colors;
    public Text powerText;

    public void SetData(PowerUpController.PowerUp.Type type)
    {
        progressBar.color = Colors[(int)type];
        powerText.text = type.ToString();
    }

    public void SetProgress(float progress)
    {
        progressBar.fillAmount = progress;
    }

    public IEnumerator DestroyBar()
    {
        yield return new WaitForSeconds(.25f);
        Destroy(gameObject);
    }
}
