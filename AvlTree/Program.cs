﻿using System;
using System.ComponentModel.Design;
class Program
{
    static void Main()
    {
        AVLTree tree1 = new AVLTree();
        tree1.Insert(10);
        tree1.Insert(5);
        tree1.Insert(15);

        AVLTree tree2 = new AVLTree();
        tree2.Insert(30);
        tree2.Insert(20);
        tree2.Insert(60);



        AVLTree result2 = tree1 + tree2;
        result2.PreOrderTraversal();



    }
}
class AVLTree
{
    private class Node
    {
        public int Value;
        public Node Left, Right;
        public int Height;

        public Node(int value)
        {
            Value = value;
            Height = 1;
        }
    }

    private Node root;

    public bool IsEmpty()
    {
        return root == null; // Дерево пустое, если корень равен null
    }

    public void Insert(int value)
    {
        root = Insert(root, value);
    }

    public void Delete(int value)
    {
        root = Delete(root, value);
    }

    public void PreOrderTraversal()
    {
        PreOrderTraversal(root);
    }

    public static AVLTree operator +(AVLTree tree1, AVLTree tree2)
    {
        // Всегда определяем дерево с меньшим максимумом как левое
        if (GetMaxValue(tree1.root) <= GetMaxValue(tree2.root))
        {
            return MergeTrees(tree1, tree2);
        }
        else
        {
            return MergeTrees(tree2, tree1);
        }
    }

    public static AVLTree MergeTrees(AVLTree tree1, AVLTree tree2)
    {
        // Находим максимальное значение и высоту обоих деревьев
        int maxValueTree1 = GetMaxValue(tree1.root);
        int maxValueTree2 = GetMaxValue(tree2.root);

        int heightT1 = GetHeight(tree1.root);
        int heightT2 = GetHeight(tree2.root);

        // Логика выбора алгоритма слияния
        if (heightT1 <= heightT2)
        {
            // Если все ключи в tree1 меньше или равны ключам в tree2
            Console.WriteLine("Используем обычный алгоритм слияния.");
            return MergeUsingAlgorithm3(tree1, tree2);
        }
        else if (heightT1 > heightT2)
        {
            Console.WriteLine("Используем симметричный алгоритм слияния.");
            return MergeUsingSymmetricAlgorithm(tree1, tree2);
        }
        else
        {
            Console.WriteLine("Используем другой алгоритм слияния.");
            return MergeTrees123(tree1, tree2);
        }
    }


    public static AVLTree MergeUsingSymmetricAlgorithm(AVLTree tree1, AVLTree tree2)
    {
        // Шаг 1: Находим минимальный элемент во втором дереве
        var minNode = GetMinNode(tree2.root);
        var minValue = minNode.Value; // Минимальный элемент
        tree2.root = tree2.Delete(tree2.root, minValue); // Удаляем минимальный элемент из второго дерева

        // Шаг 2: Находим высоту второго дерева
        int heightT2 = GetHeight(tree2.root);

        // Шаг 3: Находим правое поддерево в первом дереве с высотой, равной высоте второго дерева
        tree1.root = GetRightSubtreeWithHeight(tree1.root, heightT2, tree2, minNode);

    
        return tree1;
    }

    private static AVLTree MergeUsingAlgorithm3(AVLTree tree1, AVLTree tree2)
    {
        // Шаг 1: Находим самый правый узел в T1
        var maxNode = GetMaxNode(tree1.root);
        var x = maxNode.Value; // Максимальный элемент
        tree1.root = tree1.Delete(tree1.root, x); // Используем метод Delete

        // Шаг 2: Находим высоту первого дерева
        int heightT1 = GetHeight(tree1.root); // Получаем высоту дерева T1

        // Шаг 3: Находим левое поддерево в T2 с высотой, равной высоте T1
        tree2.root = GetLeftSubtreeWithHeight(tree2.root, heightT1, tree1, maxNode);


        return tree2;
    }

    // Метод, который удаляет узел и всё его поддерево из дерева
    private static Node RemoveRightSubtree(Node node, Node subtreeRoot)
    {
        if (node == null) return null;

        // Если текущий узел - это корень поддерева, которое нужно удалить
        if (node == subtreeRoot)
        {
            return null; // Возвращаем null для удаления этого узла
        }

        // Рекурсивно проходим по дереву для удаления
        node.Left = RemoveRightSubtree(node.Left, subtreeRoot);
        node.Right = RemoveRightSubtree(node.Right, subtreeRoot);

        return node; // Возвращаем изменённый узел
    }






    // Метод для получения правого поддерева с заданной высотой
    private static Node GetRightSubtreeWithHeight(Node node, int targetHeight, AVLTree tree2, AVLTree.Node Xvalue)
    {
        if (node == null) return null;

        // Проверяем высоту текущего узла
        int height = GetHeight(node);
        if (height > targetHeight)
        {
            node.Right = GetRightSubtreeWithHeight(node.Right, targetHeight, tree2, Xvalue); // Возвращаем узел, если его высота совпадает с целевой
            return node;
        }
        Xvalue.Right = tree2.root;
        Xvalue.Left = node;

        return Xvalue;
    }

    // Метод для получения поддерева с заданной высотой
    private static Node GetLeftSubtreeWithHeight(Node node, int targetHeight, AVLTree tree1, AVLTree.Node Minvalue)
    {
        if (node == null) return null;

        // Проверяем высоту текущего узла
        int height = GetHeight(node);
        if (height > targetHeight)
        {
            node.Left = GetLeftSubtreeWithHeight(node.Left, targetHeight, tree1, Minvalue);
            return node; // Возвращаем узел, если его высота совпадает с целевой
        }

        Minvalue.Left = tree1.root;
        Minvalue.Right = node;

        return Minvalue;
    }



    // Метод, который удаляет узел и всё его поддерево из дерева
    private static Node RemoveLeftSubtree(Node node, Node subtreeRoot)
    {
        if (node == null) return null;

        // Если текущий узел - это корень поддерева, которое нужно удалить
        if (node == subtreeRoot)
        {
            return null; // Возвращаем null для удаления этого узла
        }

        // Рекурсивно проходим по дереву для удаления
        node.Left = RemoveLeftSubtree(node.Left, subtreeRoot);
        node.Right = RemoveLeftSubtree(node.Right, subtreeRoot);

        return node; // Возвращаем изменённый узел
    }





    // Метод для получения самого левого узла в дереве
    private static Node GetMinNode(Node node)
    {
        while (node.Left != null)
        {
            node = node.Left;
        }
        return node;
    }

    // Метод для получения самого правого узла в дереве
    private static Node GetMaxNode(Node node)
    {
        while (node.Right != null)
        {
            node = node.Right;
        }
        return node;
    }

    private static AVLTree MergeTrees123(AVLTree tree1, AVLTree tree2)
    {
        // Вставляем все узлы из tree1 и tree2 в новое дерево
        AVLTree newTree = new AVLTree();
        AddToTree(tree1.root, newTree);
        AddToTree(tree2.root, newTree);
        return newTree;
    }


    public static AVLTree operator +(AVLTree tree, int value)
    {
        AVLTree newTree = new AVLTree();
        AddToNewTree(tree.root, newTree, value);
        return newTree;
    }

    private static void AddToTree(Node node, AVLTree newTree)
    {
        if (node != null)
        {
            // Вставляем значение в новое дерево
            newTree.Insert(node.Value);
            // Рекурсивно добавляем в левое поддерево
            AddToTree(node.Left, newTree);
            // Рекурсивно добавляем в правое поддерево
            AddToTree(node.Right, newTree);
        }
    }

    private static void AddToNewTree(Node node, AVLTree newTree, int value)
    {
        if (node != null)
        {
            // Вставляем увеличенное значение в новое дерево
            newTree.Insert(node.Value + value);

            // Рекурсивно обходим левое и правое поддеревья
            AddToNewTree(node.Left, newTree, value);
            AddToNewTree(node.Right, newTree, value);
        }
    }


    private Node Insert(Node node, int value)
    {
        if (node == null) return new Node(value);

        if (value < node.Value) node.Left = Insert(node.Left, value);
        else if (value > node.Value) node.Right = Insert(node.Right, value);
        else return node;

        node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        return Balance(node);
    }
    private Node Delete(Node node, int value)
    {
        if (node == null) return node; // 1. Если узел равен null значит что мы прошли дерево до конца и возращаем null

        if (value < node.Value) node.Left = Delete(node.Left, value);
        else if (value > node.Value) node.Right = Delete(node.Right, value);
        else // этот else если мы нашли заданное для удаления число
        {
            if (node.Left == null) return node.Right; // если слева от данного узла пусто, то возвращаем правое поддерево (null)
            else if (node.Right == null) return node.Left; // если нет правого поддерева, то возвращаем левое (null)

            var minNode = GetMinValueNode(node.Right); // в переменную запишется узел с минимальным значением
            node.Value = minNode.Value; // в корень подставится мин число
            node.Right = Delete(node.Right, minNode.Value); // рекурсивно удаляем дубликат
        }

        node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right)); // 2. Обновляем высоту
        return Balance(node); // 3. Возвращаем сбалансированный узел
    }
    private Node Balance(Node node)
    {
        int balance = GetBalance(node);

        if (balance > 1)
            return GetBalance(node.Left) < 0 ? RotateLeftRight(node) : RotateRight(node);

        if (balance < -1)
            return GetBalance(node.Right) > 0 ? RotateRightLeft(node) : RotateLeft(node);

        return node;
    }
    private Node RotateLeft(Node node)
    {
        var newRoot = node.Right;
        node.Right = newRoot.Left;
        newRoot.Left = node;
        node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        newRoot.Height = 1 + Math.Max(GetHeight(newRoot.Left), GetHeight(newRoot.Right));
        return newRoot;
    }
    private Node RotateRight(Node node)
    {
        var newRoot = node.Left;
        node.Left = newRoot.Right;
        newRoot.Right = node;
        node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        newRoot.Height = 1 + Math.Max(GetHeight(newRoot.Left), GetHeight(newRoot.Right));
        return newRoot;
    }
    private Node RotateLeftRight(Node node)
    {
        node.Left = RotateLeft(node.Left);
        return RotateRight(node);
    }

    private Node RotateRightLeft(Node node)
    {
        node.Right = RotateRight(node.Right);
        return RotateLeft(node);
    }
    private static int GetHeight(Node node)
    {
        if (node == null)
        {
            return 0;
        }
        else
        {
            return node.Height;
        }
    }
    private int GetBalance(Node node) => GetHeight(node.Left) - GetHeight(node.Right);


    private Node GetMinValueNode(Node node)
    {
        while (node.Left != null) node = node.Left;
        return node;
    }
    private void PreOrderTraversal(Node node)
    {
        if (node != null)
        {
            Console.WriteLine(node.Value); // Обработка текущего узла
            PreOrderTraversal(node.Left);   // Обход левого поддерева
            PreOrderTraversal(node.Right);  // Обход правого поддерева
        }
    }

    private static int GetMinValue(Node node)
    {
        while (node.Left != null) node = node.Left;
        return node.Value;
    }

    private static int GetMaxValue(Node node)
    {
        while (node.Right != null) node = node.Right;
        return node.Value;
    }
}