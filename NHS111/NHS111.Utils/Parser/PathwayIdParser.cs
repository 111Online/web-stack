namespace NHS111.Utils.Parser
{
    using Models.Models.Domain;

    public static class PathwayIdParser
    {
        // format: {PATHWAYNUMBER}{Gender}{AgeCategory}
        // ex: PW999MaleAdult
        public static bool TryParse(string pathwayId, out string pathwayNumber, out Gender gender, out AgeCategory age)
        {
            age = null;
            gender = null;
            pathwayNumber = null;
            if (string.IsNullOrEmpty(pathwayId))
            {
                return false;
            }

            age = FindAndStripAgeCategory(ref pathwayId);
            if (age == null)
                return false;
            gender = FindAndStripGender(ref pathwayId);
            if (gender == null)
                return false;

            pathwayNumber = pathwayId;

            return true;
        }

        private static Gender FindAndStripGender(ref string pathwayId)
        {
            if (pathwayId.EndsWith(Gender.Female.Value))
            {
                pathwayId = pathwayId.Replace(Gender.Female.Value, "");
                return Gender.Female;
            }

            if (pathwayId.EndsWith(Gender.Male.Value))
            {
                pathwayId = pathwayId.Replace(Gender.Male.Value, "");
                return Gender.Male;
            }

            return null;
        }

        private static AgeCategory FindAndStripAgeCategory(ref string pathwayId)
        {
            if (pathwayId.EndsWith(AgeCategory.Adult.Value))
            {
                pathwayId = pathwayId.Replace(AgeCategory.Adult.Value, "");
                return AgeCategory.Adult;
            }

            if (pathwayId.EndsWith(AgeCategory.Child.Value))
            {
                pathwayId = pathwayId.Replace(AgeCategory.Child.Value, "");
                return AgeCategory.Child;
            }

            if (pathwayId.EndsWith(AgeCategory.Toddler.Value))
            {
                pathwayId = pathwayId.Replace(AgeCategory.Toddler.Value, "");
                return AgeCategory.Toddler;
            }

            if (pathwayId.EndsWith(AgeCategory.Infant.Value))
            {
                pathwayId = pathwayId.Replace(AgeCategory.Infant.Value, "");
                return AgeCategory.Infant;
            }

            return null;
        }
    }
}