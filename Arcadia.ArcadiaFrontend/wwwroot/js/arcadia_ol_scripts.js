//Constructor
var olViewer = function () {
    var map = null;
    var vectorLayer = null;
    var vectorSource = null;
    var controlFeatures = null;
};
//Init map function
olViewer.prototype.Init = (function (targetId, items) {
    //Save airports
    controlFeatures = items;

    //Vector layer
    vectorSource = new ol.source.Vector();
    vectorLayer = new ol.layer.Vector({
        source: vectorSource,
        //style: new ol.style.Style({
        //    fill: new ol.style.Fill({ color: '#8CB2FF' }),
        //    stroke: new ol.style.Stroke({ color: '#306AFF', width: 1 }),
        //    zIndez: 99999999
        //})
        style: new ol.style.Style({
            image: new ol.style.Icon({
                anchor: [0.5, 46],
                anchorXUnits: 'fraction',
                anchorYUnits: 'pixels',
                src: 'https://www.iconpacks.net/icons/2/free-icon-airport-location-2959.png',
            })
        })
    });

    //Load map viewer
    map = new ol.Map({
        target: targetId,
        layers: [
            vectorLayer
            , new ol.layer.Tile({ source: new ol.source.OSM() })
        ],
        view: new ol.View({
            //projection: 'EPSG:4326',
            center: [0, 0],
            zoom: 2
        })
    });
});
//Add arrival airport and departures airport to vector layer
olViewer.prototype.AddFeatures = (function (jsonFeatures) {
    if (jsonFeatures == null || jsonFeatures.length < 1)
        return;

    //arrival feature
    var arrivalFeature = this.CreateFeature(this.GetControlFeature("icao", jsonFeatures[0].estArrivalAirport));
    arrivalFeature.setStyle(new ol.style.Style({
        fill: new ol.style.Fill({ color: '#FF0000' }),
        stroke: new ol.style.Stroke({ color: '#306AFF', width: 1 })
    }));
    vectorSource.addFeature(arrivalFeature);

    //departure features
    $.each(jsonFeatures, (index, value) => {
        if (value != null) {
            var departureJSONObj = this.GetControlFeature("icao", value.estDepartureAirport);
            if (departureJSONObj != null) {
                var departureFeature = this.GetControlFeature(departureJSONObj);
                try {
                    vectorSource.addFeature(departureFeature);
                } catch {
                }
            }
        }
    });
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
    vectorLayer.getSource().clear();

    //var features = vectorLayer.getSource().getFeatures();
    //features.forEach((feature) => {
    //    vectorLayer.getSource().removeFeature(feature);
    //});

});