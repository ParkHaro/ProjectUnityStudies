namespace ParkHaro.Studies.TimelineCustomTrack
{
    /// <summary>
    /// Timeline DialogueTrack이 바인딩하는 UI의 계약.
    /// 구현체를 교체해 말풍선·대화창·시스템 메시지 등 다양한 출력 스타일을 전환할 수 있다.
    /// </summary>
    public interface IDialogueUI
    {
        /// <summary>
        /// 대사 한 줄을 화면에 출력하고, 해당 요청의 고유 ID를 반환한다.
        /// 이 ID는 이후 Hide 호출 시 유효성 검사에 사용된다.
        /// </summary>
        /// <param name="speaker">화자 이름</param>
        /// <param name="text">대사 내용</param>
        /// <param name="typingSpeed">글자당 지연 시간(초). 0 이하면 즉시 표시.</param>
        /// <returns>이 출력 요청의 고유 ID</returns>
        int ShowLine(string speaker, string text, float typingSpeed);

        /// <summary>
        /// 지정된 requestId가 현재 활성 요청과 일치할 때만 대사를 감춘다.
        /// 다음 ShowLine이 이미 덮어쓴 경우 이 호출은 무시된다.
        /// </summary>
        /// <param name="requestId">ShowLine이 반환했던 ID</param>
        void Hide(int requestId);
    }
}
