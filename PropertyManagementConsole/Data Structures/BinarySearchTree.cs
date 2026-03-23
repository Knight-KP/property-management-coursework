using System;
using System.Collections.Generic;
using System.Text;

namespace PropertyManagementConsole.DataStructures;

public class BinarySearchTree<T>
{
    private BstNode<T>? _root;

    public void Insert(int key, T value)
    {
        _root = InsertRecursive(_root, key, value);
    }

    private BstNode<T> InsertRecursive(BstNode<T>? node, int key, T value)
    {
        if (node == null)
            return new BstNode<T>(key, value);

        if (key < node.Key)
            node.Left = InsertRecursive(node.Left, key, value);
        else if (key > node.Key)
            node.Right = InsertRecursive(node.Right, key, value);
        else
            node.Value = value; // overwrite if key already exists

        return node;
    }

    public T? Search(int key)
    {
        var current = _root;

        while (current != null)
        {
            if (key == current.Key)
                return current.Value;

            current = (key < current.Key) ? current.Left : current.Right;
        }

        return default;
    }
}
