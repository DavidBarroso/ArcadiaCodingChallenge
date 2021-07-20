using Arcadia.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace OpenSkyClient
{
    /// <summary>
    /// OpenSkyClient
    /// </summary>
    public class OpenSkyClient
    {
        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        private string userName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        private string password { get; set; }

        /// <summary>
        /// The client
        /// </summary>
        private RestClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenSkyClient" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public OpenSkyClient(string host, string userName = null, string password = null)
        {
            Host = host;
            this.userName = userName;
            this.password = password;

            this.client = new RestClient(host);
            if (this.userName != null && this.password != null)
                client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(this.userName, this.password);
        }

        public string GetArrivalsJSON(string airportICAO, DateTime begin, DateTime end)
        {
            RestRequest rq = GetArrivalsRQ(airportICAO, begin, end);
            IRestResponse rs = client.Get(rq);
            if (rs.StatusCode == HttpStatusCode.OK && rs.ContentType.ToLower().Contains("json"))
                return rs.Content;

            return null;
        }

        public List<Arrivals> GetArrivals(string airportICAO, DateTime begin, DateTime end)
        {
            RestRequest rq = GetArrivalsRQ(airportICAO, begin, end);
            IRestResponse<List<Arrivals>> rs = client.Get<List<Arrivals>>(rq);
            if (rs.StatusCode == HttpStatusCode.OK)
                return rs.Data;

            return null;
        }

        /// <summary>
        /// Gets the arrivals by airport.
        /// </summary>
        /// <param name="airportICAO">The airport icao.</param>
        /// <param name="begin">The begin.</param>
        /// <param name="end">The end.</param>
        private RestRequest GetArrivalsRQ(string airportICAO, DateTime begin, DateTime end)
        {
            string resource = "/flights/arrival";

            KeyValuePair<string, object> airportParam = new KeyValuePair<string, object>("airport", airportICAO);
            KeyValuePair<string, object> beginParam = new KeyValuePair<string, object>("begin", GetUnixTime(begin));
            KeyValuePair<string, object> endParam = new KeyValuePair<string, object>("end", GetUnixTime(end));
            
            //MOCK DATA
            beginParam = new KeyValuePair<string, object>("begin", 1517227200);
            endParam = new KeyValuePair<string, object>("end", 1517230800);
            //END MOCK DATA

            RestRequest rq = CreateBaseRequest(resource, Method.GET, airportParam, beginParam, endParam);
            return rq;
        }

        /// <summary>
        /// Creates the request.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        private RestRequest CreateBaseRequest(string resource, Method method, params KeyValuePair<string, object>[] parameters)
        {
            if (string.IsNullOrEmpty(resource))
                return null;

            RestRequest rq = new RestRequest(resource, method, DataFormat.Json);
            if (parameters != null && parameters.Any())
                parameters.ToList().ForEach(parameter =>
                {
                    rq.AddParameter(parameter.Key, parameter.Value);
                });

            return rq;
        }

        /// <summary>
        /// Gets the unix time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        private int GetUnixTime(DateTime time)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var result = (int)time.ToUniversalTime().Subtract(epoch).TotalSeconds;
            if (result < 0)
                return -1;

            return result;
        }
    }
}
