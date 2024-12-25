using System;

public class TreeNode
{
    public int Key;
    public TreeNode Left;
    public TreeNode Right;
    public int Height;

    public TreeNode(int key)
    {
        Key = key;
        Height = 1;
    }
}

public class AVLTree
{
    public TreeNode Root;

    // Вычисление высоты узла
    private static int Height(TreeNode node) => node?.Height ?? 0;

    // Вычисление баланса узла
    private static int GetBalance(TreeNode node) => node == null ? 0 : Height(node.Left) - Height(node.Right);

    // Правый поворот
    private static TreeNode RightRotate(TreeNode y)
    {
        var x = y.Left;
        var T2 = x.Right;

        x.Right = y;
        y.Left = T2;

        y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;
        x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;

        return x;
    }

    // Левый поворот
    private static TreeNode LeftRotate(TreeNode x)
    {
        var y = x.Right;
        var T2 = y.Left;

        y.Left = x;
        x.Right = T2;

        x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;
        y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;

        return y;
    }

    // Вставка узла
    public static TreeNode Insert(TreeNode node, int key)
    {
        if (node == null) return new TreeNode(key);

        if (key < node.Key)
            node.Left = Insert(node.Left, key);
        else if (key > node.Key)
            node.Right = Insert(node.Right, key);
        else
            return node; // Дубликаты не допускаются

        node.Height = Math.Max(Height(node.Left), Height(node.Right)) + 1;

        int balance = GetBalance(node);

        // Левый левый случай
        if (balance > 1 && key < node.Left.Key)
            return RightRotate(node);

        // Правый правый случай
        if (balance < -1 && key > node.Right.Key)
            return LeftRotate(node);

        // Левый правый случай
        if (balance > 1 && key > node.Left.Key)
        {
            node.Left = LeftRotate(node.Left);
            return RightRotate(node);
        }

        // Правый левый случай
        if (balance < -1 && key < node.Right.Key)
        {
            node.Right = RightRotate(node.Right);
            return LeftRotate(node);
        }

        return node;
    }

    // Увеличение всех ключей дерева на заданное число
    public AVLTree AddInteger(int value)
    {
        var resultTree = new AVLTree();
        resultTree.Root = CloneTree(Root); // Создаем копию дерева
        IncreaseKeys(resultTree.Root, value); // Увеличиваем ключи
        return resultTree;
    }

    private void IncreaseKeys(TreeNode node, int increment)
    {
        if (node == null) return;
        node.Key += increment; // Увеличиваем ключ текущего узла
        IncreaseKeys(node.Left, increment); // Рекурсивно увеличиваем ключи в левом поддереве
        IncreaseKeys(node.Right, increment); // Рекурсивно увеличиваем ключи в правом поддереве
    }

    private TreeNode CloneTree(TreeNode node)
    {
        if (node == null) return null;
        return new TreeNode(node.Key)
        {
            Left = CloneTree(node.Left),
            Right = CloneTree(node.Right),
            Height = node.Height
        };
    }
    public static TreeNode MergeTrees(TreeNode node1, TreeNode node2)
    {
        // Если одно из деревьев пустое, возвращаем другое дерево
        if (node1 == null) return node2;
        if (node2 == null) return node1;

        TreeNode xNode;

        // Если высота node1 меньше или равна высоте node2
        if (Height(node1) <= Height(node2))
        {
            // Найти максимальный элемент в node1
            xNode = FindMax(node1);
            node1 = Remove(node1, xNode.Key); // Удаляем максимальный элемент из node1

            TreeNode directly = FindP(node2, Height(node1), xNode, node1);

            // Проверяем, существует ли уже узел с таким ключом в node2
            if (Contains(node2, xNode.Key))
            {
                // Создаем новый узел, который станет корнем нового дерева

                node2 = Remove(node2, xNode.Key);
                // Балансируем и возвращаем новое дерево

            }


            return directly;
        }
        else
        {
            // Найти минимальный элемент в node2
            xNode = FindMin(node2);
            node2 = Remove(node2, xNode.Key); // Удаляем минимальный элемент из node2

            TreeNode symmetrically = FindP2(node1, Height(node2), xNode, node1);

            // Проверяем, существует ли уже узел с таким ключом в node1
            if (Contains(node1, xNode.Key))
            {
                // Создаем новый узел, который станет корнем нового дерева

                node1 = Remove(node1, xNode.Key);
                // Балансируем и возвращаем новое дерево

            }

            return symmetrically;
        }


    }
    // Метод проверки на соответствие Л-Дерева и Р-Дерева
    private static bool CanMerge(TreeNode leftTree, TreeNode rightTree)
    {
        if (leftTree == null || rightTree == null) return true;

        var maxLeft = FindMax(leftTree);
        var minRight = FindMin(rightTree);

        return maxLeft.Key <= minRight.Key;
    }

    // Перегрузка оператора для объединения деревьев
    public static AVLTree operator +(AVLTree tree1, AVLTree tree2)
    {
        if (tree1 == null) return tree2;
        if (tree2 == null) return tree1;

        if (CanMerge(tree1.Root, tree2.Root))
        {
            var mergedTree = new AVLTree();
            mergedTree.Root = MergeTrees(tree1.Root, tree2.Root);
            return mergedTree;
        }
        else if (CanMerge(tree2.Root, tree1.Root))
        {
            var mergedTree = new AVLTree();
            mergedTree.Root = MergeTrees(tree2.Root, tree1.Root);
            return mergedTree;
        }
        else
        {
            // Если условие Л-Дерева и Р-Дерева нарушено
            return StandardMerge(tree1, tree2);
        }
    }

    // Обычное слияние деревьев (алгоритм 2)
    private static AVLTree StandardMerge(AVLTree tree1, AVLTree tree2)
    {
        var mergedTree = new AVLTree();
        void TraverseAndInsert(TreeNode node)
        {
            if (node == null) return;
            mergedTree.Root = Insert(mergedTree.Root, node.Key);
            TraverseAndInsert(node.Left);
            TraverseAndInsert(node.Right);
        }

        TraverseAndInsert(tree1.Root);
        TraverseAndInsert(tree2.Root);
        return mergedTree;
    }

    // Метод для поиска поддерева в T2 с высотой, равной h(T1)
    private static TreeNode FindSubtreeWithHeight(TreeNode root, int targetHeight)
    {
        if (root == null) return null;

        // Если высота текущего узла равна целевой, возвращаем этот узел
        if (Height(root) == targetHeight)
        {
            return root;
        }

        // Ищем в левом поддереве
        var left = FindSubtreeWithHeight(root.Left, targetHeight);
        if (left != null) return left;

        // Ищем в правом поддереве
        return FindSubtreeWithHeight(root.Right, targetHeight);
    }

    // Метод для поиска места вставки узла
    public static TreeNode FindP(TreeNode current, int targetHeight, TreeNode xNode, TreeNode node1)
    {


        int currentHeight = Height(current);

        if (currentHeight > targetHeight) // текущий узел находится на уровне выше, чем требуется. Поэтому рекурсивно ищем в левом поддереве (current.Left), так как левое поддерево может содержать узлы с меньшей высотой.
        {
            // Рекурсивно ищем в левом поддереве
            current.Left = FindP(current.Left, targetHeight, xNode, node1);
            return BalanceTree(current);
        }
        // мы нашли узел, на уровне которого нужно вставить node1

        // Проверяем, существует ли уже узел с таким значением
        xNode.Left = node1;
        xNode.Right = current;
        return BalanceTree(xNode);



        return current; // Если не вставили, возвращаем текущий узел.
    }
    public static TreeNode FindP2(TreeNode current, int targetHeight, TreeNode xNode, TreeNode node2)
    {


        int currentHeight = Height(current);

        if (currentHeight > targetHeight) // текущий узел находится на уровне выше, чем требуется. Поэтому рекурсивно ищем в левом поддереве (current.Left), так как левое поддерево может содержать узлы с меньшей высотой.
        {
            // Рекурсивно ищем в  поддереве
            current.Right = FindP2(current.Right, targetHeight, xNode, node2);
            return BalanceTree(current);
        }
        // мы нашли узел, на уровне которого нужно вставить node1

        // Проверяем, существует ли уже узел с таким значением
        xNode.Right = node2;
        xNode.Left = current;
        return BalanceTree(xNode);



        return current; // Если не вставили, возвращаем текущий узел.
    }


    // Метод для проверки, существует ли ключ в дереве
    private static bool Contains(TreeNode node, int key)
    {
        while (node != null)
        {
            if (key == node.Key)
                return true;
            if (key < node.Key)
                node = node.Left;
            else
                node = node.Right;
        }
        return false;
    }

    private static TreeNode BalanceTree(TreeNode node)
    {


        node.Height = Math.Max(Height(node.Left), Height(node.Right)) + 1;
        int balance = GetBalance(node);

        if (balance > 1 && GetBalance(node.Left) >= 0)
            return RightRotate(node);

        if (balance > 1 && GetBalance(node.Left) < 0)
        {
            node.Left = LeftRotate(node.Left);
            return RightRotate(node);
        }

        if (balance < -1 && GetBalance(node.Right) <= 0)
            return LeftRotate(node);

        if (balance < -1 && GetBalance(node.Right) > 0)
        {
            node.Right = RightRotate(node.Right);
            return LeftRotate(node);
        }

        return node;
    }

    private static TreeNode FindMax(TreeNode node)
    {
        while (node.Right != null)
            node = node.Right;
        return node;
    }

    private static TreeNode FindMin(TreeNode node)
    {
        while (node.Left != null)
            node = node.Left;
        return node;
    }

    private static TreeNode Remove(TreeNode node, int key)
    {
        if (node == null) return null;

        if (key < node.Key)
            node.Left = Remove(node.Left, key);
        else if (key > node.Key)
            node.Right = Remove(node.Right, key);
        else
        {
            if (node.Left == null || node.Right == null)
                return node.Left ?? node.Right;

            var temp = FindMin(node.Right);
            node.Key = temp.Key;
            node.Right = Remove(node.Right, temp.Key);
        }

        node.Height = Math.Max(Height(node.Left), Height(node.Right)) + 1;
        return BalanceTree(node);
    }

    public void PrintInOrder(TreeNode node)
    {
        if (node == null) return;

        Console.Write(node.Key + " ");
        PrintInOrder(node.Left);
        PrintInOrder(node.Right);
    }
}

class Program
{
    static void Main(string[] args)
    {
        var tree1 = new AVLTree();
        tree1.Root = AVLTree.Insert(tree1.Root, 10);
        tree1.Root = AVLTree.Insert(tree1.Root, 5);
        tree1.Root = AVLTree.Insert(tree1.Root, 15);
        tree1.Root = AVLTree.Insert(tree1.Root, 17);



        var tree2 = new AVLTree();
        tree2.Root = AVLTree.Insert(tree2.Root, 30);
        tree2.Root = AVLTree.Insert(tree2.Root, 20);
        tree2.Root = AVLTree.Insert(tree2.Root, 60);


        Console.WriteLine("Первое дерево:");
        tree1.PrintInOrder(tree1.Root);

        Console.WriteLine("\nВторое дерево:");
        tree2.PrintInOrder(tree2.Root);

        var mergedTree = tree2 + tree1;
        Console.WriteLine("\nСлияние двух деревьев:");
        mergedTree.PrintInOrder(mergedTree.Root);

        var incrementedTree = tree2.AddInteger(5);
        Console.WriteLine("\nДерево после увеличения на 5:");
        incrementedTree.PrintInOrder(incrementedTree.Root);

   

    }
}