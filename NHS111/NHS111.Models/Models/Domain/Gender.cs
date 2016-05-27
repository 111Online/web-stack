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

    public class Gender {

        public static Gender Male = new Gender(GenderEnum.Male);
        public static Gender Female = new Gender(GenderEnum.Female);

        public string Value { get; private set; }

        public Gender(string gender) {
            var lower = gender.ToLower();
            if (lower == "male" || lower == "m")
                Initialise(GenderEnum.Male);
            else if (lower == "female" || lower == "f")
                Initialise(GenderEnum.Female);
            else
                throw new ArgumentException(BuildMessage(gender));
        }

        public Gender(GenderEnum gender) {
            Initialise(gender);
        }

        private void Initialise(GenderEnum gender) {
            switch (gender) {
                case GenderEnum.Male:
                    Value = "Male";
                    break;
                case GenderEnum.Female:
                    Value = "Female";
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