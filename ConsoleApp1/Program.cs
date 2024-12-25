namespace AVL_Tree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tree tree = new Tree();
            tree.TInsert(5);
            tree.TInsert(9);
            tree.TInsert(3);

            Tree tree2 = new Tree();
            tree2.TInsert(20);
            tree2.TInsert(19);
            tree2.TInsert(21);
            tree2.TInsert(23);
            tree2.TInsert(26);
            tree2.TInsert(17);

            Tree tree1_1 = new Tree();
            tree1_1.TInsert(5);
            tree1_1.TInsert(3);
            tree1_1.TInsert(6);
            tree1_1.TInsert(2);
            tree1_1.TInsert(4);
            tree1_1.TInsert(9);

            Tree tree2_2 = new Tree();
            tree2_2.TInsert(20);
            tree2_2.TInsert(19);
            tree2_2.TInsert(21);

            Tree treeTest1 = new Tree();
            treeTest1.TInsert(2);
            treeTest1.TInsert(1);
            treeTest1.TInsert(3);

            Tree treeTest2 = new Tree();
            treeTest2.TInsert(7);
            treeTest2.TInsert(6);
            treeTest2.TInsert(8);

            /*tree.Print();

            Console.WriteLine();

            tree2.Print();

            Tree tree3 = new Tree();

            tree3 = tree + tree2;*/

            Tree tree4 = new Tree();

            tree4 = tree1_1 + tree2_2;

            /*Tree tree5 = new Tree();

            tree5 = treeTest2 + treeTest1;

            tree += 2;

            Tree tree6 = new Tree();

            tree6 = tree + tree2;

            Console.WriteLine();

            tree.Print();

            Console.WriteLine();

            tree3.Print();

            Console.WriteLine();*/

            tree4.Print();

            Console.WriteLine();

            /*tree5.Print();

            Console.WriteLine();

            tree6.Print();*/
        }
    }
    public class Node
    {
        int _data;
        public int Data { get { return _data; } set { _data = value; } }

        Node? _left;
        public Node? Left { get { return _left; } set { _left = value; } }

        Node? _right;
        public Node? Right { get { return _right; } set { _right = value; } }

        public Node(int data)
        {
            _data = data;
        }
    }

    public class Tree
    {
        Node? _root;
        Node? indicate;

        public void TInsert(int data)
        {

            if (_root == null)
            {
                _root = new Node(data);
            }
            else
            {
                _root = RecursiveInsert(_root, data);
            }
        }
        private Node RecursiveInsert(Node current, int data)
        {
            if (current == null)
            {

                current = new Node(data);
                return current;
            }
            else if (data < current.Data)
            {
                current.Left = RecursiveInsert(current.Left, data);
                current = tBalance(current);
            }
            else if (data > current.Data)
            {
                current.Right = RecursiveInsert(current.Right, data);
                current = tBalance(current);
            }
            return current;
        }

        private Node tBalance(Node current)
        {
            int b_factor = balance_factor(current);
            if (b_factor > 1)
            {
                if (balance_factor(current.Left) > 0)
                {
                    current = RotateLL(current);
                }
                else
                {
                    current = RotateLR(current);
                }
            }
            else if (b_factor < -1)
            {
                if (balance_factor(current.Right) > 0)
                {
                    current = RotateRL(current);
                }
                else
                {
                    current = RotateRR(current);
                }
            }
            return current;
        }

        public void TDelete(int target)
        {
            _root = TDelete(_root, target);
        }
        private Node TDelete(Node current, int target)
        {
            Node parent;
            if (current == null)
            { return null; }
            else
            {
                if (target < current.Data)
                {
                    current.Left = TDelete(current.Left, target);
                    if (balance_factor(current) == -2)
                    {
                        if (balance_factor(current.Right) <= 0)
                        {
                            current = RotateRR(current);
                        }
                        else
                        {
                            current = RotateRL(current);
                        }
                    }
                }
                else if (target > current.Data)
                {
                    current.Right = TDelete(current.Right, target);
                    if (balance_factor(current) == 2)
                    {
                        if (balance_factor(current.Left) >= 0)
                        {
                            current = RotateLL(current);
                        }
                        else
                        {
                            current = RotateLR(current);
                        }
                    }
                }
                else
                {
                    if (current.Right != null)
                    {
                        parent = current.Right;
                        while (parent.Left != null)
                        {
                            parent = parent.Left;
                        }
                        current.Data = parent.Data;
                        current.Right = TDelete(current.Right, parent.Data);
                        if (balance_factor(current) == 2)
                        {
                            if (balance_factor(current.Left) >= 0)
                            {
                                current = RotateLL(current);
                            }
                            else { current = RotateLR(current); }
                        }
                    }
                    else
                    {
                        return current.Left;
                    }
                }
            }
            return current;
        }

        public bool TFind(int pattern)
        {
            if (TFind(pattern, _root).Data == pattern)
            {
                Console.WriteLine($"{pattern} was found!");
                return true;
            }
            else
            {
                Console.WriteLine("Nothing found!");
                return false;
            }
        }

        private Node TFind(int target, Node current)
        {

            if (target < current.Data)
            {
                if (target == current.Data)
                {
                    return current;
                }
                else
                    return TFind(target, current.Left);
            }
            else
            {
                if (target == current.Data)
                {
                    return current;
                }
                else
                    return TFind(target, current.Right);
            }

        }

        public void Print()
        {
            if (_root == null)
            {
                Console.WriteLine("Tree is empty");
                return;
            }
            InOrderPrint(_root);
            Console.WriteLine();
        }
        private void InOrderPrint(Node current)
        {
            if (current != null)
            {
                Console.Write("({0}) ", current.Data);
                InOrderPrint(current.Left);
                
                InOrderPrint(current.Right);
            }
        }

        private int max(int l, int r)
        {
            if (l > r)
            {
                return l;
            }
            else
            {
                return r;
            }
        }
        private int getHeight(Node current)
        {
            int height = 0;
            if (current != null)
            {
                int l = getHeight(current.Left);
                int r = getHeight(current.Right);
                int m = max(l, r);
                height = m + 1;
            }
            return height;
        }

        private int balance_factor(Node current)
        {
            int l = getHeight(current.Left);
            int r = getHeight(current.Right);
            int b_factor = l - r;
            return b_factor;
        }

        private Node RotateRR(Node parent)
        {
            Node pivot = parent.Right;
            parent.Right = pivot.Left;
            pivot.Left = parent;
            return pivot;
        }
        private Node RotateLL(Node parent)
        {
            Node pivot = parent.Left;
            parent.Left = pivot.Right;
            pivot.Right = parent;
            return pivot;
        }
        private Node RotateLR(Node parent)
        {
            Node pivot = parent.Left;
            parent.Left = RotateRR(pivot);
            return RotateLL(parent);
        }
        private Node RotateRL(Node parent)
        {
            Node pivot = parent.Right;
            parent.Right = RotateLL(pivot);
            return RotateRR(parent);
        }

        public static Tree operator +(Tree tree, int num)
        {
            Tree newTree = new Tree();

            InOrderPlus(tree._root, newTree, num);

            return newTree;
        }

        private static void InOrderPlus(Node? current, Tree newTree, int num)
        {
            if (current != null)
            {
                InOrderPlus(current.Left, newTree, num);
                newTree.TInsert(current.Data + num);
                InOrderPlus(current.Right, newTree, num);
            }
        }

        public static Tree operator +(Tree? tree1, Tree? tree2)
        {
            Tree newTree = new Tree();

            if (tree1 == null)
            {
                newTree = tree2;
            }
            else if (tree2 == null)
            {
                newTree = tree1;
            }
            else if (tree1.TMin() > tree2.TMax())
            {
                Tree treeBuf = new Tree();
                Tree copy1Tree = new Tree();
                Tree copy2Tree = new Tree();
                CopyTree(tree1._root, treeBuf);
                CopyTree(tree2._root, copy1Tree);
                CopyTree(treeBuf._root, copy2Tree);
                newTree = copy1Tree + copy2Tree;
            }
            else if (tree1.TMax() < tree2.TMin())
            {
                if (tree1.getHeight(tree1._root) < tree2.getHeight(tree2._root))
                {
                    CopyTree(tree2._root, newTree);
                    newTree.indicate = newTree._root;
                    Tree tree1Copy = new Tree();
                    CopyTree(tree1._root, tree1Copy);
                    Node indicator;
                    Node x = tree1Copy.TFind(tree1Copy.TMax(), tree1Copy._root);
                    tree1Copy.TDelete(tree1Copy._root, tree1Copy.TMax());
                    int t1Height = tree1Copy.getHeight(tree1Copy._root);


                    if (t1Height - 1 == 1)
                    {
                        indicator = newTree._root;
                    }
                    else
                    {
                        for (int i = 0; i < t1Height - 1; i++)
                        {
                            newTree.indicate = newTree.indicate.Left;
                            indicator = newTree.indicate;
                        }
                        indicator = newTree.indicate;
                    }
                    x.Right = indicator.Left;
                    indicator.Left = x;
                    x.Left = tree1Copy._root;
                }
                else if (tree1.getHeight(tree1._root) == tree2.getHeight(tree2._root))
                {
                    CopyTree(tree1._root, newTree);
                    newTree.indicate = newTree._root;
                    Tree tree2Copy = new Tree();
                    CopyTree(tree2._root, tree2Copy);
                    Node copyRoot = newTree._root.Right;
                    newTree.TDelete(newTree._root.Right.Data);
                    /*Node x = tree2Copy.TFind(tree2Copy.TMin(), tree2Copy._root);
                    tree2Copy.TDelete(tree2Copy._root, tree2Copy.TMin());*/
                    //int t1Height = tree1Copy.getHeight(tree1Copy._root);

                    copyRoot.Left = newTree._root;
                    newTree._root = copyRoot;

                    newTree._root.Right = tree2Copy._root;

                    /*x.Left = indicator.Right;
                    indicator.Right = x;
                    x.Right = tree2Copy._root;*/
                }
                else if (tree1.getHeight(tree1._root) > tree2.getHeight(tree2._root))
                {
                    CopyTree(tree1._root, newTree);
                    newTree.indicate = newTree._root;
                    Tree tree2Copy = new Tree();
                    CopyTree(tree2._root, tree2Copy);
                    Node indicator;
                    Node x = tree2Copy.TFind(tree2Copy.TMin(), tree2Copy._root);
                    tree2Copy.TDelete(tree2Copy._root, tree2Copy.TMin());
                    int t2Height = tree2Copy.getHeight(tree2Copy._root);
                    if (t2Height - 1 == 1)
                    {
                        indicator = newTree._root;
                    }
                    else
                    {
                        for (int i = 0; i < t2Height - 1; i++)
                        {
                            newTree.indicate = newTree.indicate.Right;
                            indicator = newTree.indicate;
                        }
                        indicator = newTree.indicate;
                    }
                    x.Left = indicator.Right;
                    indicator.Right = x;
                    x.Right = tree2Copy._root;
                }
            }
            else
            {
                CopyTree(tree1._root, newTree);
                CopyTree(tree2._root, newTree);
            }

            return newTree;
        }

        private static void CopyTree(Node? current, Tree newTree)
        {
            if (current != null)
            {
                CopyTree(current.Left, newTree);
                newTree.TInsert(current.Data);
                CopyTree(current.Right, newTree);
            }
        }
        public int TMin()
        {
            if (_root == null)
            {
                Console.WriteLine("Tree is empty");
                return 0;
            }
            return tMin(_root);
        }
        private int tMin(Node current)
        {
            while (current.Left != null)
            {
                current = current.Left;
            }
            return current.Data;
        }

        public int TMax()
        {
            if (_root == null)
            {
                Console.WriteLine("Tree is empty");
                return 0;
            }
            return tMax(_root);
        }
        private int tMax(Node current)
        {
            while (current.Right != null)
            {
                current = current.Right;
            }
            return current.Data;
        }
    }
}