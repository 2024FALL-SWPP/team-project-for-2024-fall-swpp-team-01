# `feature/UI_Boss` branch

할 일 목록!

1. 보스 체력 바 - 보스 이름, now 빨간색, 더 얇게, 수치, 포지션 (캡처 이미지 참고), 체력 깎이는 애니메이션

2. 포션 슬롯 1개, 이미지, 변하는 숫자

---

### 구현 진행상황

1. 보스 체력 바
    - 보스 이름, 체력 바 구현함
    - 체력 깎이는 애니메이션 - Player의 HP bar에는 적용함. Boss의 HP bar에는 아직 적용하지 않음. Boss HP를 관리하는 Manager의 구현이 있은 후에 연결하는 게 나을 듯.
    - 버튼을 통해 구현 내용 일단 확인할 수 있도록 해뒀고, 나중에 Boss나 Game Manager의 함수에 연결할 때도 버튼이 어떤 함수에 연결되어 있는지 확인해서 하시면 됩니다.

2. 포션 슬롯
    - [이미지 출처](https://www.pngegg.com/en/png-yoagx/download#goog_rewarded)
    - 얘도 마찬가지로 버튼을 통해 구현 내용 확인할 수 있도록 해둠. 나중에 포션 관리 시스템 연결할 때 참고하시면 됩니다.

3. 이외 개선된 사항
    - Interaction Text의 위치를 화면 크기 반응형으로 바꿨습니다.
    - SP pie 색을 파란색으로 바꿨습니다. 하얀색은 뭔가 좀 그래서...


### Debugging button 사용 방법

`Stage_canvas` 아래에 있는 `Temp_DebuggingButtons`를 활성화하면 각 버튼의 연결된 함수를 확인할 수 있다. 각 함수의 구현을 참고하여 필요한 대로 활용한다.
