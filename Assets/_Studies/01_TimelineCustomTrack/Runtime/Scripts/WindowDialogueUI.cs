using System.Collections;
using TMPro;
using UnityEngine;

namespace ParkHaro.Studies.TimelineCustomTrack
{
    /// <summary>
    /// 하단 고정 대화창 스타일의 DialogueUI 구현체.
    /// 화자 이름과 본문을 각각 표시하며, 본문은 typingSpeed에 따라 한 글자씩 타이핑된다.
    ///
    /// requestId 기반 경합 방어:
    /// - ShowLine마다 requestId 증가 및 반환
    /// - Hide(id)는 현재 requestId와 일치할 때만 실제 숨김 실행
    /// </summary>
    public class WindowDialogueUI : DialogueUI
    {
        [Header("Panel")]
        [Tooltip("Hide 시 비활성화할 대화창 루트")]
        [SerializeField] private GameObject panel;

        [Header("Text")]
        [Tooltip("화자 이름 표시용 TextMeshPro")]
        [SerializeField] private TMP_Text speakerText;

        [Tooltip("본문 표시용 TextMeshPro. 타이핑 효과가 적용됨")]
        [SerializeField] private TMP_Text bodyText;

        private int _currentRequestId;
        private Coroutine _typingCoroutine;

        private void Awake()
        {
            if (panel != null) panel.SetActive(false);
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

            if (panel != null) panel.SetActive(true);
            if (speakerText != null) speakerText.text = speaker;
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

            if (panel != null) panel.SetActive(false);
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
