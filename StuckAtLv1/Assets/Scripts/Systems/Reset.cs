using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    bool load;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(Input.GetKey(KeyCode.Z) && !load) {
            SceneManager.LoadScene("ArtifactIntro", LoadSceneMode.Additive);
            load = true;
        }

        if(Input.GetKey(KeyCode.X)) {
            if(SceneManager.GetSceneByName("ArtifactIntro").isLoaded) {
                SceneManager.UnloadSceneAsync("ArtifactIntro");
                load = false;
            }
        }
    }
}
