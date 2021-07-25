using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcadia.ArcadiaFrontend.Controllers
{
    /// <summary>
    /// BaseClientController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class BaseClientController: Controller
    {
        /// <summary>
        /// The host
        /// </summary>
        private readonly string host;

        /// <summary>
        /// The client
        /// </summary>
        protected RestClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseClientController" /> class.
        /// </summary>
        public BaseClientController()
        {
            this.host = "http://localhost:49161";
            client = new RestClient(host);
        }
    }
}
