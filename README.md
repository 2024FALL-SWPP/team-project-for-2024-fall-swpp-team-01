# `feature/UI_play` branch
Main play 화면의 UI를 제작하고 있습니다. 다양한 화면 비율에 동적으로 반응하도록 Script를 작성하였습니다. `Stage1Scene`에서 작업하였으며, `UIManager`, `Canvas/*`, `EventSystem` Object와 `UIManager.cs` Script가 생성되었습니다.

진행 상태를 이미지로 첨부하였습니다.

## Profile picture
이미지 한 개를 $W:H=1:1.23$의 비율로 조정하여 화면 좌측 상단에 표시합니다. (다른 비율로 하려면 UI 배치를 다시 해야 합니다. 사진의 크기에 대해 정하지 않았으니 정해지면 바꾸거나 저 비율로 그냥 밀고 가도 좋습니다.)

이미지를 좀 예쁘게 표시하기 위해 시도해보았으나 (예. 모서리가 둥근 직사각형 모양으로 한다든가, 끝 부분에 그림자 효과 혹은 테두리를 준다든가, 등등...) 품이 많이 들어 애초에 이미지에 그런 처리를 다 해 놓고 불러오는 게 좋을 듯 합니다.

## HP
HP를 텍스트와 가로 막대로 나타냅니다. 가로 막대의 길이는 현재 레벨에서 가능한 최고 HP의 값에 따라 변합니다. (예. HP가 40/60이면 60/90일 때보다 막대가 2/3만큼 짧음.) Update 함수를 통해 계속 업데이트됩니다. 변수 `maxHP`와 `nowHP`가 현재는 `Update` 함수 안에 지역 변수로 선언되어 있는데, 가로 막대가 실시간으로 변하는 걸 보고 싶으시다면 public global variable로 빼서 관찰하시면 됩니다.

## SP
SP를 파이 차트로 나타냅니다. 파이 차트의 특성 상 SP의 최댓값은 정의하지 않습니다. `Update` 함수를 통해 계속 업데이트됩니다. 마찬가지로 변수 `SP`의 위치를 옮겨 파이 차트가 변하는 걸 관찰하실 수 있습니다.

플레이어의 머리 위에 위치하기로 결정하였으나 현재 상태에서는 화면 좌측 상단에 고정시켜 두었습니다. 플레이어의 위치를 실시간으로 받아 고정된 offset만큼 떨어진 곳에 위치하도록 변경하면 되므로 플레이어가 scene에 등장한 이후 금방 수정할 수 있을 겁니다.
