using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace TestExamMaker.Models {
   public static class IdentityExtensions
    {
        public static string GetFirstName(this IIdentity identity)
        {
            var FirstName = ((AspNetUsers)identity).FindFirst("FirstName");
            // Test for null to avoid issues during local testing
            return (FirstName != null) ? FirstName.Value : string.Empty;
        }
    }
  }