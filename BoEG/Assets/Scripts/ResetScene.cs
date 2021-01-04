using UnityEngine;
using UnityEngine.SceneManagement;

namespace MobaGame
{
    public class ResetScene : MonoBehaviour
    {
        public void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}