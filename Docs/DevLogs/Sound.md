# `feature/sound` branch

### 음악 목록!
- **이하 모두 다운받아서 `Assets/Audios`에 넣어두었음!**
- [보스전](https://assetstore.unity.com/packages/audio/music/free-6-dark-fantasy-boss-battle-tracks-275561)
- [TitleScene](https://www.youtube.com/watch?v=8Erdny-jZW4)
- [Stage2](https://www.youtube.com/watch?v=tRiZG9WGRGo)
- [이건 뭐지?](https://youtu.be/nb89_EBBQT0?si=rDmzDhzm-Qx8UXxV&t=123)
- 기타 효과음: [https://www.epidemicsound.com](https://www.epidemicsound.com/)

## 구현 진행 상황
### BGM
| Scene | BGM | Remarks |
| -- | -- | -- |
| TitleScene | [TitleScene](https://www.youtube.com/watch?v=8Erdny-jZW4) | |
| Stage 1 Scene 1 | [Stage2](https://www.youtube.com/watch?v=tRiZG9WGRGo) | **Stage 1 음악이 안 보이는데 어떤 걸까요?** |
| Stage 1 Scene 2 | [Stage2](https://www.youtube.com/watch?v=tRiZG9WGRGo) | Stage 1 Scene 1의 BGM 끊지 않고 재생 |
| Stage 2 | [Stage2](https://www.youtube.com/watch?v=tRiZG9WGRGo) | |
| Boss Stage | [보스전](https://assetstore.unity.com/packages/audio/music/free-6-dark-fantasy-boss-battle-tracks-275561) | |

수정 필요 - Stage 1 음악이 Slack에 안 보이는데 확인 부탁드립니다! 음악 파일 다운로드 해서 `SoundManager` object의 `Stage 1 Music`에 적용하기만 하면 됩니다.

### Sound Effects
| Index | Situation | Sound Title | Remarks |
| -- | -- | -- | -- |
| 0 | Walking | Walking Footsteps | |
| 1 | Game over | Funny Falling Notes | |
| 2 | Player attack | Knife Swing | **임팩트가 좀 약한데 Body Hit으로 바꿀까요?** |
| 3 | Attacked | Body Punch | |

수정 필요 - 다른 소리 효과 넣어야 하는 게 많은 것 같은데 현재 `main`에 병합된 내용이 많지 않은 것 같아서... 접근 가능한 script가 제한되는 것 같습니다. 어차피 `SoundManager` 기본 구조 다 잡아뒀으니 소리 추가하는 건 어렵지 않을 겁니다.