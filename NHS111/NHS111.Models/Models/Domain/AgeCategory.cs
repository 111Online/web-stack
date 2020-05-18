namespace NHS111.Models.Models.Domain
{
    using System;

    public enum AgeCategoryEnum
    {
        Unknown,
        Adult,
        Child,
        Toddler,
        Infant
    }

    public class AgeCategory
    {

        public static AgeCategory Adult = new AgeCategory(AgeCategoryEnum.Adult);
        public static AgeCategory Child = new AgeCategory(AgeCategoryEnum.Child);
        public static AgeCategory Toddler = new AgeCategory(AgeCategoryEnum.Toddler);
        public static AgeCategory Infant = new AgeCategory(AgeCategoryEnum.Infant);

        public string Value { get; private set; }

        public int MinimumAge { get; private set; }

        public int MaximumAge { get; private set; }

        public AgeCategory(AgeCategoryEnum ageCategory)
        {
            Initialise(ageCategory);
        }

        public AgeCategory(int age)
        {
            Initialise(age);
        }

        public AgeCategory(string ageCategory)
        {
            var lower = ageCategory.ToLower();

            if (lower == "adult" || lower == "a")
            {
                Initialise(AgeCategoryEnum.Adult);
            }
            else if (lower == "child" || lower == "c")
            {
                Initialise(AgeCategoryEnum.Child);
            }
            else if (lower == "toddler" || lower == "t")
            {
                Initialise(AgeCategoryEnum.Toddler);
            }
            else if (lower == "infant" || lower == "i")
            {
                Initialise(AgeCategoryEnum.Infant);
            }
            else
            {
                int age;
                if (!int.TryParse(ageCategory, out age))
                    throw new ArgumentException(string.Format("Unable to parse age category of ({0}), expected one of \"Adult\", \"A\", \"Child\", \"C\", \"Toddler\", \"T\", \"Infant\", \"I\" (case insensitive) or a string representation of an integer.", ageCategory));

                Initialise(age);
            }
        }

        private void Initialise(int age)
        {
            if (age >= 16) Initialise(AgeCategoryEnum.Adult);
            else if (5 <= age && age <= 15) Initialise(AgeCategoryEnum.Child);
            else if (1 <= age && age <= 4) Initialise(AgeCategoryEnum.Toddler);
            else Initialise(AgeCategoryEnum.Infant);
        }

        private void Initialise(AgeCategoryEnum ageCategory)
        {
            switch (ageCategory)
            {
                case AgeCategoryEnum.Adult:
                    Value = "Adult";
                    MinimumAge = 16;
                    MaximumAge = 119;
                    break;
                case AgeCategoryEnum.Child:
                    Value = "Child";
                    MinimumAge = 5;
                    MaximumAge = 15;
                    break;
                case AgeCategoryEnum.Toddler:
                    Value = "Toddler";
                    MinimumAge = 1;
                    MaximumAge = 4;
                    break;
                case AgeCategoryEnum.Infant:
                    Value = "Infant";
                    MinimumAge = 0;
                    MaximumAge = 0;
                    break;
            }

        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AgeCategory))
            {
                return false;
            }
            return Value == ((AgeCategory) obj).Value;
        }
    }
}