using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;


[assembly: InternalsVisibleTo("FluentUrlBuilder.Tests")]
namespace FluentUrlBuilder
{
    /// <summary>
    /// Creates and build a URL. The building operations can be appendend.
    /// </summary>
    public class FluentUrlBuilder
    {
        //private StringBuilder urlBuilder;

        internal List<string> urlParts;
        internal Dictionary<string, string> queryStringValues;
        internal string hashFragment = "";

        private FluentUrlBuilder(params string[] parts)
        {
            var cleanedParts = parts.Select(p => p.Trim('/')).Where(p => !string.IsNullOrWhiteSpace(p));
            //urlBuilder = new StringBuilder(string.Join("/", cleanedParts));
            queryStringValues = new Dictionary<string, string>();
            urlParts = new List<string>();
            urlParts.AddRange(cleanedParts);
        }

        /// <summary>
        /// Initializes a UrlBuilder object.
        /// </summary>
        /// <param name="parts">Inital parts used to build the final URL. 
        /// </param>
        /// <remarks>All the parts are trimmed of the initial and final slashes, and if the part is not empty it is used to build the URL.</remarks>
        /// <example>
        /// To initialize a builder, you can write <c>var builder = UrlBuilder.Initialize("http://example.com", "a-section", "a-subfolder")</c>.
        /// </example>
        /// <returns>Returns the same UrlBuilder object that can be used to concatenate other methods.</returns>
        public static FluentUrlBuilder Initialize(params string[] parts)
        {
            if (parts == null || parts.Length == 0 || parts.Where(p => !string.IsNullOrWhiteSpace(TrimString(p))).Count() == 0)
                throw new ArgumentException("Unable to initialize builder without valid parts");
            return new FluentUrlBuilder(parts);
        }

        /// <summary>
        /// Adds a path part to the builder, only if the part is valid.
        /// </summary>
        /// <param name="pathPart">part to be added to the builder</param>
        /// <returns>Returns the same UrlBuilder object that can be used to concatenate other methods.</returns>
        public FluentUrlBuilder AddPathPart(string pathPart)
        {
            pathPart = FluentUrlBuilder.TrimString(pathPart);
            if (!string.IsNullOrWhiteSpace(pathPart))
                this.urlParts.Add(pathPart);
            return this;
        }

        /// <summary>
        /// Adds a path part to the builder, only if the part is valid and the <paramref name="add"/> parameter is true.
        /// </summary>
        /// <remarks>The <paramref name="add"/> parameter allows you to conditionally add a part while building the URL.
        /// </remarks>
        /// <example>
        /// <code>
        /// var part = "my-page";
        /// var builder = UrlBuilder.Initialize("http://example.com").AddPart(part, (part.Length > 5));
        /// </code>
        /// </example>
        /// <param name="pathPart">Part to be added</param>
        /// <param name="add">Initial check before adding the parameter</param>
        /// <returns>Returns the same UrlBuilder object that can be used to concatenate other methods.</returns>
        public FluentUrlBuilder AddPathPart(string pathPart, bool add)
        {
            if (add)
                return this.AddPathPart(pathPart);
            return this;
        }

        /// <summary>
        /// Adds or overwrites a Fragment to the URL, (ex: http://example.com/path#fragment)
        /// </summary>
        /// <param name="fragment">Fragment part to be added. If a fragment has already been set, it will be replaced with the new one</param>
        /// <returns>Returns the same UrlBuilder object that can be used to concatenate other methods.</returns>
        public FluentUrlBuilder SetHashFragment(string fragment)
        {
            this.hashFragment = fragment;
            return this;
        }


        /// <summary>
        /// Adds or overwrites a query string value.
        /// </summary>
        /// <param name="key">Key of the query string</param>
        /// <param name="value">Value of the query string</param>
        /// <returns>Returns the same UrlBuilder object that can be used to concatenate other methods.</returns>
        public FluentUrlBuilder UpsertQueryStringPair(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                if (queryStringValues.ContainsKey(key))
                    queryStringValues[key] = value;
                else
                    queryStringValues.Add(key, value);
            }
            return this;
        }

        /// <summary>
        /// Removes a query string value.
        /// </summary>
        /// <param name="key">Key to be removed</param>
        /// <returns>Returns the same UrlBuilder object that can be used to concatenate other methods.</returns>
        public FluentUrlBuilder RemoveQueryStringPair(string key)
        {
            if (!string.IsNullOrWhiteSpace(key) && this.queryStringValues.ContainsKey(key))
            {
                this.queryStringValues.Remove(key);
            }
            return this;
        }


        /// <summary>
        /// Builds the URL and returns it as a string
        /// </summary>
        /// <returns>URL in the form of [baseUrl]/[parts]?[query-string]#[fragment]</returns>
        public string GetAsString()
        {
            var urlBuilder = new StringBuilder();

            for (int i = 0; i < urlParts.Count - 1; i++)
            {
                urlBuilder.Append(urlParts.ElementAt(i)).Append("/");

            }
            urlBuilder.Append(urlParts.Last());

            if (queryStringValues.Keys.Any())
            {
                urlBuilder.Append("?");

                var sel = queryStringValues.Select(x => $"{x.Key}={x.Value}");
                urlBuilder.Append(string.Join("&", sel));
            }
            if (!string.IsNullOrWhiteSpace(hashFragment))
                urlBuilder.Append($"#{hashFragment}");

            return urlBuilder.ToString();
        }

        /// <summary>
        /// Builds the URL and returns it as a Uri object
        /// </summary>
        /// <returns>A Uri object that represents the final result of the builder</returns>
        public Uri GetAsUri()
        {
            var url = this.GetAsString();
            return new Uri(url);
        }


        internal static string TrimString(string part)
        {
            if (string.IsNullOrWhiteSpace(part))
                return string.Empty;
            return part.Trim().Trim('/').Trim();
        }
    }
}
