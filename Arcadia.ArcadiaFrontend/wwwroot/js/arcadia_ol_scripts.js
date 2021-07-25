//Constructor
var olViewer = (function () {
    var map = null;
    var vectorLayer = null;
    var airports = null;
    var overlay = null;
    var arrivals = null;
});
//Init map function
olViewer.prototype.Init = (function (targetId, popupContainer, items) {
    //Save airports
    airports = items;

    //Init arrivals
    arrivals = null;

    //Init layer
    vectorLayer = null;

    //Init popup
    overlay = new ol.Overlay({
        element: popupContainer,
        autoPan: true,
        autoPanAnimation: {
            duration: 250,
        },
    });

    //Load map viewer
    map = new ol.Map({
        target: targetId,
        layers: [new ol.layer.Tile({ source: new ol.source.OSM() })],
        overlays: [overlay],
        view: new ol.View({
            projection: 'EPSG:3857',
            center: [0, 0],
            zoom: 2,
            minZoom: 2
        })
    });
});
//Add arrival airport and departures airport to vector layer
olViewer.prototype.AddVectorLayer = (function (jsonFeatures) {
    arrivals = jsonFeatures;
    this.ClearFeatures();
    if (jsonFeatures == null || jsonFeatures.length < 1)
        return;
    
    var features = [];

    //departure features
    var index = 0;
    jsonFeatures.forEach((value) => {
        if (value != null) {
            var departureJSONObj = this.GetAirport("icao", value.estDepartureAirport);
            if (departureJSONObj != null) {
                var departureFeature = this.CreateFeature(departureJSONObj);
                departureFeature.setId('departureFeature' + index);
                features.push(departureFeature);
                index++;
            }
        }
    });

    //arrival feature
    var arrivalFeature = this.CreateFeature(this.GetAirport("icao", jsonFeatures[0].estArrivalAirport));
    arrivalFeature.setStyle(new ol.style.Style({
        image: new ol.style.Circle({
            fill: new ol.style.Fill({ color: '#4EE084' }),
            stroke: new ol.style.Stroke({ color: '#008236', width: 1 }),
            radius: 5,
            zIndex: 9999
        }),
    }));
    arrivalFeature.setId('arrivalFeature');
    features.push(arrivalFeature);

    //Vector layer
    var vectorSource = new ol.source.Vector({ features: features });
    vectorLayer = new ol.layer.Vector({
        source: vectorSource,
        style: new ol.style.Style({
            image: new ol.style.Circle({
                fill: new ol.style.Fill({ color: '#8CB2FF' }),
                stroke: new ol.style.Stroke({ color: '#306AFF', width: 1 }),
                radius: 5,
                zIndex : 9998
            }),
        })
    });
    map.addLayer(vectorLayer);

    //Center features
    this.CenterFeatures(vectorLayer.getSource().getFeatures());
});
//Create ol feature to add to map
olViewer.prototype.CreateFeature = (function (jsonObj) {
    if (jsonObj == null)
        return null;

    var lon = parseFloat(jsonObj.longitude);
    var lat = parseFloat(jsonObj.latitude);
    var feature = new ol.Feature({
        geometry: new ol.geom.Point(ol.proj.transform([lon, lat], 'EPSG:4326', 'EPSG:3857')),
        //geometry: new ol.geom.Point([jsonObj.longitude, jsonObj.latitude]),
        //labelPoint: new ol.geom.Point(jsonObj.name),
        //name: jsonObj.name,
    });
    return feature;
});
//Get airport feature
olViewer.prototype.GetAirport = (function (key, value) {
    for (var i = 0; i < airports.length; i++) {
        var candidate = airports[i];
        if (candidate[key] == value) {
            return candidate;
        }
    }
    return null;
});
//Filtered airport feature
olViewer.prototype.GetArrival = (function (keys, values) {
    if (keys != null && values != null && keys.length == values.length) {
        for (var i = 0; i < arrivals.length; i++) {
            var candidate = arrivals[i];
            var allKeysIsTrue = true;
            for (var j = 0; j < keys.length; j++) {
                if (candidate[keys[j]] != values[j]) {
                    allKeysIsTrue = false;
                }
            }
            if (allKeysIsTrue)
                return candidate;
        }
    }
    return null;
});
//Clean all feactures
olViewer.prototype.ClearFeatures = (function () {
    if (vectorLayer != null) {
        map.removeLayer(vectorLayer);
        vectorLayer = null;
    }

});
//Center features
olViewer.prototype.CenterFeatures = (function (features) {
    if (features == null || features.length < 1)
        return;

    var featuresExtent = features[0].getGeometry().getExtent();
    for (var i = 1; i < features.length; i++) {
        ol.extent.extend(featuresExtent, features[i].getGeometry().getExtent());
    }

    map.getView().fit(featuresExtent, {
        size: map.getSize(),
        padding: [10,10,10,10]
    });
    
});
//Open popup
olViewer.prototype.OpenPopup = (function (arrival, contentContainer) {
    var dAirport = this.GetAirport("icao", arrival.estDepartureAirport)
    var aAirport = this.GetAirport("icao", arrival.estArrivalAirport)

    var contentPopup = this.GetContentPopup(arrival, dAirport, aAirport);
    contentContainer.html(contentPopup);

    var lon = parseFloat(dAirport.longitude);
    var lat = parseFloat(dAirport.latitude);
    var coordinates = ol.proj.transform([lon, lat], 'EPSG:4326', 'EPSG:3857');
    overlay.setPosition(coordinates);
});
//Generate content pop up
olViewer.prototype.GetContentPopup = (function (arrival, dAirport, aAirport) {
    var baseContent = `<table>
                           <tr>
                               <td colspan="3" class="ol-popop-td">
                                   <b>Flight</b></br>
                                   ICAO-24bit: {icao24}</br>
                                   Callsign: {callsign}</br>
                                   Distance: {distance} km</br>
                               </td>
                           </tr>
                           <tr>
                               <td class="ol-popop-td">
                                   <b>Departure Airport</b></br>
                                   ICAO: {dICAO}</br>
                                   Name: {dIATAName}</br>
                                   Address: {dCountry} {dState} {dCity}</br>
                                   Coordinates: [{dLon},{dLat}]</br>
                                   Elevation: {dElevation} m
                               </td>
                               <td style="width:0.5rem"></td>
                               <td class="ol-popop-td">
                                   <b>Arrival Airport</b></br>
                                   ICAO: {aICAO}</br>
                                   Name: {aIATAName}</br>
                                   Address: {aCountry} {aState} {aCity}</br>
                                   Coordinates: [{aLon},{aLat}]</br>
                                   Elevation: {aElevation} m
                               </td>
                           </tr>
                       </table>`;

    baseContent = baseContent.replace('{icao24}', arrival.icao24);
    baseContent = baseContent.replace('{callsign}', arrival.callsign);
    baseContent = baseContent.replace('{distance}', Math.round((arrival.distanceToDepartureAirport + Number.EPSILON) * 100) / 100);

    baseContent = baseContent.replace('{dICAO}', dAirport.icao);
    baseContent = baseContent.replace('{dIATAName}', dAirport.name);
    baseContent = baseContent.replace('{dCountry}', dAirport.country);
    baseContent = baseContent.replace('{dState}', dAirport.state);
    baseContent = baseContent.replace('{dCity}', dAirport.city);
    baseContent = baseContent.replace('{dLon}', Math.round((dAirport.longitude + Number.EPSILON) * 1000) / 1000);
    baseContent = baseContent.replace('{dLat}', Math.round((dAirport.latitude + Number.EPSILON) * 1000) / 1000);
    baseContent = baseContent.replace('{dElevation}', Math.round((dAirport.elev + Number.EPSILON) * 1000) / 1000);

    baseContent = baseContent.replace('{aICAO}', aAirport.icao);
    baseContent = baseContent.replace('{aIATAName}', aAirport.name);
    baseContent = baseContent.replace('{aCountry}', aAirport.country);
    baseContent = baseContent.replace('{aState}', aAirport.state);
    baseContent = baseContent.replace('{aCity}', aAirport.city);
    baseContent = baseContent.replace('{aLon}', Math.round((aAirport.longitude + Number.EPSILON) * 1000) / 1000);
    baseContent = baseContent.replace('{aLat}', Math.round((aAirport.latitude + Number.EPSILON) * 1000) / 1000);
    baseContent = baseContent.replace('{aElevation}', Math.round((aAirport.elev + Number.EPSILON) * 1000) / 1000);

    return baseContent;
});
//Close popup
olViewer.prototype.ClosePopup = (function () {
    overlay.setPosition(undefined);
});