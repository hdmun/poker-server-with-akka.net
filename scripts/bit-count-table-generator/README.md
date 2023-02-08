# bit-count-table-generator

`Domain.Poker.Simulator.HandEvaluator` 클래스에서 사용할 비트 마스킹 테이블 생성 스크립트입니다.


## generate_straight_table()

`Straight`를 판별하는 비트 플래그 맵을 생성합니다.

key, value 값은 다음과 같습니다.

- `Bit Flag` : `Striaght Priority`


## generate_top_count_table(top_count: int, min: int, max: int):

주어진 비트 플래그 내에서 높은 순서대로 `top_count`만큼 비트 플래그를 추출한 맵을 생성합니다.


## Bit Flag for Card Number

카드 값은 각 비트 자리수로 나타내며 비트 자릿 수에 대응하는 카드 값은 다음과 같습니다.

```py
0b1_0000_0000_0000  # A
0b0_1000_0000_0000  # K
0b0_0100_0000_0000  # Q
0b0_0010_0000_0000  # J
0b0_0001_0000_0000  # T, 10
0b0_0000_1000_0000  # 9
0b0_0000_0100_0000  # 8
0b0_0000_0010_0000  # 7
0b0_0000_0001_0000  # 6
0b0_0000_0000_1000  # 5
0b0_0000_0000_0100  # 4
0b0_0000_0000_0010  # 3
0b0_0000_0000_0001  # 2
```
