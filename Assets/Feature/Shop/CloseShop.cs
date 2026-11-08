using UnityEngine;
using UnityEngine.UI;

public class CloseShop : MonoBehaviour
{
    [SerializeField] GameObject shopUI;
    [SerializeField] Button closeBtt;

    // Update is called once per frame
    void Update()
    {
        if(closeBtt.IsPressed()){
            shopUI.SetActive(false);
        }
    }

}
