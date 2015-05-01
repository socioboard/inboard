using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace BaseLib
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<TElement>> Partition<TElement>(this IEnumerable<TElement> @this, int partitionSize)
        {
            if (@this == null) throw new ArgumentNullException("this");

            return new PartitionedEnumerable<TElement>(@this, partitionSize);
        }

        private sealed class PartitionedEnumerable<TElement> : IEnumerable<IEnumerable<TElement>>
        {
            #region Public

            public PartitionedEnumerable(IEnumerable<TElement> elements, int partitionSize)
            {
                this.elements = elements;
                this.partitionSize = partitionSize;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<IEnumerable<TElement>> GetEnumerator()
            {
                IEnumerator<TElement> elementEnumerator = this.elements.GetEnumerator();

                var partition = new List<TElement>();
                while (elementEnumerator.MoveNext())
                {
                    partition.Add(elementEnumerator.Current);

                    if (partition.Count == partitionSize)
                    {
                        yield return partition;
                        partition = new List<TElement>();
                    }
                }

                if (partition.Count > 0) yield return partition;
            }

            #endregion

            #region Private

            private readonly IEnumerable<TElement> elements;
            private readonly int partitionSize;

            #endregion
        }
    }
}
