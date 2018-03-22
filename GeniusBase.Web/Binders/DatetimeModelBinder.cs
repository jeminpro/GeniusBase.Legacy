﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeniusBase.Web.Binders
{
    // Not used at the moment
    public class DateTimeModelBinder : DefaultModelBinder
    {
        private string _customFormat;

        public DateTimeModelBinder(string customFormat)
        {
            _customFormat = customFormat;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            return DateTime.ParseExact(value.AttemptedValue, _customFormat, CultureInfo.InvariantCulture);
        }
    }
}