using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchSceneERC20()
    {
        SceneManager.LoadScene("ERC20_Example");
    }

    public void SwitchSceneERC721()
    {
        SceneManager.LoadScene("ERC721_Example");
    }
}
