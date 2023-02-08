import json
from generator import generate_straight_table, generate_top_count_table


if __name__ == '__main__':
    straight_table = generate_straight_table()
    print('len(straight_table): ', len(straight_table))
    with open('straightTable.json', 'w') as fp:
        json.dump(straight_table, fp)

    top_five_table = generate_top_count_table(5, min=6, max=7)
    print('len(top_five_table): ', len(top_five_table))
    with open('topFiveCardTable.json', 'w') as fp:
        json.dump(top_five_table, fp)

    top_table = generate_top_count_table(1, min=2, max=7)
    print('len(top_table): ', len(top_table))
    with open('topCardTable.json', 'w') as fp:
        json.dump(top_table, fp)
