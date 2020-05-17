using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

/*TODO:  
 1- rinominare Part come pathPart
 2- rinominare Hash come Fragment
 3- rinomina Solution e classe come FluentUrlBuilder
 4- trasforma InsertQS in UpsertQS
 5- trasforma InsertHash in UpsertHash
 6- Aggiungi overload in addPart, insertQS e insertHash per Predicate<T>=>bool per inserimento condizionale.
     */


[assembly: InternalsVisibleTo("UrlBuilder.Tests")]
namespace UrlBuilder
{
    /// <summary>
    /// Creates and build a URL. The building operations can be appendend.
    /// </summary>
    public class UrlBuilder
    {
        private StringBuilder urlBuilder;
        internal Dictionary<string, string> qs;
        internal string hashPart = "";

        private UrlBuilder(params string[] parts)
        {
            var cleanedParts = parts.Select(p => p.Trim('/')).Where(p => !string.IsNullOrWhiteSpace(p));
            urlBuilder = new StringBuilder(string.Join("/", cleanedParts));
            qs = new Dictionary<string, string>();
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
        public static UrlBuilder Initialize(params string[] parts)
        {
            return new UrlBuilder(parts);
        }

        /// <summary>
        /// Adds a path part to the builder, only if the part is valid.
        /// </summary>
        /// <param name="pathPart">part to be added to the builder</param>
        /// <returns>Returns the same UrlBuilder object that can be used to concatenate other methods.</returns>
        public UrlBuilder AddPart(string pathPart)
        {
            pathPart = pathPart.Trim('/');
            if (!string.IsNullOrWhiteSpace(pathPart))
                this.urlBuilder.Append('/').Append(pathPart);
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
        public UrlBuilder AddPart (string pathPart, bool add)
        {
            if (add)
                return this.AddPart(pathPart);
            return this;
        }

        /// <summary>
        /// Adds a Fragment to the URL, (ex: http://example.com/path#fragment)
        /// </summary>
        /// <param name="fragment">Fragment part to be added. If a fragment has already been set, it will be replaced with the new one</param>
        /// <returns>Returns the same UrlBuilder object that can be used to concatenate other methods.</returns>
        public UrlBuilder AddHashValue(string fragment)
        {
            if (!string.IsNullOrWhiteSpace(fragment))
                this.hashPart = fragment;
            return this;
        }

        /// <summary>
        /// Removes the Fragment part
        /// </summary>
        /// <returns>Returns the same UrlBuilder object that can be used to concatenate other methods.</returns>
        public UrlBuilder RemoveFragment()
        {
            this.hashPart = null;
            return this;
        }

        /// <summary>
        /// Adds a query string value.
        /// </summary>
        /// <param name="key">Key of the query string</param>
        /// <param name="value">Value of the query string</param>
        /// <returns>Returns the same UrlBuilder object that can be used to concatenate other methods.</returns>
        public UrlBuilder AddQueryStringPair(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(key))
                qs.Add(key, value);
            return this;
        }

        /// <summary>
        /// Removes a query string value.
        /// </summary>
        /// <param name="key">Key to be removed</param>
        /// <returns>Returns the same UrlBuilder object that can be used to concatenate other methods.</returns>
        public UrlBuilder RemoveQueryStringPair(string key)
        {
            if(!string.IsNullOrWhiteSpace(key) && this.qs.ContainsKey(key))
            {
                this.qs.Remove(key);
            }
            return this;
        }

        /// <summary>
        /// Replaces an existing query string value.
        /// </summary>
        /// <param name="key">Key to be added</param>
        /// <param name="value">Value to be added</param>
        /// <returns></returns>
        public UrlBuilder ReplaceQueryStringPair(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(key) && this.qs.ContainsKey(key))
            {
                this.qs[key] = value;
            }
            return this;
        }

        /// <summary>
        /// Builds the URL and returns it as a string
        /// </summary>
        /// <returns>URL in the form of [baseUrl]/[parts]?[query-string]#[fragment]</returns>
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

        /// <summary>
        /// Builds the URL and returns it as a Uri object
        /// </summary>
        /// <returns>A Uri object that represents the final result of the builder</returns>
        public Uri GetAsUri()
        {
            var url = this.GetResult();
            return new Uri(url);
        }
    }
}
