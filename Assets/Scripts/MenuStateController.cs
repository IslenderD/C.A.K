using UnityEngine;
using Unity.Cinemachine;         
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class MenuStateController : MonoBehaviour
{
    public enum MenuState { Title, Play, Options, Credits }

    [Header("Cameras")]
    [SerializeField] CinemachineCamera camTitle;     
    [SerializeField] CinemachineCamera camPlay;
    [SerializeField] CinemachineCamera camOptions;
    [SerializeField] CinemachineCamera camCredits;

    [Header("Panels")]
    [SerializeField] CanvasGroup panelTitle;
    [SerializeField] CanvasGroup panelPlay;
    [SerializeField] CanvasGroup panelOptions;
    [SerializeField] CanvasGroup panelCredits;

    [Header("Tuning")]
    [SerializeField] float fadeSpeed = 5f;

    MenuState state = MenuState.Title;

    void Start()
    {
        ApplyCameras();
        SetPanel(panelTitle, true, true);
        SetPanel(panelPlay, false, true);
        SetPanel(panelOptions, false, true);
        SetPanel(panelCredits, false, true);
    }

    public void ShowTitle()   { state = MenuState.Title;   ApplyCameras(); }
    public void ShowPlay()    { state = MenuState.Play;    ApplyCameras(); }
    public void ShowOptions() { state = MenuState.Options; ApplyCameras(); }
    public void ShowCredits() { state = MenuState.Credits; ApplyCameras(); }

    public void SetVolume(float v) => AudioListener.volume = v;

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void Update()
    {
        if (state != MenuState.Title && BackPressed()) ShowTitle();

        SetPanel(panelTitle,   state == MenuState.Title,   false);
        SetPanel(panelPlay,    state == MenuState.Play,    false);
        SetPanel(panelOptions, state == MenuState.Options, false);
        SetPanel(panelCredits, state == MenuState.Credits, false);
    }

    void ApplyCameras()
    {
        SetPriority(camTitle,   state == MenuState.Title);
        SetPriority(camPlay,    state == MenuState.Play);
        SetPriority(camOptions, state == MenuState.Options);
        SetPriority(camCredits, state == MenuState.Credits);
    }

    void SetPriority(CinemachineCamera cam, bool active)
    {
        if (cam != null) cam.Priority = active ? 20 : 10;
    }

    void SetPanel(CanvasGroup g, bool visible, bool instant)
    {
        if (g == null) return;
        float target = visible ? 1f : 0f;
        g.alpha = instant ? target
                          : Mathf.MoveTowards(g.alpha, target, fadeSpeed * Time.unscaledDeltaTime);
        g.interactable = visible;
        g.blocksRaycasts = visible;
    }

    bool BackPressed()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.Escape);
#endif
    }
}