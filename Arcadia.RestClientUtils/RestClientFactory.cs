using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Arcadia.RestClientUtils
{
    /// <summary>
    /// RestClientFactory
    /// </summary>
    public static class RestClientFactory
    {
        /// <summary>
        /// Creates the rest client.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static RestClient CreateRestClient(string host, string userName = null, string password = null)
        {
            RestClient client = new RestClient(host);
            if (userName != null && password != null)
                client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(userName, password);
            return client;
        }

        /// <summary>
        /// Creates the rest request.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="method">The method.</param>
        /// <param name="format">The format.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static RestRequest CreateRestRequest(string resource, Method method, DataFormat format, params KeyValuePair<string, object>[] parameters)
        {
            if (string.IsNullOrEmpty(resource))
                return null;

            RestRequest rq = new RestRequest(resource, method, format);
            if (parameters != null && parameters.Any())
                parameters.ToList().ForEach(parameter =>
                {
                    rq.AddParameter(parameter.Key, parameter.Value);
                });

            return rq;
        }

        /// <summary>
        /// Gets the date time as unix format.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public static int GetDateTimeAsUnixFormat(DateTime time)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var result = (int)time.ToUniversalTime().Subtract(epoch).TotalSeconds;
            if (result < 0)
                return -1;

            return result;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static T GetData<T>(string json)
        {
            JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return GetData<T>(json, options);
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The json.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static T GetData<T>(string json, JsonSerializerOptions options)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default(T);
            T result = JsonSerializer.Deserialize<T>(json, options);
            return result;
        }
    }
}
