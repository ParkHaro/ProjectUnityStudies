using System.Collections;
using TMPro;
using UnityEngine;

namespace ParkHaro.Studies.TimelineCustomTrack
{
    /// <summary>
    /// 특정 Transform(화자)의 머리 위에 붙는 말풍선 스타일 DialogueUI 구현체.
    ///
    /// followTarget이 설정되어 있으면 매 프레임 스크린 좌표를 갱신해 따라다닌다.
    /// 화자 이름은 별도 표시하지 않고 followTarget 자체가 화자를 암시한다.
    /// (speaker 인자는 디버그 로깅 용도로만 사용)
    /// </summary>
    public class BubbleDialogueUI : DialogueUI
    {
        [Header("Bubble")]
        [Tooltip("Hide 시 비활성화할 말풍선 루트")]
        [SerializeField] private GameObject bubbleRoot;

        [Tooltip("본문 표시용 TextMeshPro")]
        [SerializeField] private TMP_Text bodyText;

        [Header("Follow")]
        [Tooltip("말풍선이 따라다닐 월드 상의 Transform (없으면 고정 위치)")]
        [SerializeField] private Transform followTarget;

        [Tooltip("화면 기준 오프셋(픽셀). followTarget의 스크린 좌표에 더해진다.")]
        [SerializeField] private Vector2 screenOffset = new Vector2(0f, 60f);

        [Tooltip("월드 좌표 → 스크린 좌표 변환에 사용할 카메라. 비어있으면 Camera.main 사용")]
        [SerializeField] private Camera worldCamera;

        private RectTransform _rectTransform;
        private int _currentRequestId;
        private Coroutine _typingCoroutine;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            if (bubbleRoot != null) bubbleRoot.SetActive(false);
        }

        private void LateUpdate()
        {
            if (followTarget == null || _rectTransform == null) return;
            if (bubbleRoot != null && !bubbleRoot.activeSelf) return;

            var cam = worldCamera != null ? worldCamera : Camera.main;
            if (cam == null) return;

            Vector3 screenPos = cam.WorldToScreenPoint(followTarget.position);
            _rectTransform.position = (Vector2)screenPos + screenOffset;
        }

        public override int ShowLine(string speaker, string text, float typingSpeed)
        {
            _currentRequestId++;
            int requestId = _currentRequestId;

            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
                _typingCoroutine = null;
            }

            if (bubbleRoot != null) bubbleRoot.SetActive(true);
            if (bodyText != null) bodyText.text = string.Empty;

            _typingCoroutine = StartCoroutine(TypeRoutine(text, typingSpeed));

            return requestId;
        }

        public override void Hide(int requestId)
        {
            if (requestId != _currentRequestId) return;

            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
                _typingCoroutine = null;
            }

            if (bubbleRoot != null) bubbleRoot.SetActive(false);
        }

        private IEnumerator TypeRoutine(string fullText, float typingSpeed)
        {
            if (bodyText == null) yield break;

            if (typingSpeed <= 0f)
            {
                bodyText.text = fullText;
                yield break;
            }

            var wait = new WaitForSeconds(typingSpeed);
            for (int i = 1; i <= fullText.Length; i++)
            {
                bodyText.text = fullText.Substring(0, i);
                yield return wait;
            }
        }
    }
}
