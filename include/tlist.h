#include <iostream>
template<typename T>
class Tlist {
private:
	struct Node {
		T data;
		Node* next;
	};
	class iterator {
	public:
		Node* node;
		iterator() {
			node = nullptr;
		}
		iterator(Node* n) {
			node = n;
		}
		Node& operator*() {
			return *node;
		}
		Node operator*() const {
			return *node;
		}
		iterator& operator++() {
			node = node->next;
			return *this;
		}
		bool operator==(iterator iter) {
			return node == iter.node;
		}
		bool operator!=(iterator iter) {
			return node != iter.node;
		}
	};
	Node* first;
public:
	Tlist() {
		first = nullptr;
	};
	Tlist(int n, T deflt = T()) {
		if (n <= 0) {
			throw "eogts";
		}
		Node* temp = new Node();
		temp->data = deflt;
		temp->next = nullptr;
		first = temp;
		Node* current = first;
		for (int i = 1; i < n; i++) {
			temp = new Node();
			temp->data = deflt;
			temp->next = nullptr;
			current->next = temp;
			current = temp;
		}
	}
	~Tlist() {
		Node* current = first;
		Node* temp;
		while (current!=nullptr) {
			temp = current;
			current = current->next;
			delete temp;
		}
		first = nullptr;
	};
	Tlist(const Tlist& list) {
		Node* current_old = list.first;
		Node* temp = new Node();
		temp->data = current_old->data;
		temp->next = nullptr;
		first = temp;
		Node* current_new = first;
		current_old = current_old->next;
		while (current_old!=nullptr) {
			temp = new Node();
			temp->data = current_old->data;
			temp->next = nullptr;
			current_new->next = temp;
			current_new = temp;
			current_old = current_old->next;
		}
	};
	bool operator==(const Tlist& list) const {
		bool res = true;
		Node* current2 = list.first;
		Node* current1 = first;
		while (current1 != nullptr || current2 != nullptr) {
			if ((current1 == nullptr && current2 != nullptr) || (current2 == nullptr && current1 != nullptr)) {
				res = false;
				return res;
			}
			if (current1->data != current2->data) {
				res = false;
				return res;
			}
			current1 = current1->next;
			current2 = current2->next;
		}
		return res;
	}
	Tlist& operator= (const Tlist& list) {
		if (first == list.first) {
			return *this;
		}
		Tlist* res = new Tlist(list);
		Tlist* temp = new Tlist();
		temp->first = first;
		delete temp;
		first = res->first;
		res->first = nullptr;
		delete res;
		return *this;
	}
	Tlist& operator= (Node* node) {
		if (first == node) {
			return *this;
		}
		Tlist* temp = new Tlist();
		temp->first = first;
		delete temp;
		first = node;
		return *this;
	}
	void print() {
		Node* current = first;
		while (current != nullptr) {
			std::cout << current->data << " ";
			current = current->next;
		}
		std::cout << "\n";
	};
	T& operator[](int index) {
		if (index<0 || index>=this->size()) {
			throw "list index out of range";
		}
		Node* current_ptr = first;
		int cur_ind = 0;
		T res = current_ptr->data;
		while (current_ptr != nullptr && cur_ind < index) {
			current_ptr = current_ptr->next;
			cur_ind++;
			res = current_ptr->data;
		}
		if (current_ptr == nullptr) {
			throw "no such element";
		}
		return res;

	};//O(n)
	void set_element(T value, int index) {
		iterator t;
		try {
			t = find_ptr(index);
		}
		catch (...) {
			throw  "list index out of range";
		}
		t.node->data = value;
	}
	iterator insert(T value, iterator prev) {
		Node* temp = new Node();
		temp->data = value;
		temp->next = prev.node->next;
		prev.node->next = temp;
		return iterator(temp);
	};
	iterator insert_front(T value) {
		Node* temp = new Node();
		temp->data = value;
		temp->next = first;
		first = temp;
		return iterator(first);
	};
	iterator errase(iterator prev) {
		Node* temp = new Node();
		temp = prev.node->next;
		if (temp) {
			prev.node->next = temp->next;
			delete temp;
		}
		return first;
	};
	iterator errase_front() {
		Node* temp = first;
		first = first->next;
		delete temp;
		return iterator(first);
	};
	iterator find(T value) {
		Node* current = first;
		Node* res = nullptr;
		while (current != nullptr) {
			if (current->data == value) {
				res = current;
			}
			current = current->next;
		}
		return iterator(res);
	};//O(n)
	iterator find_ptr(int index) {
		if (index<0 || index>=this->size()) {
			throw "list index out of range";
		}
		Node* current = first;
		for(int i=0; i<index;i++) {
			current = current->next;
		}
		return iterator(current);
	}
	size_t size() {
		size_t i = 0;
		Node* current = first;
		while (current != nullptr) {
			//std::cout << current->data << " ";
			current = current->next;
			i++;
		}
		return i;
	};

	Node* merge_sorted_lists(Tlist& list) {
		if (list.first == nullptr) {
			Tlist* res = new Tlist(*this);
			return res->first;
		}
		if (first == nullptr) {
			Tlist* res = new Tlist(list);
			return res->first;
		}
		Node* curr1 = first;
		Node* curr2 = list.first;

		Tlist* res = new Tlist();
		Node* curr;
		if (curr1->data < curr2->data)
		{
			Node* temp = new Node();
			temp->data = curr1->data;
			res->first = temp;
			curr = temp;
			curr1 = curr1->next;
		}
		else
		{
			Node* temp = new Node();
			temp->data = curr2->data;
			res->first = temp;
			curr = temp;
			curr2 = curr2->next;
		}
		while (curr1 != nullptr && curr2 != nullptr)
		{
			if (curr1->data < curr2->data)
			{
				Node* temp = new Node();
				temp->data = curr1->data;
				curr->next = temp;
				curr = temp;
				curr1 = curr1->next;
			}
			else
			{
				Node* temp = new Node();
				temp->data = curr2->data;
				curr->next = temp;
				curr = temp;
				curr2 = curr2->next;
			}
		}
		while (curr1 != nullptr)
		{
			Node* temp = new Node();
			temp->data = curr1->data;
			curr->next = temp;
			curr = temp;
			curr1 = curr1->next;
		}
		while (curr2 != nullptr)
		{
			Node* temp = new Node();
			temp->data = curr2->data;
			curr->next = temp;
			curr = temp;
			curr2 = curr2->next;
		}
		return res->first;
	}
	iterator begin() {
		return iterator(first);
	}
	iterator end() {
		return iterator(nullptr);
	}
};