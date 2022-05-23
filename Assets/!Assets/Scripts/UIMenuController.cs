using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuController : MonoBehaviour
{
    [SerializeField] private float _screenAnimationSpeed = 0.25f;
    [Space(5)]
    [SerializeField] private CanvasGroup _mainMenu;
    [SerializeField] private CanvasGroup _settingsMenu;
    [Space(3)]
    [SerializeField] private CanvasGroup _controlsSettings;
    [SerializeField] private CanvasGroup _audioSettings;
    [SerializeField] private CanvasGroup _videoSettings;



    private void Start()
    {
        HideImediateCG(_settingsMenu);
        HideImediateCG(_controlsSettings);
        HideImediateCG(_audioSettings);
        HideImediateCG(_videoSettings);

    }

    public void HideImediateCG(CanvasGroup cg)
    {
        cg.blocksRaycasts = false;
        cg.alpha = 0.0f;
    }

    public void StartTheGame()
    {
        SceneManager.LoadScene(1);
    }
    public void OpenSettings()
    {
        _mainMenu.blocksRaycasts = false;
        _settingsMenu.DOFade(1.0f, _screenAnimationSpeed).OnComplete(() =>
                 _settingsMenu.blocksRaycasts = true
        );

    }
    public void CloseSettings()
    {
        _settingsMenu.blocksRaycasts = false;
        _settingsMenu.DOFade(0.0f, _screenAnimationSpeed).OnComplete(() =>
                 _mainMenu.blocksRaycasts = false
        );

    }
    public void ExitFromGame()
    {
        Application.Quit();
    }


}
