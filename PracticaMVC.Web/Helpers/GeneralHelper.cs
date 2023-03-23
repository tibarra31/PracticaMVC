using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PracticaMVC.Web.Helpers
{
    public static class GeneralHelper
    {
        public static RouteValueDictionary AddIf(this RouteValueDictionary dict, bool condition, string name, object value)
        {
            if (condition) dict.Add(name, value);
            return dict;
        }
    }
}