using UnityEngine.Playables;

namespace ParkHaro.Studies.TimelineCustomTrack
{
    /// <summary>
    /// DialogueClip이 재생될 때 실제로 UI에 대사를 출력하는 PlayableBehaviour.
    /// Clip으로부터 speaker, text, typingSpeed를 전달받아 보관하고,
    /// 재생 라이프사이클에 맞춰 TrackBinding된 DialogueUI를 제어한다.
    /// </summary>
    public class DialogueBehaviour : PlayableBehaviour
    {
        // --- Clip으로부터 전달받는 데이터 (DialogueClip.CreatePlayable에서 주입) ---
        public string speaker;
        public string text;
        public float typingSpeed;

        // --- 런타임 상태 ---
        private DialogueUI _ui;
        private int _requestId = -1;
        private bool _hasShown;

        /// <summary>
        /// Clip 진입 시 호출. 상태만 초기화하고, 실제 UI 호출은 ProcessFrame에서.
        /// OnBehaviourPlay는 UI 참조(playerData)를 받지 못하므로 여기서 ShowLine을 호출할 수 없다.
        /// </summary>
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            _hasShown = false;
            _requestId = -1;
        }

        /// <summary>
        /// Clip 구간 내 매 프레임 호출. playerData로 TrackBinding된 DialogueUI가 전달된다.
        /// 진입 첫 프레임에 한 번만 ShowLine을 호출하고 requestId를 보관한다.
        /// </summary>
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            _ui = playerData as DialogueUI;
            if (_ui == null) return;

            if (!_hasShown)
            {
                _requestId = _ui.ShowLine(speaker, text, typingSpeed);
                _hasShown = true;
            }
        }

        /// <summary>
        /// Clip 이탈 시 호출. requestId를 넘겨 Hide를 시도한다.
        /// 다음 Clip이 이미 ShowLine으로 덮어쓴 경우 UI 측에서 이 Hide는 무시된다.
        /// </summary>
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (_ui != null && _requestId >= 0)
            {
                _ui.Hide(_requestId);
            }
            _requestId = -1;
            _hasShown = false;
            _ui = null;
        }
    }
}
