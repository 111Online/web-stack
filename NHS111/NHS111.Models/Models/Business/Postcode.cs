
namespace NHS111.Models.Models.Business
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{NormalisedValue}")]
    public class Postcode
        : IEquatable<Postcode>
    {

        public static implicit operator Postcode(string postcode)
        {
            return new Postcode(postcode);
        }

        public Postcode(string postcode)
        {
            Value = postcode;
        }

        public string NormalisedValue
        {
            get { return Normalise(Value); }
        }

        public static string Normalise(string postcode)
        {
            if (string.IsNullOrEmpty(postcode))
                return postcode;

            return postcode.Trim().Replace(" ", "").ToUpper();
        }

        public string Value { get; private set; }

        public bool Equals(Postcode other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return string.Equals(NormalisedValue, other.NormalisedValue);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != this.GetType())
                return false;

            return Equals((Postcode)obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}