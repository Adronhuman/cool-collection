using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;

namespace CoolCollection.CoolList
{
    public class CoolList<T> : IEnumerable<T> where T : struct, IComparable<T>, IEquatable<T>
    {
        private const int DefaultCapacity = 20;
        private T[] _items;
        private int _count;
        private int _indexOfMax;

        public int Count
        {
            get { return _count; }
        }

        public T MaxElement
        {
            get { return _items[_indexOfMax]; }
        }

        public int IndexOfMax
        {
            get { return _indexOfMax; }
        }

        public CoolList()
        {
            _items = new T[] {};
        }

        public CoolList(int capacity)
        {
            _items = new T[capacity];
        }

        public T this[int index]
        {
            get
            {
                if (index >= Count || -index > Count)
                {
                    throw new IndexOutOfRangeException();
                }

                if (index < 0 && -index <= Count)
                {
                    return _items[Count + index];
                }

                return _items[index];
            }

            set
            {
                if (index >= Count || -index > Count)
                {
                    throw new IndexOutOfRangeException();
                }

                if (index < 0 && -index < Count)
                {
                    _items[Count + index] = value;
                    _indexOfMax = value.CompareTo(_items[_indexOfMax]) > 0 ? Count + index : _indexOfMax;
                }
                else
                {
                    _items[index] = value;
                    _indexOfMax = value.CompareTo(_items[_indexOfMax]) > 0 ? index : _indexOfMax;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return _items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(T value)
        {
            if (Count < _items.Length)
            {
                _items[Count] = value;
            }
            else
            {
                ResizeArray(Math.Max(DefaultCapacity, _items.Length * 2));
                _items[Count] = value;
            }

            _count += 1;

            _indexOfMax = value.CompareTo(_items[_indexOfMax]) > 0 ? Count -1 : _indexOfMax;
        }

        public void Insert(int index, T value)
        {
            if (index > Count)
                throw new ArgumentOutOfRangeException();

            if (index >= _items.Length)
                ResizeArray(Math.Max(DefaultCapacity, _items.Length * 2));

            Array.Copy(_items, index, _items, index + 1, Count - index);
            _items[index] = value;

            _count += 1;
            _indexOfMax = value.CompareTo(_items[_indexOfMax]) > 0 ? index : _indexOfMax;
        }

       
        public void Remove(T value)
        {
            int? indexOf = null;
            for (int i = 0; i < Count; i++)
            {
                if (_items[i].Equals(value))
                {
                    indexOf = i;
                    break;
                }
            }

            if (indexOf.HasValue) RemoveAt(indexOf.Value);
        }

        public void RemoveAt(int index)
        {
            _count--;
            if (index < Count)
                Array.Copy(_items, index + 1, _items, index, Count - index);

            for (int i = 0; i < Count; i++)
            {
                _indexOfMax = _items[i].CompareTo(_items[_indexOfMax]) > 0 ? i : _indexOfMax;
            }
        }

        private bool ResizeArray(int newCapacity)
        {
            if (newCapacity <= _items.Length)
                return false;

            var newArray = new T[newCapacity];
            Array.Copy(_items, newArray, Count);
            _items = newArray;

            return true;
        }
    }



}