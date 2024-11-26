# `feature/scene1_extra_features` branch

담당: 박혜준

목적: Scene1 마무리 꾸미기 작업

### 업데이트 내역

1. Scene을 1과 2로 나눠두셨던 것을 하나로 다시 합쳤습니다. UI, Canvas, Player, Camera, Directional Light, Skybox, ... 등 많은 객체를 공유하기에 관리하기가 어렵고, zone이 딱 두 개 밖에 없다보니 상황에 따라 잠시 unload 했다가 load 했다가 하기에도 부적합할 것 같다고 판단했습니다. 제가 생각하지 못한 부분이 있다면 꼭 알려주세요.

2. Skybox 적용 ([에셋 출처](https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633))

3. Stage boundary 지정 - 나무 에셋 ([출처](https://assetstore.unity.com/packages/3d/vegetation/trees/realistic-tree-9-rainbow-tree-54622)) 및 절벽 에셋에 Box collider 적용하여 맵을 감쌌음

4. (추가) Cliff object도 새로 지정 ([에셋 출처](https://assetstore.unity.com/packages/3d/environments/landscapes/low-poly-cliff-pack-67289))

**질문**: Canvas의 하얀 테두리가 Game 창에서도 보이는데 저만 그런가요???
