using UnityEngine;
using UnityEngine.UI;

public class OpenShop : MonoBehaviour
{
    [SerializeField] Button shopBtt;
    [SerializeField] GameObject shop;

    // Update is called once per frame
    void Update()
    {
        if (shopBtt.IsPressed())
        {
            shop.SetActive(true);
        }
    }
}
