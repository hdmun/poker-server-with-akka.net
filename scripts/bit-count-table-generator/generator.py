from data import Rank


def bit_count_by_loop1(bits: int) -> int:
    count = 0
    while bits > 0:
        count += bits & 1
        bits >>= 1
    return count


def bit_count_by_loop2(bits: int) -> int:
    '''Brian Kernighan's Algorithm'''
    count = 0
    while bits > 0:
        bits &= (bits - 1)
        count += 1
    return count


def bit_count(bits: int) -> int:
    '''Divide and Conquer

    step
        0101010101010101010101010101010101010101010101010101010101010101
        0011001100110011001100110011001100110011001100110011001100110011
        0000111100001111000011110000111100001111000011110000111100001111
        0000000011111111000000001111111100000000111111110000000011111111
        0000000000000000111111111111111100000000000000001111111111111111
        0000000000000000000000000000000011111111111111111111111111111111
    '''

    bits = (bits & 0x5555555555555555) + ((bits >> 1) & 0x5555555555555555)
    bits = (bits & 0x3333333333333333) + ((bits >> 2) & 0x3333333333333333)
    bits = (bits & 0x0f0f0f0f0f0f0f0f) + ((bits >> 4) & 0x0f0f0f0f0f0f0f0f)
    bits = (bits & 0x00ff00ff00ff00ff) + ((bits >> 8) & 0x00ff00ff00ff00ff)
    bits = (bits & 0x0000ffff0000ffff) + ((bits >> 16) & 0x0000ffff0000ffff)
    bits = (bits & 0x00000000ffffffff) + ((bits >> 32) & 0x00000000ffffffff)
    return bits


def bit_flag_combination(min: int, max: int):
    for bit_set in range(1 << len(Rank)):
        if min <= bit_count(bit_set) <= max:
            yield bit_set


def generate_straight_table():
    def _straight_flag_table():
        table = dict()
        straight = 0b1_1111_0000_0000  # Broadway Straight
        priority = 10
        while straight > 0b1111:
            table[straight] = priority
            straight >>= 1
            priority -= 1

        table[0b1_0000_0000_1111] = 1  # Low Straight
        return table

    straight_flag_table = _straight_flag_table()
    straight_flags = straight_flag_table.keys()

    straight_table = dict()
    for bit_set in bit_flag_combination(5, 7):
        for straight in straight_flags:
            count = bit_count(bit_set & straight)
            if count >= 5:
                # ditionary `keys()` ordered in python 3.7+ 
                straight_table[bit_set] = straight_flag_table[straight]
                break

    return straight_table


def generate_top_count_table(top_count: int, min: int, max: int):
    def extract(bit_set: int, top: int) -> int:
        top_bit_set = 0
        for i in range(12, -1, -1):
            if top <= 0:
                break

            bit_flag = bit_set & (1 << i)
            if bit_flag > 0:
                top_bit_set |= (1 << i)
                top -= 1

        return top_bit_set

    top_bit_set_table = dict()
    for bit_set in bit_flag_combination(min, max):
        top_bit_set_table[bit_set] = extract(bit_set, top=top_count)

    return top_bit_set_table
