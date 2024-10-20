#include <iostream>
template<typename T>
class list {
private:
	struct Node {
		T data;
		Node* next;
	};
	Node* first;
public:
	list() {
		first = nullptr;
	};
	list(int n, T deflt = T()) {
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
	~list() {
		Node* current = first;
		Node* temp;
		while (current) {
			temp = current;
			current = current->next;
			delete temp;
		}
		first = nullptr;
	};
	list(const list&) {
		delete this;
		Node* current_old = list.get_first();
		Node* temp = new Node();
		temp->data = current_old->data;
		temp->next = nullptr;
		first = temp;
		Node* current_new = first;
		while (current_old) {
			current_old = current_old->next;
			temp = new Node();
			temp->data = current_old->data;
			temp->next = nullptr;
			current_new->next = temp;
			current_new = temp;

		}
	};
	list operator= (const list&) {
		delete this;
		Node* current_old = list.get_first();
		Node* temp = new Node();
		temp->data = current_old->data;
		temp->next = nullptr;
		first = temp;
		Node* current_new = first;
		while (current_old) {
			current_old = current_old->next;
			temp = new Node();
			temp->data = current_old->data;
			temp->next = nullptr;
			current_new->next = temp;
			current_new = temp;
		};
	}
	void print() {
		Node* current = first;
		while (current != nullptr) {
			std::cout << current->data << " ";
			current = current->next;
		}
	};
	T& operator[](int index) {
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
	Node* insert(T value, Node* prev) {
		Node* temp = new Node(value, prev->next);
		prev->next = temp;
		return first;
	};
	Node* insert_front(T value) {
		Node* temp = new Node(value, first);
		first = temp;
		return first;
	};
	Node* errase(Node* prev) {
		Node* temp = prev->next;
		prev->next = temp->next;
		delete temp;
		return first;
	};
	Node* errase_front() {
		Node* temp = first;
		first = first->nest;
		delete temp;
		return first;
	};
	Node* find(T value) {

	};//O(n)
	Node* get_first() {
		return first;
	};
	size_t size() {
		size_t i = 0;
		Node* current = first;
		while (current != nullptr) {
			std::cout << current->data << " ";
			current = current->next;
			i++;
		}
		return i;
	};
};