using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Arcadia.Model
{
    /// <summary>
    /// ArrivalCollection
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable&lt;Arcadia.Model.Arrivals&gt;" />
    /// <seealso cref="System.Collections.Generic.IEnumerable&lt;Arcadia.Model.Arrivals&gt;" />
    /// <seealso cref="System.Collections.Generic.IEnumerable&lt;Arcadia.Model.Arrivals&gt;" />
    [Serializable]
    public class ArrivalCollection: IEnumerable<Arrivals>
    {
        /// <summary>
        /// The arrivals
        /// </summary>
        private List<Arrivals> _arrivals;

        /// <summary>
        /// Gets or sets the icao.
        /// </summary>
        /// <value>
        /// The icao.
        /// </value>
        public string ICAO { get; set; }

        /// <summary>
        /// Gets or sets the begin.
        /// </summary>
        /// <value>
        /// The begin.
        /// </value>
        public int Begin { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        public int End { get; set; }

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrivalCollection" /> class.
        /// </summary>
        /// <param name="icao">The icao.</param>
        /// <param name="begin">The begin.</param>
        /// <param name="end">The end.</param>
        /// <param name="arrivals">The arrivals.</param>
        public ArrivalCollection(string icao, int begin, int end, List<Arrivals> arrivals)
        {
            this.ICAO = icao;
            this.Begin = begin;
            this.End = end;
            _arrivals = arrivals;
        }
        #endregion

        #region Indexer
        /// <summary>
        /// Gets or sets the <see cref="Arrivals" /> with the specified i.
        /// </summary>
        /// <value>
        /// The <see cref="Arrivals" />.
        /// </value>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public Arrivals this[int i]
        {
            get { return _arrivals[i]; }
            set { _arrivals[i] = value; }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get
            {
                if (_arrivals == null)
                    return 0;
                return _arrivals.Count;
            }
        }
        #endregion

        #region IEnumerable
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Arrivals> GetEnumerator()
        {
            return _arrivals.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator1.
        /// </summary>
        /// <returns></returns>
        private IEnumerator GetEnumerator1()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }
        #endregion

        #region EqualityComparasion        
        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(ArrivalCollection item1, ArrivalCollection item2)
        {
            if (object.ReferenceEquals(item1, item2))
                return true;
            else if (!object.ReferenceEquals(item1, null))
                return item1.Equals(item2);
            else
                return item2.Equals(item1);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(ArrivalCollection item1, ArrivalCollection item2)
        {
            bool result = item1 == item2;
            return !result;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as ArrivalCollection);
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        private bool Equals(ArrivalCollection other)
        {
            if (object.ReferenceEquals(other, null))
                return false;
            if (Object.ReferenceEquals(this, other))
                return true;

            return this.ICAO == other.ICAO && this.Begin == other.Begin && this.End == other.End && this.Count == other.Count;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
