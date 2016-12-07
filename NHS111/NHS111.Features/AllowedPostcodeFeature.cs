using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public class AllowedPostcodeFeature : BaseFeature, IAllowedPostcodeFeature
    {

        public AllowedPostcodeFeature()
        {
            DefaultIsEnabledSettingStrategy = new EnabledByDefaultSettingStrategy();
        }

        public TextReader PostcodeFile
        {
            get
            {
                try
                {
                    var filePath = FeatureValue(new PostcodeFilePathDefaultSettingStrategy(), "PostcodeFilePath").Value;
                    return new StreamReader(filePath);
                }
                catch (Exception ex) // missing file
                {
                    if (ex is ArgumentException || ex is DirectoryNotFoundException || ex is FileNotFoundException) return (TextReader.Null);
                    throw;
                }
            }
        }
    }

    public interface IAllowedPostcodeFeature : IFeature
    {
        TextReader PostcodeFile { get; }
    }
}