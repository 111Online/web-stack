using System.IO;
using System.Linq;
using CsvHelper;
using NHS111.Features;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Models.Models.Web.Validators
{
    public class PostCodeAllowedValidator: IPostCodeAllowedValidator
    {
        private readonly IAllowedPostcodeFeature _allowedPostcodeFeature;

        public PostCodeAllowedValidator(IAllowedPostcodeFeature allowedPostcodeFeature)
        {
            _allowedPostcodeFeature = allowedPostcodeFeature;
        }

        public bool IsAllowedPostcode(string postcode)
        {
            if (!_allowedPostcodeFeature.IsEnabled) return true;

            var postcodeFile = _allowedPostcodeFeature.PostcodeFile;
            if(postcodeFile == TextReader.Null) return false;

            var csv = new CsvReader(postcodeFile);
            var postcodes = csv.GetRecords<ValidPostCode>().ToList();
            return postcodes.Any(p => p.ParsedPostcode.Contains(ValidPostCode.ParsePostcode(postcode)));
        }
    }
     
    public interface IPostCodeAllowedValidator
    {
        bool IsAllowedPostcode(string postcode);
    }
}
