namespace NHS111.DOS.Functional.Tests.TestBenchApi
{
    using Models.Models.Domain;
    using Models.Models.Web.DosRequests;

    public static class BlankDosCase
    {
        public static DosFilteredCase WithDxCode(DispositionCode dispositionCode)
        {
            return new DosFilteredCase
            {
                Disposition = dispositionCode.DosCode
            };
        }
    }
}