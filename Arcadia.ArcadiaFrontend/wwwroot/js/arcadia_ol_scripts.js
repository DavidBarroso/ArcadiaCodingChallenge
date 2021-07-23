//Constructor
var olViewer = (function () {
    var map = null;
    var vectorLayer = null;
    var controlFeatures = null;
});
//Init map function
olViewer.prototype.Init = (function (targetId, items) {
    //Save airports
    controlFeatures = items;

    //Init layer
    vectorLayer = null;

    //Load map viewer
    map = new ol.Map({
        target: targetId,
        layers: [new ol.layer.Tile({ source: new ol.source.OSM() })],
        view: new ol.View({
            projection: 'EPSG:3857',
            center: [0, 0],
            zoom: 2
        })
    });
});
//Add arrival airport and departures airport to vector layer
olViewer.prototype.AddVectorLayer = (function (jsonFeatures) {
    this.ClearFeatures();
    if (jsonFeatures == null || jsonFeatures.length < 1)
        return;
    
    var features = [];

    //arrival feature
    var arrivalFeature = this.CreateFeature(this.GetControlFeature("icao", jsonFeatures[0].estArrivalAirport));
    arrivalFeature.setStyle(new ol.style.Style({
        image: new ol.style.Circle({
            fill: new ol.style.Fill({ color: '#4EE084' }),
            stroke: new ol.style.Stroke({ color: '#008236', width: 1 }),
            radius: 5
        }),
    }));
    arrivalFeature.setId('arrivalFeature');
    features.push(arrivalFeature);

    //departure features
    var index = 0;
    jsonFeatures.forEach((value) => {
        if (value != null) {
            var departureJSONObj = this.GetControlFeature("icao", value.estDepartureAirport);
            if (departureJSONObj != null) {
                var departureFeature = this.CreateFeature(departureJSONObj);
                departureFeature.setId('departureFeature' + index);
                features.push(departureFeature);
                index++;
            }
        }
    });

    //Vector layer
    var vectorSource = new ol.source.Vector({ features: features });
    vectorLayer = new ol.layer.Vector({
        source: vectorSource,
        style: new ol.style.Style({
            image: new ol.style.Circle({
                fill: new ol.style.Fill({ color: '#8CB2FF' }),
                stroke: new ol.style.Stroke({ color: '#306AFF', width: 1 }),
                radius: 5
            }),
        })
    });
    map.addLayer(vectorLayer);
    var test = vectorLayer.getSource().getFeatures();
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
olViewer.prototype.GetControlFeature = (function (key, value) {
    for (var i = 0; i < controlFeatures.length; i++) {
        var candidate = controlFeatures[i];
        if (candidate[key] == value) {
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