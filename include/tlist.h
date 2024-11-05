#include <iostream>
template<typename T>
class Tlist {
private:
	struct Node {
		T data;
		Node* next;
	};
	Node* first;
public:
	Tlist() {
		first = nullptr;
	};
	Tlist(int n, T deflt = T()) {
		if (n < 0) {
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
		Node* t;
		try {
			t = find_ptr(index);
		}
		catch (...) {
			throw  "list index out of range";
		}
		t->data = value;
	}
	void insert(T value, int index) {
		Node* prev;
		try {
			prev = find_ptr(index-1);
		} 
		catch (...) {
			throw  "list index out of range";
		}
		Node* temp = new Node();
		temp->data = value;
		temp->next = prev->next;
		prev->next = temp;
	};
	void insert_front(T value) {
		Node* temp = new Node();
		temp->data = value;
		temp->next = first;
		first = temp;
	};
	Node* errase(int index) {
		Node* prev;
		try {
			prev = find_ptr(index-1);
		}
		catch (...) {
			throw  "list index out of range";
		}
		Node* temp = new Node();
		temp = prev->next;
		prev->next = temp->next;
		delete temp;
		return first;
	};
	Node* errase_front() {
		Node* temp = first;
		first = first->next;
		delete temp;
		return first;
	};
	Node* find(T value) {
		Node* current = first;
		Node* res = nullptr;
		while (current != nullptr) {
			if (current->data == value) {
				res = current;
			}
			current = current->next;
		}
		return res;
	};//O(n)
	Node* find_ptr(int index) {
		if (index<0 || index>=this->size()) {
			throw "list index out of range";
		}
		Node* current = first;
		for(int i=0; i<index;i++) {
			current = current->next;
		}
		return current;
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

	Tlist& merge_sorted_lists(Tlist& list) {
		if (first == nullptr) {
			return list;
		}
		if (list->first == nullptr) {
			return *this;
		}
		Node* current1 = first;
		Node* current2 = list->first;
		if (current1->data < current2->data) {
			Node* temp = current1->next;
			current1->next = current2;
			current2 = current2->next;
			current1->next->next = temp;
		}
	}
};