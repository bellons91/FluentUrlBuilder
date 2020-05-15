using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UrlBuilder
{
    public class UrlBuilder
    {
        StringBuilder urlBuilder;
        Dictionary<string, string> qs;
        string hashPart = "";

        private UrlBuilder(params string[] parts)
        {
            var cleanedParts = parts.Select(p => p.Trim('/')).Where(p => !string.IsNullOrWhiteSpace(p));
            urlBuilder = new StringBuilder(string.Join("/", cleanedParts));
            qs = new Dictionary<string, string>();
        }

        public static UrlBuilder Initialize(params string[] parts)
        {
            return new UrlBuilder(parts);
        }

        public UrlBuilder AddPart(string part)
        {
            part = part.Trim('/');
            if (!string.IsNullOrWhiteSpace(part))
                this.urlBuilder.Append('/').Append(part);
            return this;
        }

        public UrlBuilder AddHashValue(string part)
        {
            if (!string.IsNullOrWhiteSpace(part))
                this.hashPart = part;
            return this;
        }

        public UrlBuilder AddQueryStringPair(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(key))
                qs.Add(key, value);
            return this;
        }


        public string GetResult()
        {
            if (qs.Keys.Any())
            {
                this.urlBuilder.Append("?");

                var sel = qs.Select(x => $"{x.Key}={x.Value}");
                urlBuilder.Append(string.Join("?", sel));
            }
            if (!string.IsNullOrWhiteSpace(hashPart))
                urlBuilder.Append($"#{hashPart}");

            return urlBuilder.ToString();

        }
    }

}
