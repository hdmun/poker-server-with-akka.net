import unittest

from generator import generate_straight_table, generate_top_count_table

class GenerateTastCase(unittest.TestCase):
    def test_straight_table(self):
        # given, when
        straight_table = generate_straight_table()

        # then
        self.assertEqual(298, len(straight_table))

    def test_top_five_table(self):
        # given, when
        top_five_table = generate_top_count_table(5, min=6, max=7)

        # then
        self.assertEqual(3432, len(top_five_table))

    def test_top_table(self):
        # given, when
        top_table = generate_top_count_table(1, min=2, max=7)

        # then
        self.assertEqual(5798, len(top_table))
