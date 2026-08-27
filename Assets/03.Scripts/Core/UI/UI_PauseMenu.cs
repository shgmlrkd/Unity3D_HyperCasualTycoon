using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class UI_PauseMenu : MonoBehaviour
{
    private const float SAVE_COMPLETE_WIDTH = 1000.0f;
    private const float SAVE_FAILED_WIDTH = 1450.0f;
    private const float SAVE_WIDTH = 1200.0f;
    private const float GO_TO_TITLE_WIDTH = 1600.0f;

    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject contentPanel;

    [Header("Confirm Modal Popup")]
    [SerializeField] private UI_ConfirmModal confirmModal;

    [Header("Scene Settings")]
    [SerializeField] private string titleSceneName = "TitleScene";

    [SerializeField] private Button pauseBtn;
    [SerializeField] private Button closeBtn;

    //private bool openState = false;

    private void Start()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.Subscribe(EventType.OnGameStateChanged, OnGameStateChanged);
        }
        //GameManager.Instance.PauseGame();
        GameManager.Instance.ResumeGame();
        pauseBtn.onClick.AddListener(() => OnOpenClosePause());//upgrade버튼
        closeBtn.onClick.AddListener(() => OnOpenClosePause());//upgrade버튼



    }

    private void OnDestroy()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.Unsubscribe(EventType.OnGameStateChanged, OnGameStateChanged);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnOpenClosePause();
        }

    }
    
    private void OnOpenClosePause()
    {
        if (optionPanel != null && optionPanel.activeSelf)
        {
            OnClickCloseOption();
            //return;
        }

        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.CurrentState == GameState.Play)
            {
                
                GameManager.Instance.PauseGame();

            }
            else if (GameManager.Instance.CurrentState == GameState.Pause)
            {
                GameManager.Instance.ResumeGame();
            }
        }
    }

    private void OnGameStateChanged(object param)
    {
        if (GameManager.Instance == null || pausePanel == null) return;

        bool isPaused = (GameManager.Instance.CurrentState == GameState.Pause);

        pausePanel.SetActive(isPaused);

        
        //if (isPaused)
        //{
        //    //pausePanel.SetActive(isPaused);
        //    //if (openState) return;

        //    pausePanel.transform.DOScale(1.0f, 1.0f);

        //    // DOScale 의 첫 번째 파라미터는 목표 Scale 값, 두 번째는 시간입니다.
        //    //seq.Append(pausePanel.transform.DOScale(1.1f, 0.2f));
        //    //seq.Append(pausePanel.transform.DOScale(1f, 0.1f));
        //    //seq.Play().OnComplete(() =>
        //    //{
        //    //    //pausePanel.SetActive(isPaused);
                
        //    //    //openState = true;
        //    //    //OpenState = true;
        //    //});
            
        //}
        //else
        //{
        //    var seq = DOTween.Sequence();
        //    //if(!pausePanel.activeSelf)return;
        //    //if (!openState) return;

        //    //pausePanel.transform.localScale = Vector3.one * 0.2f;

        //    seq.Append(pausePanel.transform.DOScale(1.1f, 0.1f));
        //    seq.Append(pausePanel.transform.DOScale(0.2f, 0.2f));

        //    // OnComplete 는 seq 에 설정한 애니메이션의 플레이가 완료되면
        //    // { } 안에 있는 코드가 수행된다는 의미입니다.
        //    // 여기서는 닫기 애니메이션이 완료된 후 객첼르 비활성화 합니다.
        //    seq.Play().OnComplete(() =>
        //    {
        //        pausePanel.SetActive(isPaused);
        //        pausePanel.transform.DOScale(1.0f, 1.0f);
        //        //openState = false;
        //    });
        //    //pausePanel.SetActive(isPaused);
        //}

    }

    private void SetConfirmModalWidth(float width)
    {
        if (confirmModal == null)
            return;

        RectTransform rectTransform = confirmModal.GetComponent<RectTransform>();

        if (rectTransform == null)
            return;

        Vector2 size = rectTransform.sizeDelta;
        size.x = width;
        rectTransform.sizeDelta = size;
    }

    public void OnClickResume()
    {
        SoundManager.Instance.PlaySFX();
        GameManager.Instance?.ResumeGame();
    }

    public void OnClickSave()
    {
        if (SaveManager.Instance == null) return;

        SoundManager.Instance.PlaySFX();

        if (SaveManager.Instance.HasSaveFile())
        {
            SetConfirmModalWidth(SAVE_WIDTH);

            confirmModal?.ShowConfirm(
                "이미 저장된 게임이 있습니다.\n덮어쓰시겠습니까?",
                onYes: ExecuteSave
            );
        }
        else
        {
            ExecuteSave();
        }
    }

    private void ExecuteSave()
    {
        bool success = SaveManager.Instance.SaveGameData();

        if (success)
        {
            SetConfirmModalWidth(SAVE_COMPLETE_WIDTH);
            confirmModal?.ShowAlert("게임 저장이 완료되었습니다.");
        }
        else
        {
            SetConfirmModalWidth(SAVE_FAILED_WIDTH);
            confirmModal?.ShowAlert("게임 저장에 실패했습니다.\n잠시 후 다시 시도해주세요.");
        }
    }

    public void OnClickToTitle()
    {
        SoundManager.Instance.PlaySFX();

        SetConfirmModalWidth(GO_TO_TITLE_WIDTH);

        if (SaveManager.Instance != null && SaveManager.Instance.IsDirty)
        {
            confirmModal?.ShowConfirm(
                "저장하지 않은 내용이 있습니다.\n저장하지 않고 타이틀 화면으로 이동하시겠습니까?",
                onYes: GoToTitleScene
            );
        }
        else
        {
            GoToTitleScene();
        }
    }

    private void GoToTitleScene()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResumeGame();
        }

        Time.timeScale = 1f;
        SceneManagerEx.Instance?.LoadScene(titleSceneName);
    }

    public void OnClickOpenOption()
    {
        if (optionPanel != null)
        {
            SoundManager.Instance.PlaySFX();
            optionPanel.SetActive(true);
            contentPanel.SetActive(false);
        }
    }

    public void OnClickCloseOption()
    {
        if (optionPanel != null)
        {
            SoundManager.Instance.PlaySFX();
            optionPanel.SetActive(false);
            contentPanel.SetActive(true);
        }
    }
}