namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal class ImmutableList<T> : IReadOnlyList<T>
    {
        internal static readonly ImmutableList<T> Empty = new ImmutableList<T>();
        private readonly T[] data;

        private ImmutableList()
        {
            this.data = new T[0];
        }

        public ImmutableList(T[] data)
        {
            this.data = data;
        }

        public int Count => this.data.Length;

        public T this[int index] => this.data[index];

        public ImmutableList<T> Add(T value)
        {
            var newData = new T[this.data.Length + 1];
            Array.Copy(this.data, newData, this.data.Length);
            newData[this.data.Length] = value;
            return new ImmutableList<T>(newData);
        }

        public ImmutableList<T> Remove(T value)
        {
            var i = this.IndexOf(value);
            if (i < 0)
            {
                return this;
            }

            var length = this.data.Length;
            if (length == 1)
            {
                return Empty;
            }

            var newData = new T[length - 1];

            Array.Copy(this.data, 0, newData, 0, i);
            Array.Copy(this.data, i + 1, newData, i, length - i - 1);

            return new ImmutableList<T>(newData);
        }

        public int IndexOf(T value)
        {
            return Array.IndexOf(this.data, value);
        }

        public IEnumerator<T> GetEnumerator() => ((IList<T>)this.data).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}