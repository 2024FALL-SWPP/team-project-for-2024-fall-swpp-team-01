# `feature/map-scene1-zone2` branch

이름은 zone2이지만 zone1도 그냥 여기로 merge 해버렸습니다. (zone1 기존 구현 branch: `feature/Scene1_asset`, 담당: 김성헌) 여기에서 scene1의 맵을 총괄합니다.

목적: Scene1의 맵을 구성합니다. ~~대로를 기준으로 우측 부분(zone2)을 구현합니다.~~ 

관리: 박혜준

### 구현 설명
나름 hierarchical structure를 잘 구현해보려 노력했습니다. 예를 들어,
```
Zone2Objects
├── House_Blocks
│   ├── I_Block
│   ├── I_Block (1)
│   ├── L_Block
│   ├── L_Block (1)
│   ...
├── Monsters
│   ├── monster_1_animation
│   ├── monster_1_animation (1)
│   ├── monster_1_animation (2)
│   ...
├── Wells
└── Plane
```
