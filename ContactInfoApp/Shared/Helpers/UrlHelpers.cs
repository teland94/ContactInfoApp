using System;
using System.Collections.Generic;

namespace ContactInfoApp.Shared.Helpers
{
    public static class UrlHelpers
    {
        public static string AddQueryParameters(string url, IDictionary<string, string> parameters)
        {
            var uriBuilder = new UriBuilder(url);
            var q = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var (key, value) in parameters)
            {
                q[key] = value;
            }
            uriBuilder.Query = q.ToString() ?? string.Empty;

            return $"{uriBuilder.Host}{uriBuilder.Path}{uriBuilder.Query}";
        }

        public static string GetQueryParameter(string url, string paramName)
        {
            var uriBuilder = new UriBuilder(url);
            var q = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            return q[paramName] ?? string.Empty;
        }
    }
}
