# ArcadiaCodingChallenge
This application is a searcher and viewer of arrivals of any airport in the world.
The application uses the OpenSky Rest service to get information of arrivals and flights and a list of airports in a json list distributed with de system.
## Status of project ##
This project is finished following the specifications describes into “Arcadia coding challenge.pdf” fulfilling all the mandatory points and all the optional points excepts the authentication of the services.
In addition, has been implemented a detail info view as a popup in the viewer with more information about the flight and the departure and arrival airports. And also has been implemented a simulated route that’s link the departure airport with the arrival airport in the map viewer.
## Architecture of system: ##
The software is composed by two parts:
### The backend: ###
This is a microservice that consume the OpenSky API rest and expose the data into a private rest microservice with the airports.
This mentioned microservice, allows queries the airports by country or list of countries.
In addition, allows, filter query the arrivals by selected airport and a couple of dates. The begin date and the end dates, indicate ranges of date, that the flight arrival in the selected airport.
Before expose the data obtained from OpenSky, the arcadia backend service calculates the great circle distance between airports using a haversine formulation assuming a perfect sphere earth with 6371 km of diameter.
This information, distance, is added to the OpenSky arrival data to expose into rest service.
### The frontend: ###
This front is a responsive web application that consume the data exposed by the backend and show this in sortable, searchable and pageable grid and in map viewer that show the positions of all identified airports, departures and arrival airport.
The user can indicate the country or list of countries that he wants view the airports, select an airport via drop down list, select a couple of dates, begin and end, typing this into a text box, o via selection into a calendar/time control to searches all arrivals that compliance this filter.
Into the pageable arrivals grid, the user cans hide the columns, search by any field and order by any column.
Also, the user cans select a row of the arrivals grid, and the map viewer center de departure airport and show de detail info of the flight, departure airport and arrival airport. Besides the map viewer show a line that simulate the route between the two airports. This straight line, is not the real route, because is not a great circle line, only a simulated route across the straight-line distance.
Moreover, if the user moves the pointer mouse hover the airports painted into the map viewer, the same popup with arrival detail info is showed with the route between airports.
### Other support projects: ###
The application has another two support projects, 
#### Model ####
The first project is a collection of models shared by the backend and frontend that contains the definition of arrival and airports.
It is necessary knows the airport model to change or update the airport list.
This model defines a lot of optional properties, and the json file can be provide data to all or some of these properties for each airport.
        
        /// <summary>
        /// Gets or sets the icao.
        /// </summary>
        /// <value>
        /// The icao.
        /// </value>
        public string Icao { get; set; }

        /// <summary>
        /// Gets or sets the iata.
        /// </summary>
        /// <value>
        /// The iata.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the elevation.
        /// </summary>
        /// <value>
        /// The elevation.
        /// </value>
        public string Elev { get; set; }

        /// <summary>
        /// Gets or sets the lat.
        /// </summary>
        /// <value>
        /// The lat.
        /// </value>
        public string Lat { get; set; }        

        /// <summary>
        /// Gets or sets the lon.
        /// </summary>
        /// <value>
        /// The lon.
        /// </value>
        public string Lon { get; set; }        


        /// <summary>
        /// Gets or sets the tz.
        /// </summary>
        /// <value>
        /// The tz.
        /// </value>
        public string Tz { get; set; }
 
#### RestClient.Utils ####
The second support project is a helper lib to provide the infrastructure to connect easily with OpenSky from the backend and connect with the backend from the frontend.
## Technology of system: ##
Generally, all the project is made with .NET core v3.1 for the backend parts, models and server side.
The backend has two controllers that provide two endpoints to consume the service, airports and arrivals.
Sometime I thought to separate the two endpoints into two individual services, following more restrictive the microservice idea. But I think that these two services must be share information and functionality and the other solution may be down into over architecture.
For the client side, I use a cluster of technologies, like aspnet core v3.1 with MVC and bootstrap for the basic architecture, together with jquery and openlayers v6 JavaScript API for the map viewer.
Special mention, to the olViewer class to create the map viewer, that has been done using the technique of adding methods to the prototype, writing the global variables in the constructor and adding the methods afterwards. 
Also, the system uses a RestSharp library, that it is an implementation rest client to connect with REST services.
Deployment:
All the systems can be deployed into dockers containers, I provide the Dockerfile for backend and frontend to build the images to deploy into Dockerdesktop o any docker services, always with Linux as operative system.
In addition, provides the docker-compose.yml to deploy the both images of backend and frontend altogether into separates dockers like separated services, but with only one command.
In the docker-compose has configurated a shared networks that both services use to communicated with each other.
Config files:
The two subsystems, backend and frontend, has two config files called appsettings.json that provide some configuration keys to personalize the software and behaviors.
Backend config file:
This file has the following sections with the following keys:
```
{
  /*
  Section Logging: Provide the configuration of levels of logging. 
  This section is mandatory for use log and this current configuration is the default configuration.
  */
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  /* This key allows change the hostname of this service */
  "AllowedHosts": "*",
  /*
  Section OpenSkyRestAPIConfiguration: Provide configuration to connect with OpenSky, like a host, endpoints and credentials.
  This section is optional, and the system use the default values if not exists.
  The default credentials values are, no credentials.
  */
  "OpenSkyRestAPIConfiguration": {
    "Host": "https://opensky-network.org/api",
    "ArrivalResource": "/flights/arrival",
    "UserName": "",
    "Pass": ""
  },
  /*
  Section AirportsConfig: Allow configurate the path where the system search the airport list.
  This section is optional, and the system use the default values if not exists.
  The airports.json file must comply the correct format defining in model.
  */
  "AirportsConfig": {
    "FilePath": "./Resources/airports.json"
  }
}
```

Frontend config file:
This file has the following sections with the following keys:

```
{
  /*
  Section Logging: Provide the configuration of levels of logging. 
  This section is mandatory for use log and this current configuration is the default configuration.
  */
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  /* This key allows change the hostname of this service */
  "AllowedHosts": "*",
  /*
  Section ArcadiaBackend: Provide configuration to connect with the backend, like a host, endpoints and initial selected countries.
  This section is optional, and the system use the default values if not exists.
  */

  "ArcadiaBackend": {
    "Host": "http://arcadia.arcadiabackend",
    "ArrivalResource": "arrivals",
    "AirportResource": "airports",
    "AirportInitCountries": "Spain, Germany"
  }
}
```

## Management: ##
This project has been made following a limited and particular agile methodology, using a basic Kanban panel with three columns, (TO DO, IN PROGRESS, DONE) to follow the progress of the tasks and bugs.
The development it is made into separated branch for each task and bugs and the pull requests has been used to merge from developer branch to main branch, that contains the final version of the code, including this readme.md file.

Please enjoy using the __Arcadia Coding Challenge__ as I enjoyed doing it :wink:
