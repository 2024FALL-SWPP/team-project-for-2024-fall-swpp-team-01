# GameManager  
게임 전반적인 라인 구현

## Singleton Pattern  
GameManager, StageUIManager, MainCamera, PlayerController를 Singleton으로 구성 (전부 TitleScene으로 옮김)
StageCanvas 또한 TitleScene으로 옮기고, DoNotDestroyOnLoad에 넣어 Singleton처럼 기능하도록 함  
**GameManager에서 Scene Index를 이용해 어느 항목이 Activate되야하는지 설정해줌 (Title에서는 Player, StageUI Unactivate 하는 방식)
  
### Game Manager  
새 게임, 저장, 로딩, 다음 씬으로 이동 등 게임 전반적인 기능 수행  
Player.PlayerEventManage에서 Well, Fire, NextStage등과 닿은 상태에서 E버튼 누르면 저런 함수들 호출하도록 함  
Save, Load는 모닥불에서만 가능하고, 저장은 1칸만 자동으로 덮어씌우도록 경로 설정해둠  
Scene 바뀔 때 초기 position 설정해줘야하는데, 아직은 Stage1Scene1에 대해서만 제대로 해놓음  

### StageUIManager
PlayerHealthManager와 연동해 UI 띄워야하는데, 관리하기 힘들어서 Singleton으로 바꿈  
  
### MainCamera  
Scene 바뀔 때마다 새로운 카메라가 플레이어 찾도록 하기 번거로워서 Singleton으로 바꿈

### PlayerController  
Player 하나만 존재하도록 Singleton으로 구성  
원래 구현해두었던 기능 전부 수행 가능  
Player Prefab으로 바꾸었으니 새로운 기능 개발해서 Prefab만 바꿔주면 됌  
공격 시 스테미나 소진되도록 수정하는 것 필요하고, 칼과 방패 넣어줘야함  


## 그 외 수정사항  
맵 밖으로 바닥 Plane 연장해서 덜 어색하도록 함  
Next Stage로 이동하는거 흰색 cube로 임시로 넣어두었음  
