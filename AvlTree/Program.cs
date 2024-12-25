using System;
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
        tree2.Insert(25);
        tree2.Insert(28);
        tree2.Insert(40);


        Console.WriteLine("tree1 с узлами увеличенными на 20");
        AVLTree result3 = tree1 + 20;
        result3.PreOrderTraversal();
        Console.WriteLine();

        AVLTree result2 = tree1 + tree2;
        result2.PreOrderTraversal();
        Console.WriteLine();
       
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
        // Находим максимальное значение в tree1
        int maxValueTree1 = GetMaxValue(tree1.root);
        // Находим минимальное значение в tree2
        int minValueTree2 = GetMinValue(tree2.root);

        int heightT1 = tree1.GetHeight(tree1.root);
        int heightT2 = tree2.GetHeight(tree2.root);

        AVLTree newTree = new AVLTree();

        if (maxValueTree1 <= minValueTree2)
        {
            // Если все ключи в tree1 меньше либо равны ключам в tree2
            if (heightT1 <= heightT2)
            {
                Console.WriteLine("Используем обычный алгоритм слияния");
                return MergeUsingAlgorithm3(tree1, tree2);
            }
            else
            {
                Console.WriteLine("Симметричный");
                return MergeUsingSymmetricAlgorithm(tree1, tree2);
            }
        }
        else
        {
            Console.WriteLine("Используем обычный алгоритм Дерево + Дерево");
            return MergeTrees(tree1, tree2);
        }
    }


    private static AVLTree MergeUsingAlgorithm3(AVLTree tree1, AVLTree tree2)
    {
        // Шаг 1: Если T1 пустое, вернуть T2
        if (tree1.IsEmpty())
        {
            return tree2;
        }

        // Шаг 2: Если T2 пустое, вернуть T1
        if (tree2.IsEmpty())
        {
            return tree1;
        }

        // Шаг 3: Находим самый правый узел в T1
        var maxNode = GetMaxNode(tree1.root);
        var x = maxNode.Value; // Ключ стыковочного узла

        // Шаг 4: Отделяем x от дерева T1
        tree1.root = tree1.Delete(tree1.root, x); // Используем ваш метод Delete

        // Шаг 5: Находим левое поддерево в T2 с высотой, равной высоте T1
        int heightT1 = tree1.GetHeight(tree1.root); // Получаем высоту дерева T1
        var leftSubtree = GetLeftSubtreeWithHeight(tree2.root, heightT1, tree2);

  
        // Шаг 6: Конструируем новое дерево S
        AVLTree newTree = new AVLTree();
        newTree.root = new Node(x); // Создаем новый узел с ключом x
        newTree.root.Left = tree1.root; // Левый потомок - T1
        newTree.root.Right = leftSubtree; // Правый потомок - P

        // Шаг 7: Подключаем дерево S на прежнее место дерева P
        if (leftSubtree != null)
        {
            ConnectToParent(tree2.root, leftSubtree, newTree);
        }

        return newTree;
    }

    private static AVLTree MergeUsingSymmetricAlgorithm(AVLTree tree1, AVLTree tree2)
    {
        // Шаг 1: Если T1 пустое, вернуть T2
        if (tree1.IsEmpty())
        {
            return tree2;
        }

        // Шаг 2: Если T2 пустое, вернуть T1
        if (tree2.IsEmpty())
        {
            return tree1;
        }

        // Шаг 3: Находим минимальный узел в T2
        var minNode = GetMinNode(tree2.root);
        var y = minNode.Value; // Ключ стыковочного узла

        // Шаг 4: Отделяем y от дерева T2
        tree2.root = tree2.Delete(tree2.root, y); // Удаляем минимальный узел из T2

        // Шаг 5: Создаем новое дерево S
        AVLTree newTree = new AVLTree();
        newTree.root = new Node(y); // Создаем новый узел с ключом y

        // Шаг 6: Подключаем все элементы из T1 как левое поддерево
        newTree.root.Left = tree1.root; // Левый потомок - все элементы из T1

        // Шаг 7: Подключаем правое поддерево из T2
        newTree.root.Right = tree2.root; // Правый потомок - все элементы из T2

        return newTree;
    }

    // Метод для получения поддерева с заданной высотой
    private static Node GetLeftSubtreeWithHeight(Node node, int targetHeight, AVLTree tree)
    {
        if (node == null) return null;

        // Проверяем высоту текущего узла
        int height = tree.GetHeight(node);
        if (height == targetHeight)
        {
            return node; // Возвращаем узел, если его высота совпадает с целевой
        }

        // Рекурсивно ищем в левом поддереве
        Node leftResult = GetLeftSubtreeWithHeight(node.Left, targetHeight, tree);

        // Если не нашли в левом, ищем в правом поддереве
        if (leftResult == null)
        {
            leftResult = GetLeftSubtreeWithHeight(node.Right, targetHeight, tree);
        }

        return leftResult;
    }

    // Метод для подключения нового дерева к родителю
    private static void ConnectToParent(Node parent, Node child, AVLTree newTree)
    {
        if (parent == null) return;

        if (parent.Left == child)
        {
            parent.Left = newTree.root; // Подключаем новое дерево как левое поддерево
        }
        else if (parent.Right == child)
        {
            parent.Right = newTree.root; // Подключаем новое дерево как правое поддерево
        }

        // Здесь также можно добавить логику для обновления высот и балансировки узлов.
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

    private static AVLTree MergeTrees(AVLTree tree1, AVLTree tree2)
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
    private int GetHeight(Node node)
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