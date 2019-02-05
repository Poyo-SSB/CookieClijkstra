using System;
using System.Collections.Generic;

namespace CookieClijkstra
{
    public class FibonacciHeap<T, TKey> where TKey : IComparable<TKey>
    {
        private static readonly double one_over_log_phi = 1.0 / Math.Log((1.0 + Math.Sqrt(5.0)) / 2.0);

        private readonly TKey minimumKeyValue;

        public bool IsEmpty => this.Root == null;
        public int Size { get; private set; }
        public FibonacciHeapNode<T, TKey> Root { get; private set; }

        public FibonacciHeap(TKey minKeyValue)
        {
            this.minimumKeyValue = minKeyValue;
        }

        public void Clear()
        {
            this.Root = null;
            this.Size = 0;
        }

        public void DecreaseKey(FibonacciHeapNode<T, TKey> node, TKey key)
        {
            if (key.CompareTo(node.Key) > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(key));
            }

            node.Key = key;

            FibonacciHeapNode<T, TKey> y = node.Parent;

            if ((y != null) && (node.Key.CompareTo(y.Key) < 0))
            {
                this.Cut(node, y);
                this.CascadingCut(y);
            }

            if (node.Key.CompareTo(this.Root.Key) < 0)
            {
                this.Root = node;
            }
        }

        public void Delete(FibonacciHeapNode<T, TKey> node)
        {
            this.DecreaseKey(node, this.minimumKeyValue);
            this.Pop();
        }

        public FibonacciHeapNode<T, TKey> Insert(T data, TKey key)
        {
            var node = new FibonacciHeapNode<T, TKey>(data, key);

            if (this.Root != null)
            {
                node.Left = this.Root;
                node.Right = this.Root.Right;
                this.Root.Right = node;
                node.Right.Left = node;

                if (node.Key.CompareTo(this.Root.Key) < 0)
                {
                    this.Root = node;
                }
            }
            else
            {
                this.Root = node;
            }

            this.Size++;

            return node;
        }

        public FibonacciHeapNode<T, TKey> Pop()
        {
            FibonacciHeapNode<T, TKey> minNode = this.Root;

            if (minNode != null)
            {
                int numKids = minNode.Degree;
                FibonacciHeapNode<T, TKey> oldMinChild = minNode.Child;

                while (numKids > 0)
                {
                    FibonacciHeapNode<T, TKey> tempRight = oldMinChild.Right;

                    oldMinChild.Left.Right = oldMinChild.Right;
                    oldMinChild.Right.Left = oldMinChild.Left;

                    oldMinChild.Left = this.Root;
                    oldMinChild.Right = this.Root.Right;
                    this.Root.Right = oldMinChild;
                    oldMinChild.Right.Left = oldMinChild;

                    oldMinChild.Parent = null;
                    oldMinChild = tempRight;
                    numKids--;
                }

                minNode.Left.Right = minNode.Right;
                minNode.Right.Left = minNode.Left;

                if (minNode == minNode.Right)
                {
                    this.Root = null;
                }
                else
                {
                    this.Root = minNode.Right;
                    this.Consolidate();
                }

                this.Size--;
            }

            return minNode;
        }

        protected void CascadingCut(FibonacciHeapNode<T, TKey> y)
        {
            FibonacciHeapNode<T, TKey> z = y.Parent;

            if (z != null)
            {
                if (!y.Marked)
                {
                    y.Marked = true;
                }
                else
                {
                    this.Cut(y, z);

                    this.CascadingCut(z);
                }
            }
        }

        protected void Consolidate()
        {
            int arraySize = ((int)Math.Floor(Math.Log(this.Size) * one_over_log_phi)) + 1;

            var array = new List<FibonacciHeapNode<T, TKey>>(arraySize);

            for (int i = 0; i < arraySize; i++)
            {
                array.Add(null);
            }

            int numRoots = 0;
            FibonacciHeapNode<T, TKey> x = this.Root;

            if (x != null)
            {
                numRoots++;
                x = x.Right;

                while (x != this.Root)
                {
                    numRoots++;
                    x = x.Right;
                }
            }

            while (numRoots > 0)
            {
                int d = x.Degree;
                FibonacciHeapNode<T, TKey> next = x.Right;

                while (true)
                {
                    FibonacciHeapNode<T, TKey> y = array[d];
                    if (y == null)
                    {
                        break;
                    }

                    if (x.Key.CompareTo(y.Key) > 0)
                    {
                        FibonacciHeapNode<T, TKey> temp = y;
                        y = x;
                        x = temp;
                    }

                    this.Link(y, x);

                    array[d] = null;
                    d++;
                }

                array[d] = x;

                x = next;
                numRoots--;
            }

            this.Root = null;

            for (int i = 0; i < arraySize; i++)
            {
                FibonacciHeapNode<T, TKey> y = array[i];
                if (y == null)
                {
                    continue;
                }

                if (this.Root != null)
                {
                    y.Left.Right = y.Right;
                    y.Right.Left = y.Left;

                    y.Left = this.Root;
                    y.Right = this.Root.Right;
                    this.Root.Right = y;
                    y.Right.Left = y;

                    if (y.Key.CompareTo(this.Root.Key) < 0)
                    {
                        this.Root = y;
                    }
                }
                else
                {
                    this.Root = y;
                }
            }
        }

        protected void Cut(FibonacciHeapNode<T, TKey> x, FibonacciHeapNode<T, TKey> y)
        {
            x.Left.Right = x.Right;
            x.Right.Left = x.Left;
            y.Degree--;

            if (y.Child == x)
            {
                y.Child = x.Right;
            }

            if (y.Degree == 0)
            {
                y.Child = null;
            }

            x.Left = this.Root;
            x.Right = this.Root.Right;
            this.Root.Right = x;
            x.Right.Left = x;

            x.Parent = null;

            x.Marked = false;
        }

        protected void Link(FibonacciHeapNode<T, TKey> newChild, FibonacciHeapNode<T, TKey> newParent)
        {
            newChild.Left.Right = newChild.Right;
            newChild.Right.Left = newChild.Left;

            newChild.Parent = newParent;

            if (newParent.Child == null)
            {
                newParent.Child = newChild;
                newChild.Right = newChild;
                newChild.Left = newChild;
            }
            else
            {
                newChild.Left = newParent.Child;
                newChild.Right = newParent.Child.Right;
                newParent.Child.Right = newChild;
                newChild.Right.Left = newChild;
            }

            newParent.Degree++;

            newChild.Marked = false;
        }
    }

    public class FibonacciHeapNode<T, TKey> where TKey : IComparable<TKey>
    {
        public FibonacciHeapNode(T data, TKey key)
        {
            this.Right = this;
            this.Left = this;
            this.Data = data;
            this.Key = key;
        }

        public T Data { get; set; }

        public FibonacciHeapNode<T, TKey> Child { get; set; }
        public FibonacciHeapNode<T, TKey> Left { get; set; }
        public FibonacciHeapNode<T, TKey> Parent { get; set; }
        public FibonacciHeapNode<T, TKey> Right { get; set; }

        public bool Marked { get; set; }

        public TKey Key { get; set; }

        public int Degree { get; set; }
    }
}