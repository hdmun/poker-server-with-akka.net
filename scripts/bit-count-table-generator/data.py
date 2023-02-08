import enum


class Shape(enum.IntEnum):
    Club = 0
    Diamond = 1
    Heart = 2
    Spade = 3


class Rank(enum.IntEnum):
    Two = 1
    Three = 2
    Four = 3
    Five = 4
    Six = 5
    Seven = 6
    Eight = 7
    Nine = 8
    Ten = 9
    Jack = 10
    Queen = 11
    King = 12
    Ace = 13


StrToShapeDict = {
    'c': Shape.Club,
    'd': Shape.Diamond,
    'h': Shape.Heart,
    's': Shape.Spade
}

ShapeToStrDict = {
    Shape.Club: 'c',
    Shape.Diamond: 'd',
    Shape.Heart: 'h',
    Shape.Spade: 's'
}

StrToRankDict = {
    'T': Rank.Ten,
    'J': Rank.Jack,
    'Q': Rank.Queen,
    'K': Rank.King,
    'A': Rank.Ace
}
# other ranks
for number in range(2, 10):
    StrToRankDict[f'{number}'] = number - 1
