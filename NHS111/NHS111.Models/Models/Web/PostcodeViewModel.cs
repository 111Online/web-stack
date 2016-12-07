using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Models.Web
{
    [Validator(typeof(PostCodeViewModelValidator))]
    public class PostcodeViewModel
    {
        public string Postcode { get; set; }
    }
}
