using System;
using System.Collections.Generic;
using NHS111.Business.DOS.Configuration;
using NHS111.Models.Models.Business;

namespace NHS111.Business.DOS
{
    public class ProfileHoursOfOperation
    {
        private IConfiguration _configuration;
        public ProfileHoursOfOperation(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ProfileServiceTimes GeServiceTime(DateTime date)
        {
            throw new NotImplementedException();
        }



       
    }
}
