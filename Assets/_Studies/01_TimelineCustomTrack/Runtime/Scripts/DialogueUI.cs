using UnityEngine;

namespace ParkHaro.Studies.TimelineCustomTrack
{
    /// <summary>
    /// DialogueTrack이 TrackBindingType으로 바인딩하는 추상 기반 클래스.
    /// MonoBehaviour를 상속해 씬의 GameObject에 붙일 수 있게 하고,
    /// IDialogueUI 계약을 강제한다.
    /// 구체 구현(WindowDialogueUI, BubbleDialogueUI 등)은 이 클래스를 상속한다.
    /// </summary>
    public abstract class DialogueUI : MonoBehaviour, IDialogueUI
    {
        public abstract int ShowLine(string speaker, string text, float typingSpeed);
        public abstract void Hide(int requestId);
    }
}
