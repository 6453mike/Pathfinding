using System;
using System.Collections.Generic;

public class PriorityQueue<T> where T : IComparable<T> {
    private List<T> data;

    public PriorityQueue() {
        this.data = new List<T>();
    }

    public List<T> Data {
        get {
            return data;
        }
    }

    public void Enqueue(T item) {
        data.Add(item);

        int childIndex = data.Count - 1;
        while (childIndex > 0) {
            int parentIndex = (childIndex - 1) / 2;

            // If the child is larger than or equal to its parent priority, then we are done
            if (data[childIndex].CompareTo(data[parentIndex]) >= 0) break;

            // Bubble-up the child
            T tmp = data[childIndex];
            data[childIndex] = data[parentIndex];
            data[parentIndex] = tmp;

            childIndex = parentIndex;
        }
    }

    public T Dequeue() {
        // Assumes that the priority queue is not empty
        int lastItemIndex = data.Count - 1;

        // Get the item at the front
        T frontItem = data[0];

        // Set the front to be the last item to start the bubble-up
        data[0] = data[lastItemIndex];
        data.RemoveAt(lastItemIndex);
        lastItemIndex--;

        int parentIndex = 0;
        while (true) {
            // Left child
            int leftChildIndex = parentIndex * 2 + 1;

            // Check if there are no children
            if (leftChildIndex > lastItemIndex) break;

            int rightChildIndex = leftChildIndex + 1;

            // If a right child exists and it has a smaller priority than the left child, use the right child instead
            if (rightChildIndex <= lastItemIndex && data[rightChildIndex].CompareTo(data[leftChildIndex]) < 0)
                leftChildIndex = rightChildIndex;

            // If the parent is smaller than or equal to the smallest child, we are done
            if (data[parentIndex].CompareTo(data[leftChildIndex]) <= 0) break;

            // Otherwise, swap the parent and child
            T tmp = data[parentIndex]; data[parentIndex] = data[leftChildIndex]; data[leftChildIndex] = tmp;
            parentIndex = leftChildIndex;
        }

        return frontItem;
    }

    public T Peek() {
        T frontItem = data[0];
        return frontItem;
    }

    public int Count() {
        return data.Count;
    }
}