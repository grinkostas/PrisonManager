using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public void ResetSaves()
    {
        ES3.DeleteFile();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
