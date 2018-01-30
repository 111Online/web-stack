﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Models.Models.Business
{
    public class DosService : Web.FromExternalServices.DosService
    {
        [JsonProperty(PropertyName = "callbackEnabled")]
        public bool CallbackEnabled { get; set; }
    }
}