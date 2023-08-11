using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu Elements")]
    [SerializeField] private GameObject _gameName;
    [SerializeField] private GameObject _mainMenu, _optionsMenu, _loadingScreen;
    [SerializeField] private Slider _loadingProgressBar;
    [SerializeField] private TMP_Text _loadingProgressText;

    [Header("Volume Elements")]
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private TMP_Text _volumeValue;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private float _soundLevel;

    [Header("UI Dropdowns")]
    [SerializeField] private TMP_Dropdown _resolutionDropdown, _graphicsDropdown;

    private void Awake()
    {
        _mainMenu.SetActive(true);
        _optionsMenu.SetActive(false);
        _loadingScreen.SetActive(false);
        // enable vsync by default to limit fps in menus
        QualitySettings.vSyncCount = 1;
    }

    private void Start()
    {
        // set the volume
        _soundLevel = PlayerPrefs.GetFloat("Volume");
        _mixer.SetFloat("MasterVolume", Mathf.Log10(_soundLevel) * 20);
        _volumeSlider.value = _soundLevel;
        _volumeValue.text = _soundLevel.ToString("p");

        // set the screen mode and resolution
        _graphicsDropdown.value = PlayerPrefs.GetInt("Screen Mode");
        _resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        _gameName.SetActive(false);
        _mainMenu.SetActive(false);
        _loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            _loadingProgressBar.value = progress;
            _loadingProgressText.SetText(progress.ToString("p"));
            yield return null;
        }
    }

    public void Options()
    {
        _mainMenu.SetActive(false);
        _optionsMenu.SetActive(true);
    }

    public void OptionsBack()
    {
        _mainMenu.SetActive(true);
        _optionsMenu.SetActive(false);
    }

    public void SetVolume(float sliderValue)
    {
        _soundLevel = sliderValue;

        if (_soundLevel < 0.01)
        {
            _volumeValue.SetText("0.00 %");
        }
        else
        {
            _volumeValue.SetText(_soundLevel.ToString("p"));
        }
    }

    public void ApplyOptions()
    {
        // apply sound
        // it will change the mixer between -80 decibels to 0 decibels
        _mixer.SetFloat("MasterVolume", Mathf.Log10(_soundLevel) * 20);
        PlayerPrefs.SetFloat("Volume", _soundLevel);

        // apply resolution and screen mode
        switch (_graphicsDropdown.value)
        {
            case 0:
                switch (_resolutionDropdown.value)
                {
                    case 0:
                        Screen.SetResolution(1920, 1080, true);
                        break;
                    case 1:
                        Screen.SetResolution(1280, 720, true);
                        break;
                }

                break;
            case 1:
                switch (_resolutionDropdown.value)
                {
                    case 0:
                        Screen.SetResolution(1920, 1080, false);
                        break;
                    case 1:
                        Screen.SetResolution(1280, 720, false);
                        break;
                }

                break;
        }

        PlayerPrefs.SetInt("Screen Mode", _graphicsDropdown.value);
        PlayerPrefs.SetInt("Resolution", _resolutionDropdown.value);
    }

    public void Exit()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }
}
