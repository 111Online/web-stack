namespace NHS111.Models.Models.Domain {
    using System;

    /// <summary>
    /// The ETL output currently only contains Male and Female
    /// </summary>
    public enum GenderEnum {
        //Unknown,
        Indeterminate,
        Male,
        Female,
        //Other
    }

    public class Gender
        :IEquatable<Gender> {

        public static Gender Male = new Gender(GenderEnum.Male);
        public static Gender Female = new Gender(GenderEnum.Female);
        public static Gender Indeterminate = new Gender(GenderEnum.Indeterminate);

        public string Value { get; private set; }
        public GenderEnum Enum { get; private set; }

        public Gender(string gender) {
            var lower = gender.ToLower();
            if (lower == "male" || lower == "m")
                Initialise(GenderEnum.Male);
            else if (lower == "female" || lower == "f")
                Initialise(GenderEnum.Female);
            else if (lower == "indeterminate")
                Initialise(GenderEnum.Indeterminate);
            else
                throw new ArgumentException(BuildMessage(gender));
        }

        public Gender(GenderEnum gender) {
            Initialise(gender);
        }

        public bool Equals(Gender other) {
            if (other == null)
                return false;
            return this.Enum == other.Enum;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var p = (Gender)obj;
            return Equals(p);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        private void Initialise(GenderEnum gender) {
            Enum = gender;
            switch (gender) {
                case GenderEnum.Male:
                    Value = "Male";
                    break;
                case GenderEnum.Female:
                    Value = "Female";
                    break;
                case GenderEnum.Indeterminate:
                    Value = "Indeterminate";
                    break;
                default:
                    throw new ArgumentException(BuildMessage(gender.ToString()));
            }
        }

        private static string BuildMessage(string gender) {
            return string.Format("The gender supplied ({0}) currently isn't supported. Please refer to {1}.", gender, typeof(GenderEnum).FullName);
        }
    }
}