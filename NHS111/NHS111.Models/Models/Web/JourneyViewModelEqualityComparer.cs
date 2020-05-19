
namespace NHS111.Models.Models.Web
{
    using FromExternalServices;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IJourneyViewModelEqualityComparer : IEqualityComparer<JourneyViewModel>
    {
        bool ComparePathwayNo { get; set; }
        bool CompareAge { get; set; }
        bool CompareSex { get; set; }
        bool CompareQuestionNos { get; set; }
        bool CompareAnswers { get; set; }
        bool Equals(JourneyViewModel x, JourneyViewModel y);
    }

    public class JourneyViewModelEqualityComparer
        : IJourneyViewModelEqualityComparer
    {

        public bool ComparePathwayNo { get; set; }
        public bool CompareAge { get; set; }
        public bool CompareSex { get; set; }
        public bool CompareQuestionNos { get; set; }
        public bool CompareAnswers { get; set; }

        public JourneyViewModelEqualityComparer()
        {
            ComparePathwayNo = true;
            CompareAge = false;
            CompareSex = false;
            CompareQuestionNos = true;
            CompareAnswers = true;
        }

        public bool Equals(JourneyViewModel x, JourneyViewModel y)
        {
            //a lot of these checks can be pulled out into their respective types
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            if (ComparePathwayNo && x.PathwayNo != y.PathwayNo)
                return false;

            if (CompareAge && !AgeEquals(x.UserInfo, y.UserInfo))
                return false;

            if (CompareSex && !SexEquals(x.UserInfo, y.UserInfo))
                return false;

            if ((!CompareQuestionNos && !CompareAnswers))
                return true; //we're done

            return JourneysEquals(x.Journey, y.Journey);
        }

        private bool JourneysEquals(Journey x, Journey y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            if (x.Steps == null && y.Steps == null)
                return true;

            if (x.Steps == null || y.Steps == null)
                return false;

            if (!x.Steps.Any() && !y.Steps.Any())
                return true;

            for (var i = 0; i < x.Steps.Count; i++)
            {

                if (x.Steps[i] == null && y.Steps[i] == null)
                    continue; //no need to compare these two any further

                if (x.Steps[i] == null || y.Steps[i] == null)
                    return false;

                if (CompareQuestionNos && !string.Equals(x.Steps[i].QuestionNo, y.Steps[i].QuestionNo,
                        StringComparison.CurrentCultureIgnoreCase))
                    return false;

                if (!CompareAnswers)
                    continue;

                //these checks could fold into Answer.Equals() for example
                if (x.Steps[i].Answer == null && y.Steps[i].Answer == null)
                    continue;

                if (x.Steps[i].Answer == null || y.Steps[i].Answer == null)
                    return false;

                if (x.Steps[i].Answer.Title.ToLower() != y.Steps[i].Answer.Title.ToLower())
                    return false;
            }

            return true;
        }

        private bool SexEquals(UserInfo x, UserInfo y)
        {
            if (!DemographyEquals(x, y))
                return false;

            return x.Demography.Age == y.Demography.Age;
        }

        private static bool DemographyEquals(UserInfo x, UserInfo y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            if (x.Demography == null && y.Demography == null)
                return true;

            if (x.Demography == null || y.Demography == null)
                return false;

            return false;
        }

        private bool AgeEquals(UserInfo x, UserInfo y)
        {
            if (!DemographyEquals(x, y))
                return false;

            return x.Demography.Gender == y.Demography.Gender;
        }

        public int GetHashCode(JourneyViewModel obj)
        {
            throw new NotImplementedException();
        }
    }
}