#include "tlist.h"

#include <gtest.h>

TEST(Tlist, can_create_list_with_positive_length)
{
	ASSERT_NO_THROW(Tlist<int> m(5));
}

TEST(Tlist, throws_when_create_list_with_negative_length)
{
	ASSERT_ANY_THROW(Tlist<int> m(-5));
}

TEST(Tlist, can_create_copied_list)
{
	Tlist<int> m(5);
	ASSERT_NO_THROW(Tlist<int> m1(m));
}

TEST(Tlist, copied_list_is_equal_to_source_one)
{
	Tlist<int> m(5);
	Tlist<int> m1(m);
	EXPECT_EQ(m, m1);
}

TEST(Tlist, copied_list_has_its_own_memory)
{
	Tlist<int> m(5);
	Tlist<int> m1(m);
	m1[1] = 1;
	Tlist<int> res(5);
	res[1] = 1;
	EXPECT_EQ(m1, res);
}

TEST(Tlist, can_get_size)
{
	int sz = 5;
	Tlist<int> l(sz);
	EXPECT_EQ(sz, l.size());
}

TEST(Tlist, can_get_element)
{
	Tlist<int> m(5);
	ASSERT_NO_THROW(m[3]);
}

TEST(Tlist, throws_when_get_element_with_negative_index)
{
	Tlist<int> m(5);
	ASSERT_ANY_THROW( m[-1]);
}

TEST(Tlist, throws_when_get_element_with_too_large_index)
{
	Tlist<int> m(5);
	ASSERT_ANY_THROW(m[6]);
}

TEST(Tlist, can_assign_matrix_to_itself)
{
	Tlist<int> m(5);
	ASSERT_NO_THROW(m = m);
	EXPECT_EQ(m,m);
}

TEST(Tlist, can_assign_matrices_of_equal_size)
{
	Tlist<int> m1(5), m2(5);
	ASSERT_NO_THROW(m1 = m2);
	EXPECT_EQ(m1,m2);
}

TEST(Tlist, assign_operator_change_matrix_size)
{
	Tlist<int> m1(5), m2(3);
	m1 = m2;
	EXPECT_EQ(m1.size(), 3);
}

TEST(Tlist, can_assign_matrices_of_different_size)
{
	Tlist<int> m1(5), m2(3);
	ASSERT_NO_THROW(m1 = m2);
	EXPECT_EQ(m1, m2);
}

TEST(Tlist, compare_equal_matrices_return_true)
{
	Tlist<int> m1(5), m2(5);
	m1[0] = 100;
	m2[0] = 100;
	EXPECT_EQ(true, m1==m2);
}

TEST(Tlist, compare_matrix_with_itself_return_true)
{
	Tlist<int> m1(5);
	EXPECT_EQ(true, m1 == m1);
}

TEST(Tlist, matrices_with_different_size_are_not_equal)
{
	Tlist<int> m1(5), m2(3);
	EXPECT_EQ(false, m1 == m2);
}

TEST(Tlist, can_insert_element)
{
	Tlist<int> m1(3);
	Tlist<int> res(4);
	res.set_element(100, 3);
	m1.insert(100, 3);
	EXPECT_EQ(true, m1==res);
}

TEST(Tlist, can_insert_front)
{
	Tlist<int> m1(3);
	Tlist<int> res(4);
	res.set_element(100, 0);
	m1.insert_front(100);
	EXPECT_EQ(res, m1);
}

TEST(Tlist, can_erraze_element)
{
	Tlist<int> m1(3);
	Tlist<int> res(2);
	m1.errase(2);
	EXPECT_EQ(res, m1);
}

TEST(Tlist, can_erraze_front)
{
	Tlist<int> m1(3);
	Tlist<int> res(2);
	m1.errase_front();
	EXPECT_EQ(res, m1);
}

TEST(Tlist, can_merge_sorted_lists_and_output_list_also_sorted)
{
	int sz1 = 5, sz2 = 2;
	Tlist<int> m1(sz1), m2(sz2), res(sz1 + sz2), m3(sz1+sz2);
	m1[0] = 0;
	m1[1] = 2;
	m1[2] = 3;
	m1[3] = 5;
	m1[4] = 10000;
	m2[0] = 1;
	m2[1] = 100000;
	res[0] = 0;
	res[1] = 1;
	res[2] = 2;
	res[3] = 3;
	res[4] = 5;
	res[5] = 10000;
	res[6] = 100000;
	m3 = m1.merge_sorted_lists(m2);
	EXPECT_EQ(m3, res);
}
